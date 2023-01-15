using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject[] panels;
    [SerializeField] private GameObject profileSelectionPanel;
    [SerializeField] private GameObject profileCreationPanel;
    [SerializeField] private GameObject profileEditPanel;

    [SerializeField] private SceneController sceneController;
    [SerializeField] private ConfirmationPanel confirmationPanel;

    [SerializeField] private InputField profileCreationInputField;
    [SerializeField] private InputField profileEditInputField;

    [SerializeField] private GameObject profileButtonTemplate;
    [SerializeField] private Transform profileSelectionContentPanel;

    [SerializeField] private Button editButton;
    [SerializeField] private Button startButton;

    public enum PanelType { start, profileSelection, profileCreation, profileEdit };
    private bool bHasEntered;
    private CanvasGroup profileSelectionPanelCanvasGroup;
    private CanvasGroup profileCreationPanelCanvasGroup;
    private CanvasGroup profileEditPanelCanvasGroup;
    // make variable to store profile buttons index
    private List<GameObject> profileButtons;
    private string selectedProfileName;

    // Start is called before the first frame update
    void Start()
    {
        bHasEntered = false;
        profileButtons = new List<GameObject>();
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

        // play startmenu music
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
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

        await startPanel.GetComponent<CanvasGroup>().DOFade(0, 0.25f).SetEase(Ease.Linear).AsyncWaitForCompletion();
        startPanel.SetActive(false);

        RefreshProfileSelectionPanel();
        await ShowPanel(PanelType.profileSelection);

        bHasEntered = true;
    }

    public void CreateProfile()
    {
        // not yet implemented: regex function/class for input field?

        if (profileCreationInputField.text != null && profileCreationInputField.text != "")
        {
            if (ProfileManager.CreateProfile(profileCreationInputField.text))
            {
                //sceneController.ChangeScene("PlayScene"); // see comment @SceneController for this function

                ShowProfileSelectionPanel();
            }
        }
        else
        {
            Debug.LogWarning("profile name cannot be empty.");

            // show error
        }
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
            if (ProfileManager.UpdateProfile(selectedProfileName, profileEditInputField.text))
            {
                ShowProfileSelectionPanel();
            }
        }
        else
        {
            Debug.LogWarning("profile name cannot be empty.");

            // show error
        }
    }

    public void DeleteProfile()
    {
        if (ProfileManager.DeleteProfile(selectedProfileName, true))
        {
            ShowProfileSelectionPanel();
        }
    }

    public void StartGame() // load selected profile data then enter GameScene // not finished
    {
        //sceneController.EnterScene();
        sceneController.EnterPlayScene();
    }

    public async void ShowProfileSelectionPanel() // hide every other panels then show profileSelectionPanel
    {
        // load all profiles data
        RefreshProfileSelectionPanel();
        selectedProfileName = null;
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
    }

    private void RefreshProfileSelectionPanel() // destroy buttons, get profiles and add buttons back
    {
        for (int i = 0; i < profileButtons.Count; i++) // this loop destroy all buttons
        {
            Destroy(profileButtons[i]);
        }

        profileButtons.Clear();
        ProfileData[] profiles = ProfileManager.GetProfiles();

        for (int i = 0; i < profiles.Length; i++)
        {
            GameObject clone = Instantiate(profileButtonTemplate, profileSelectionContentPanel);

            RecursiveFindChild(clone.transform, "Name").GetComponent<Text>().text = profiles[i].profileName;
            // not finished
            // add if statement to determine hero class then show the appropriate image

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
