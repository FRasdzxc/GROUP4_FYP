using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using PathOfHero.Controllers;
using PathOfHero.Managers;

public class PauseMenu : PanelOverride
{
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

    [SerializeField]
    private InputReader m_InputReader;

    private static PauseMenu m_instance;
    public static PauseMenu Instance => m_instance;

    protected override void Awake()
    {
        base.Awake();

        if (!m_instance)
            m_instance = this;
    }

    private void OnEnable()
    {
        m_InputReader.ShowPause += OnShowPause;
        m_InputReader.SaveGame += SaveGame;
        m_InputReader.ExitToMenu += ExitToMenu;
    }

    private void OnDisable()
    {
        m_InputReader.ShowPause -= OnShowPause;
        m_InputReader.SaveGame -= SaveGame;
        m_InputReader.ExitToMenu -= ExitToMenu;
    }

    // Start is called before the first frame update
    void Start()
    {
        // isOpened = false;

        pauseMenuBackground.SetActive(false);
        pauseMenuPanel.SetActive(false);

        pauseMenuBackground.GetComponent<Image>().color = new Color32(0, 0, 0, 0);
        sideMenuPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(-sideMenuPanel.GetComponent<RectTransform>().sizeDelta.x, 0);
        settingsMenuPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(settingsMenuPanel.GetComponent<RectTransform>().sizeDelta.x, 0);
    }

    // Update is called once per frame
    void Update()
    {
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
        // if (isOpened)
        if (panelState.Equals(PanelState.Shown))
            return;

        base.ShowPanel();

        GameManager.Instance.GameState = GameState.Paused;

        pauseMenuBackground.SetActive(true);
        pauseMenuPanel.SetActive(true);

        pauseMenuBackground.GetComponent<Image>().DOFade(0.5f, 0.25f).SetEase(Ease.OutQuart);
        sideMenuPanel.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 0), 0.25f).SetEase(Ease.OutQuart);
        settingsMenuPanel.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 0), 0.25f).SetEase(Ease.OutQuart);

        await Task.Delay(250);

        Time.timeScale = 0;
        pauseMenuPanel.SetActive(true);

        // isOpened = true;
        panelState = PanelState.Shown;
    }

    public async Task HidePauseMenu()
    {
        Time.timeScale = 1;

        pauseMenuBackground.GetComponent<Image>().DOFade(0, 0.25f).SetEase(Ease.OutQuart);
        sideMenuPanel.GetComponent<RectTransform>().DOAnchorPos(new Vector2(-sideMenuPanel.GetComponent<RectTransform>().sizeDelta.x, 0), 0.25f).SetEase(Ease.OutQuart);
        settingsMenuPanel.GetComponent<RectTransform>().DOAnchorPos(new Vector2(settingsMenuPanel.GetComponent<RectTransform>().sizeDelta.x, 0), 0.25f).SetEase(Ease.OutQuart);

        await Task.Delay(250);

        pauseMenuBackground.SetActive(false);
        pauseMenuPanel.SetActive(false);

        settingsMenu.SaveSettings();
        GameManager.Instance.GameState = GameState.Playing;

        // isOpened = false;
        panelState = PanelState.Hidden;
    }

    public override void HidePanel()
    {
        // if (!isOpened)
        if (panelState.Equals(PanelState.Hidden))
            return;

        base.HidePanel();
        _ = HidePauseMenu();
    }

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

    public void OnShowPause()
    {
        if (panelState != PanelState.Hidden ||
            GameManager.Instance.GameState != GameState.Playing)
            return;

        ShowPanel();
    }

    public void SaveGame()
    {
        if (panelState != PanelState.Shown)
            return;

        // await HidePauseMenu(true);
        HidePanel();
        settingsMenu.SaveSettings();
        SaveManager.Instance.SaveProfile();
    }

    public async void ExitToMenu()
    {
        if (panelState != PanelState.Shown)
            return;

        await HidePauseMenu();

        // save settings to device
        settingsMenu.SaveSettings();

        // save data to profile
        SaveManager.Instance.SaveProfile(false);

        // exit to menu
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

    protected override GameObject GetPanelGobj()
        => pauseMenuPanel;
}
