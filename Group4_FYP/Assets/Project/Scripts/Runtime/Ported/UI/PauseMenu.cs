using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using DG.Tweening;
using PathOfHero.Controllers;

public class PauseMenu : Panel
{
    private bool isOpened;
    [SerializeField] private GameObject hudPanel;
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private GameObject sideMenuPanel;
    [SerializeField] private GameObject settingsMenuPanel;
    [SerializeField] private GameObject pauseMenuBackground;
    [SerializeField] private SceneController sceneController;
    [SerializeField] private SettingsMenu settingsMenu;

    [SerializeField] private Button giveUpButton;
    [SerializeField] private Button saveButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private GameObject warning;

    private InputAction showPauseAction;
    private InputAction saveGameAction;
    private InputAction exitToMenuAction;

    private static PauseMenu m_instance;
    public static PauseMenu Instance => m_instance;

    protected override void Awake()
    {
        base.Awake();

        if (!m_instance)
            m_instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        // base.Start();

        isOpened = false;

        // code for showing hudpanel and hiding osdpanel
        //hudPanel.SetActive(true);
        pauseMenuBackground.SetActive(false);
        pauseMenuPanel.SetActive(false);

        pauseMenuBackground.GetComponent<Image>().color = new Color32(0, 0, 0, 0);
        sideMenuPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(-sideMenuPanel.GetComponent<RectTransform>().sizeDelta.x, 0);
        settingsMenuPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(settingsMenuPanel.GetComponent<RectTransform>().sizeDelta.x, 0);

        showPauseAction = playerInput.actions["ShowPause"];
        saveGameAction = playerInput.actions["SaveGame"];
        exitToMenuAction = playerInput.actions["ExitToMenu"];

        showPauseAction.Enable();
        saveGameAction.Enable();
        exitToMenuAction.Enable();
    }

    // Update is called once per frame
    // protected async override void Update()
    void Update()
    {
        // base.Update();

        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    if (isOpened)
        //    {
        //        await HidePauseMenu(true);
        //    }
        //    else
        //    {
        //        /*await */ShowPanel();
        //    }
        //}

        if (showPauseAction.triggered)
        {
            if (GameManager.Instance.GameState == GameState.Playing)
                ShowPanel();
        }

        if (isOpened) // not finished: also check if saving is allowed atm
        {
            // if (Input.GetKeyDown(KeyCode.BackQuote))
            if (saveGameAction.triggered)
                SaveGame();
            // if (Input.GetKeyDown(KeyCode.End))
            if (exitToMenuAction.triggered)
                ExitToMenu();
        }

#if UNITY_EDITOR
        // test only
        if (Input.GetKeyDown(KeyCode.Alpha9))
            SetDungeonMode(true);
        if (Input.GetKeyDown(KeyCode.Alpha0))
            SetDungeonMode(false);
#endif
    }

    //public async Task ShowPauseMenu()
    public async override void ShowPanel()
    {
        base.ShowPanel();

        GameManager.Instance.GameState = GameState.Paused;

        //HUD.Instance.HideHUD();

        pauseMenuBackground.SetActive(true);
        pauseMenuPanel.SetActive(true);
        // hudPanel.GetComponent<CanvasGroup>().DOFade(0, 0.25f);

        pauseMenuBackground.GetComponent<Image>().DOFade(0.5f, 0.25f).SetEase(Ease.OutQuart);
        sideMenuPanel.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 0), 0.25f).SetEase(Ease.OutQuart);
        settingsMenuPanel.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 0), 0.25f).SetEase(Ease.OutQuart);

        await Task.Delay(250);

        Time.timeScale = 0;
        pauseMenuPanel.SetActive(true);
        // hudPanel.SetActive(false);

        isOpened = true;
    }

    public async Task HidePauseMenu(bool bShowHudPanel)
    {
        Time.timeScale = 1;

        if (bShowHudPanel)
        {
            // hudPanel.SetActive(true);
            // hudPanel.GetComponent<CanvasGroup>().DOFade(1, 0.25f);
            //HUD.Instance.ShowHUD();
        }

        pauseMenuBackground.GetComponent<Image>().DOFade(0, 0.25f).SetEase(Ease.OutQuart);
        sideMenuPanel.GetComponent<RectTransform>().DOAnchorPos(new Vector2(-sideMenuPanel.GetComponent<RectTransform>().sizeDelta.x, 0), 0.25f).SetEase(Ease.OutQuart);
        settingsMenuPanel.GetComponent<RectTransform>().DOAnchorPos(new Vector2(settingsMenuPanel.GetComponent<RectTransform>().sizeDelta.x, 0), 0.25f).SetEase(Ease.OutQuart);

        await Task.Delay(250);

        pauseMenuBackground.SetActive(false);
        pauseMenuPanel.SetActive(false);
        // if (bShowHudPanel)
        // {
        //     hudPanel.SetActive(true);
        //     hudPanel.GetComponent<CanvasGroup>().alpha = 1;
        // }

        settingsMenu.SaveSettings();
        GameManager.Instance.GameState = GameState.Playing;

        isOpened = false;
    }

    public override void HidePanel()
    {
        base.HidePanel();
        _ = HidePauseMenu(true);
        // ResumeGame();
    }

    // public void ResumeGame()
    // {
    //     _ = HidePauseMenu(true);
    // }

    public void GiveUp()
    {
        HidePanel();

        ConfirmationPanel.Instance.ShowConfirmationPanel
        (
            "Give Up",
            "You will lose all your progress earned in this dungeon!\nAre you sure you want to give up?",
            () => { GameManager.Instance.GiveUp(); },
            true
        );
    }

    public void SaveGame()
    {
        // await HidePauseMenu(true);
        HidePanel();
        settingsMenu.SaveSettings();
        SaveSystem.Instance.SaveData();
    }

    public async void ExitToMenu()
    {
        await HidePauseMenu(false);
        // await Task.Delay(250);

        // save settings to device
        settingsMenu.SaveSettings();

        // save data to profile
        SaveSystem.Instance.SaveData(false);
        if (File.Exists(ProfileManagerJson.GetHeroProfileDirectoryPath() + "_testprofile.heroprofile"))
        {
            ProfileManagerJson.DeleteProfile("_testprofile", false); // test only
        }

        // exit to menu
        //sceneController.ChangeScene("StartScene");
        SceneController.Instance.ChangeScene("StartScene", false);
        AudioManager.Instance.StopMusic();
    }

    public void SetDungeonMode(bool value)
    {
        if (value)
        {
            giveUpButton.gameObject.SetActive(true);
            saveButton.interactable = false;
            exitButton.interactable = false;
            warning.SetActive(true);
        }
        else
        {
            giveUpButton.gameObject.SetActive(false);
            saveButton.interactable = true;
            exitButton.interactable = true;
            warning.SetActive(false);
        }
    }
}
