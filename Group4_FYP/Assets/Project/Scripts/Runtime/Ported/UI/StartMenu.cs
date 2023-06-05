using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using DG.Tweening;
using PathOfHero.Controllers;
using PathOfHero.Others;
using PathOfHero.PersistentData;
using PathOfHero.UI;
using PathOfHero.Utilities;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class StartMenu : Singleton<StartMenu>
{
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject[] panels;
    [SerializeField] private GameObject profileSelectionPanel;
    [SerializeField] private GameObject profileCreationPanel;
    [SerializeField] private GameObject profileCreationClassContainer;
    [SerializeField] private GameObject profileEditPanel;

    //[SerializeField] private SceneController sceneController;

    [SerializeField] private Text enterGameHintText;
    [SerializeField] private InputField profileCreationInputField;
    [SerializeField] private InputField profileEditInputField;

    [SerializeField] private GameObject profileButtonTemplate;
    [SerializeField] private Transform profileSelectionContentPanel;

    [SerializeField] private Button editButton;
    [SerializeField] private Button startButton;

    [SerializeField] private HeroList heroList;
    [SerializeField] private GameObject heroButtonPrefab;
    [SerializeField] private AudioClip enterGameSound;

    [SerializeField] private InputReader m_InputReader;
    [SerializeField] private InputActionReference m_EnterGameAction;

    public enum PanelType { start, profileSelection, profileCreation, profileEdit };
    private bool hasEntered;
    // make variable to store profile buttons index
    private List<GameObject> profileButtons;
    private string selectedProfileName;
    private HeroClass? selectedClassType;

    private void OnEnable()
    {
        m_InputReader.EnterGame += EnterGame;
    }

    private void OnDisable()
    {
        m_InputReader.EnterGame -= EnterGame;
    }

    void Start()
    {
        m_InputReader.EnableInput(InputReader.ActionMapType.Gameplay);
        hasEntered = false;
        profileButtons = new List<GameObject>();
        PrepareClassContainer();
        SetBottomButtonsInteractable(false);

        // code for showing the startPanel and hiding all other panels
        for (int i = 0; i < panels.Length; i++)
        {
            if (i == 0)
            {
                panels[i].SetActive(true);
                panels[i].GetComponent<CanvasGroup>().alpha = 1;
            }
            else
            {
                panels[i].SetActive(false);
                panels[i].GetComponent<CanvasGroup>().alpha = 0;
            }
        }

        enterGameHintText.text = $"- Press {m_EnterGameAction.action.GetBindingDisplayString()} to Continue -";
    }

    private async void EnterGame() // hide startPanel then show profileSelectionPanel
    {
        if (hasEntered)
            return;

        hasEntered = true;

        // play some sound effects maybe
        AudioManager.Instance.PlaySound(enterGameSound);

        await startPanel.GetComponent<CanvasGroup>().DOFade(0, 0.25f).SetEase(Ease.Linear).AsyncWaitForCompletion();
        startPanel.SetActive(false);

        RefreshProfileSelectionPanel();
        await ShowPanel(PanelType.profileSelection);
    }

    public void CreateProfile()
    {
        if (string.IsNullOrWhiteSpace(profileCreationInputField.text))
        {
            _ = Notification.Instance.ShowNotificationAsync("Profile name cannot be empty");
            return;
        }

        if (!selectedClassType.HasValue)
        {
            _ = Notification.Instance.ShowNotificationAsync("Please select a class");
            return;
        }

        var name = profileCreationInputField.text;
        var heroClass = selectedClassType.Value;
        var classDefault = heroList.GetHeroInfo(heroClass)?.defaultStats;
        if (HeroProfile.Create(name, heroClass, classDefault, out _))
            ShowProfileSelectionPanel();
    }

    private void PrepareClassContainer()
    {
        if (heroList == null)
        {
            Debug.LogError("Missing Hero List");
            return;
        }

        if (heroButtonPrefab == null)
        {
            Debug.LogError("Missing Hero Button Prefab");
            return;
        }

        foreach (var hero in heroList.heros)
        {
            var buttonObj = Instantiate(heroButtonPrefab, profileCreationClassContainer.transform);
            if (buttonObj == null)
            {
                Debug.LogError("Unable to instantiate prefab");
                continue;
            }

            var button = buttonObj.GetComponent<Button>();
            if (button != null)
                button.onClick.AddListener(() => { SelectClass(hero.heroClass); });

            var image = Common.RecursiveFindChild(buttonObj.transform, "Image");
            if (image != null)
                (image.GetComponent<Image>()).sprite = hero.heroInfo.heroSprite;

            var text = Common.RecursiveFindChild(buttonObj.transform, "Text");
            if (text != null)
                (text.GetComponent<Text>()).text = hero.heroInfo.heroName;

            buttonObj.transform.SetParent(profileCreationClassContainer.transform);
        }
    }

    public void SelectClass(HeroClass type)
        => selectedClassType = type;

    public void SelectProfile(string profileName)
    {
        selectedProfileName = profileName;

        // make edit and start button interactable
        SetBottomButtonsInteractable(true);

        PlayerPrefs.SetString("selectedProfileName", selectedProfileName);
    }

    public void UpdateProfile()
    {
        if (profileEditInputField == null || string.IsNullOrWhiteSpace(profileEditInputField.text))
        {
            _ = Notification.Instance.ShowNotificationAsync("Profile name cannot be empty");
            return;
        }

        if (HeroProfile.Update(selectedProfileName, profileEditInputField.text))
            ShowProfileSelectionPanel();
    }

    public void DeleteProfile()
    {
        ConfirmationPanel.Instance.ShowConfirmationPanel
        (
            "Delete Profile",
            "Upon deletion, all your data will be lost. Do you really wish to continue?",
            () =>
            {
                HeroProfile.Delete(selectedProfileName);
                ShowProfileSelectionPanel();
            }, true);
    }

    public void StartGame() // load selected profile data then enter GameScene // not finished
    {
        SceneController.Instance.ChangeScene("InGameScene", true);
        AudioManager.Instance.StopMusic();
    }

    public async void ShowProfileSelectionPanel() // hide every other panels then show profileSelectionPanel
    {
        // load all profiles data
        RefreshProfileSelectionPanel();
        PlayerPrefs.SetString("selectedProfileName", null);
        SetBottomButtonsInteractable(false);
        await ShowPanel(PanelType.profileSelection);

        ResetInputs();
    }

    public async void ShowProfileCreationPanel() // hide every other panels then show profileCreationPanel
    {
        await ShowPanel(PanelType.profileCreation);
    }

    public async void ShowProfileEditPanel() // hide every other panels then show profileEditPanel
    {
        // load profile data
        profileEditInputField.text = selectedProfileName;

        await ShowPanel(PanelType.profileEdit);
    }

    public void QuitGame()
    {
        ConfirmationPanel.Instance.ShowConfirmationPanel("Quit Game", "Take a rest?", async () =>
        {
            AudioManager.Instance.StopMusic();

            //await MaskingCanvas.Instance.ShowMaskingCanvas(true); 
            await LoadingScreen.Instance.FadeInAsync();
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }, false);
    }

    private async Task ShowPanel(PanelType panelType) // show inputted panelType and hide all the others
    {
        for (int i = 0; i < panels.Length; i++)
        {
            if (i == (int)panelType)
            {
                panels[i].SetActive(true);
                panels[i].GetComponent<CanvasGroup>().DOFade(1, 0.25f).SetEase(Ease.OutQuart);
            }
            else
                panels[i].GetComponent<CanvasGroup>().DOFade(0, 0.25f).SetEase(Ease.OutQuart);
        }

        await Task.Delay(250);

        for (int i = 0; i < panels.Length; i++)
        {
            if (i == (int)panelType)
            {
                panels[i].SetActive(true);
                panels[i].GetComponent<CanvasGroup>().alpha = 1;
            }
            else
            {
                panels[i].SetActive(false);
                panels[i].GetComponent<CanvasGroup>().alpha = 0;
            }
        }
    }

    private void ResetInputs() // called by ShowProfileSelectionPanel() to reset inputs if the player clicks "Cancel" button
    {
        profileCreationInputField.text = null;
        profileEditInputField.text = null;
        selectedClassType = null;
        selectedProfileName = null;
    }

    private void RefreshProfileSelectionPanel() // destroy buttons, get profiles and add buttons back
    {
        for (int i = 0; i < profileButtons.Count; i++) // this loop destroy all buttons
            Destroy(profileButtons[i]);

        profileButtons.Clear();
        var profiles = HeroProfile.GetSavedProfiles();
        foreach (var profile in profiles)
        {
            GameObject clone = Instantiate(profileButtonTemplate, profileSelectionContentPanel);

            Common.RecursiveFindChild(clone.transform, "Name").GetComponent<Text>().text = profile.DisplayName;
            Common.RecursiveFindChild(clone.transform, "Class").GetComponent<Text>().text = "Class " + profile.Class;
            Common.RecursiveFindChild(clone.transform, "Level").GetComponent<Text>().text = "Level " + profile.Level;

            // assigning image
            for (int j = 0; j < heroList.heros.Length; j++)
            {
                if (profile.Class == heroList.heros[j].heroClass)
                    Common.RecursiveFindChild(clone.transform, "Image").GetComponent<Image>().sprite = heroList.heros[j].heroInfo.heroSprite;
            }

            clone.GetComponent<Button>().onClick.AddListener(() => SelectProfile(profile.DisplayName));
            clone.SetActive(true);
            profileButtons.Add(clone);
        }
    }

    private void SetBottomButtonsInteractable(bool value)
    {
        editButton.interactable = value;
        startButton.interactable = value;
    }
}
