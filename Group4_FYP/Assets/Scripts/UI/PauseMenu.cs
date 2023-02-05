using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PauseMenu : MonoBehaviour
{
    private bool isOpened;
    [SerializeField] private GameObject hudPanel;
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private GameObject sideMenuPanel;
    [SerializeField] private GameObject settingsMenuPanel;
    [SerializeField] private GameObject pauseMenuBackground;
    [SerializeField] private SceneController sceneController;
    [SerializeField] private SettingsMenu settingsMenu;

    // Start is called before the first frame update
    void Start()
    {
        isOpened = false;

        // code for showing hudpanel and hiding osdpanel
        //hudPanel.SetActive(true);
        pauseMenuBackground.SetActive(false);
        pauseMenuPanel.SetActive(false);

        pauseMenuBackground.GetComponent<Image>().color = new Color32(0, 0, 0, 0);
        sideMenuPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(-sideMenuPanel.GetComponent<RectTransform>().sizeDelta.x, 0);
        settingsMenuPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(settingsMenuPanel.GetComponent<RectTransform>().sizeDelta.x, 0);
    }

    // Update is called once per frame
    async void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isOpened)
            {
                await HidePauseMenu(true);
            }
            else
            {
                await ShowPauseMenu();
            }
        }

        if (isOpened && Input.GetKeyDown(KeyCode.BackQuote)) // not finished: also check if saving is allowed atm
        {
            SaveGame();
        }
    }

    public async Task ShowPauseMenu()
    {
        GameController.Instance.SetGameState(GameState.Paused);

        pauseMenuBackground.SetActive(true);
        pauseMenuPanel.SetActive(true);
        hudPanel.GetComponent<CanvasGroup>().DOFade(0, 0.25f);

        pauseMenuBackground.GetComponent<Image>().DOFade(0.5f, 0.25f).SetEase(Ease.OutQuart);
        sideMenuPanel.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 0), 0.25f).SetEase(Ease.OutQuart);
        settingsMenuPanel.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 0), 0.25f).SetEase(Ease.OutQuart);

        await Task.Delay(250);

        Time.timeScale = 0;
        pauseMenuPanel.SetActive(true);
        hudPanel.SetActive(false);

        isOpened = true;
    }

    public async Task HidePauseMenu(bool bShowHudPanel)
    {
        Time.timeScale = 1;

        if (bShowHudPanel)
        {
            hudPanel.SetActive(true);
            hudPanel.GetComponent<CanvasGroup>().DOFade(1, 0.25f);
        }

        pauseMenuBackground.GetComponent<Image>().DOFade(0, 0.25f).SetEase(Ease.OutQuart);
        sideMenuPanel.GetComponent<RectTransform>().DOAnchorPos(new Vector2(-sideMenuPanel.GetComponent<RectTransform>().sizeDelta.x, 0), 0.25f).SetEase(Ease.OutQuart);
        settingsMenuPanel.GetComponent<RectTransform>().DOAnchorPos(new Vector2(settingsMenuPanel.GetComponent<RectTransform>().sizeDelta.x, 0), 0.25f).SetEase(Ease.OutQuart);

        await Task.Delay(250);

        pauseMenuBackground.SetActive(false);
        pauseMenuPanel.SetActive(false);
        if (bShowHudPanel)
        {
            hudPanel.SetActive(true);
            hudPanel.GetComponent<CanvasGroup>().alpha = 1;
        }

        settingsMenu.SaveSettings();
        GameController.Instance.SetGameState(GameState.Playing);

        isOpened = false;
    }

    public void ResumeGame()
    {
        _ = HidePauseMenu(true);
    }

    public async void SaveGame()
    {
        await HidePauseMenu(true);
        settingsMenu.SaveSettings();
        SaveSystem.Instance.SaveData(true);
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
        sceneController.ChangeScene("StartScene");
    }
}
