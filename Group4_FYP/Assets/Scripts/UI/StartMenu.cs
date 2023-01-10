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
    [SerializeField] private InputField[] inputFields; // for ResetInputs() function only;

    public enum PanelType { start, profileSelection, profileCreation, profileEdit };
    private bool bHasEntered;
    private CanvasGroup profileSelectionPanelCanvasGroup;
    private CanvasGroup profileCreationPanelCanvasGroup;
    private CanvasGroup profileEditPanelCanvasGroup;
    // make variable to store profile buttons index

    // Start is called before the first frame update
    void Start()
    {
        bHasEntered = false;

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

        await ShowPanel(PanelType.profileSelection);

        bHasEntered = true;
    }

    public async void ShowProfileSelectionPanel() // hide every other panels then show profileSelectionPanel
    {
        // load all profiles data

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

        await ShowPanel(PanelType.profileEdit);
    }

    public async void StartGame() // load selected profile data then enter GameScene // not finished
    {
        // create/load profile data

        // enter scene
        //sceneController.ChangeScene();
        sceneController.EnterScene();
    }

    public async void UpdateProfile()
    {

    }

    public async void DeleteProfile()
    {

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
        for (int i = 0; i < inputFields.Length; i++)
        {
            inputFields[i].text = "";
        }
    }
}
