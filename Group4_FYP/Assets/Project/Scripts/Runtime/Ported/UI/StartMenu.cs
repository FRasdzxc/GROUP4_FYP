using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEditor;
using DG.Tweening;
using PathOfHero.UI;
using PathOfHero.Controllers;
using PathOfHero.Utilities;

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

    public enum PanelType { start, profileSelection, profileCreation, profileEdit };
    private bool bHasEntered;
    // make variable to store profile buttons index
    private List<GameObject> profileButtons;
    private string selectedProfileName;
    private HeroClass? selectedClassType;

    private PlayerInput playerInput;
    private InputAction enterGameAction;

    public static event Action onPlayerSetUp;

    protected override void Awake()
    {
        base.Awake();
        playerInput = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();
    }

    // Start is called before the first frame update
    void Start()
    {
        bHasEntered = false;
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

        enterGameAction = playerInput.actions["EnterGame"];
        enterGameAction.Enable();
        enterGameHintText.text = $"- Press {enterGameAction.GetBindingDisplayString()} to Continue -";

        onPlayerSetUp?.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Space))
        if (enterGameAction.triggered)
        {
            if (!bHasEntered)
            {
                EnterGame();
            }
        }
    }

    private async void EnterGame() // hide startPanel then show profileSelectionPanel
    {
        // play some sound effects maybe
        AudioManager.Instance.PlaySound(enterGameSound);

        await startPanel.GetComponent<CanvasGroup>().DOFade(0, 0.25f).SetEase(Ease.Linear).AsyncWaitForCompletion();
        startPanel.SetActive(false);

        RefreshProfileSelectionPanel();
        await ShowPanel(PanelType.profileSelection);

        bHasEntered = true;
    }

    public void CreateProfile()
    {
        if (profileCreationInputField.text != null && profileCreationInputField.text != "")
        {
            if (selectedClassType.HasValue)
            {
                if (ProfileManagerJson.CreateProfile(profileCreationInputField.text, selectedClassType.Value, heroList.GetHeroInfo(selectedClassType.Value)))
                {
                    //sceneController.ChangeScene("PlayScene"); // see comment @SceneController for this function

                    ShowProfileSelectionPanel();
                }
            }
            else
            {
                _ = Notification.Instance.ShowNotificationAsync("Please select a class");
            }
        }
        else
        {
            // show error
            _ = Notification.Instance.ShowNotificationAsync("Profile name cannot be empty");
        }
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

            var image = RecursiveFindChild(buttonObj.transform, "Image");
            if (image != null)
                (image.GetComponent<Image>()).sprite = hero.heroInfo.heroSprite;

            var text = RecursiveFindChild(buttonObj.transform, "Text");
            if (text != null)
                (text.GetComponent<Text>()).text = hero.heroInfo.heroName;

            buttonObj.transform.SetParent(profileCreationClassContainer.transform);
        }
    }

    public void SelectClass(HeroClass type)
    {
        selectedClassType = type;
    }

    public void SelectProfile(string profileName)
    {
        selectedProfileName = profileName;

        // make edit and start button interactable
        SetBottomButtonsInteractable(true);

        PlayerPrefs.SetString("selectedProfileName", selectedProfileName);
    }

    public void UpdateProfile()
    {
        if (profileEditInputField.text != null && profileEditInputField.text != "")
        {
            if (ProfileManagerJson.UpdateProfile(selectedProfileName, profileEditInputField.text))
            {
                ShowProfileSelectionPanel();
            }
        }
        else
        {
            // show error
            _ = Notification.Instance.ShowNotificationAsync("Profile name cannot be empty");
        }
    }

    public void DeleteProfile()
    {
        ConfirmationPanel.Instance.ShowConfirmationPanel
        (
            "Delete Profile",
            "Upon deletion, all your data will be lost. Do you really wish to continue?",
            () =>
            {
                ProfileManagerJson.DeleteProfile(selectedProfileName);
                ShowProfileSelectionPanel();
            }, true);
    }

    public void StartGame() // load selected profile data then enter GameScene // not finished
    {
        //SceneController.Instance.ChangeScene("PlayScene", true);
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
            {
                panels[i].GetComponent<CanvasGroup>().DOFade(0, 0.25f).SetEase(Ease.OutQuart);
            }
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
        if (File.Exists(ProfileManagerJson.GetHeroProfileDirectoryPath() + "_testprofile.heroprofile"))
        {
            ProfileManagerJson.DeleteProfile("_testprofile", false); // test only
        }

        for (int i = 0; i < profileButtons.Count; i++) // this loop destroy all buttons
        {
            Destroy(profileButtons[i]);
        }

        profileButtons.Clear();
        ProfileData[] profiles = ProfileManagerJson.GetProfiles();

        for (int i = 0; i < profiles.Length; i++)
        {
            GameObject clone = Instantiate(profileButtonTemplate, profileSelectionContentPanel);

            RecursiveFindChild(clone.transform, "Name").GetComponent<Text>().text = profiles[i].profileName;
            RecursiveFindChild(clone.transform, "Class").GetComponent<Text>().text = "Class " + profiles[i].heroClass;
            RecursiveFindChild(clone.transform, "Level").GetComponent<Text>().text = "Level " + profiles[i].level;

            // assigning image
            for (int j = 0; j < heroList.heros.Length; j++)
            {
                if ((HeroClass)Enum.Parse(typeof(HeroClass), profiles[i].heroClass) == heroList.heros[j].heroClass)
                {
                    RecursiveFindChild(clone.transform, "Image").GetComponent<Image>().sprite = heroList.heros[j].heroInfo.heroSprite;
                }
            }

            int i2 = i; // https://answers.unity.com/questions/1271901/index-out-of-range-when-using-delegates-to-set-onc.html
            clone.GetComponent<Button>().onClick.AddListener(() => SelectProfile(profiles[i2].profileName));
            clone.SetActive(true);
            profileButtons.Add(clone);
        }
    }

    private Transform RecursiveFindChild(Transform parent, string childName)
    {
        foreach (Transform child in parent)
        {
            if (child.name == childName)
            {
                return child;
            }
            else
            {
                Transform child2 = RecursiveFindChild(child, childName);
                if (child2 != null)
                {
                    return child2;
                }
            }
        }

        return null;
    }

    private void SetBottomButtonsInteractable(bool value)
    {
        editButton.interactable = value;
        startButton.interactable = value;
    }
}
