using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PauseMenu : MonoBehaviour
{
    private bool bIsOpened;
    [SerializeField] private GameObject hudPanel;
    [SerializeField] private GameObject osdPanel;
    [SerializeField] private GameObject sideMenuPanel;
    [SerializeField] private GameObject settingsMenuPanel;
    [SerializeField] private GameObject pauseMenuBackground;
    [SerializeField] private SceneController sceneController;

    // Start is called before the first frame update
    void Start()
    {
        bIsOpened = false;

        // code for showing hudpanel and hiding osdpanel
        hudPanel.SetActive(true);
        osdPanel.SetActive(false);

        pauseMenuBackground.GetComponent<Image>().color = new Color32(0, 0, 0, 0);
        sideMenuPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(-sideMenuPanel.GetComponent<RectTransform>().sizeDelta.x, 0);
        settingsMenuPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(settingsMenuPanel.GetComponent<RectTransform>().sizeDelta.x, 0);
    }

    // Update is called once per frame
    async void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (bIsOpened)
            {
                HidePauseMenu(true);
            }
            else
            {
                ShowPauseMenu();
            }
        }
    }

    public async void ShowPauseMenu()
    {
        osdPanel.SetActive(true);
        hudPanel.GetComponent<CanvasGroup>().DOFade(0, 0.25f);

        pauseMenuBackground.GetComponent<Image>().DOFade(0.5f, 0.25f).SetEase(Ease.OutQuart);
        sideMenuPanel.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 0), 0.25f).SetEase(Ease.OutQuart);
        settingsMenuPanel.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 0), 0.25f).SetEase(Ease.OutQuart);

        await Task.Delay(250);

        osdPanel.SetActive(true);
        hudPanel.SetActive(false);

        bIsOpened = true;
    }

    public async void HidePauseMenu(bool bShowHudPanel)
    {
        if (bShowHudPanel)
        {
            hudPanel.SetActive(true);
            hudPanel.GetComponent<CanvasGroup>().DOFade(1, 0.25f);
        }

        pauseMenuBackground.GetComponent<Image>().DOFade(0, 0.25f).SetEase(Ease.OutQuart);
        sideMenuPanel.GetComponent<RectTransform>().DOAnchorPos(new Vector2(-sideMenuPanel.GetComponent<RectTransform>().sizeDelta.x, 0), 0.25f).SetEase(Ease.OutQuart);
        settingsMenuPanel.GetComponent<RectTransform>().DOAnchorPos(new Vector2(settingsMenuPanel.GetComponent<RectTransform>().sizeDelta.x, 0), 0.25f).SetEase(Ease.OutQuart);

        await Task.Delay(250);

        osdPanel.SetActive(false);
        if (bShowHudPanel)
        {
            hudPanel.SetActive(true);
            hudPanel.GetComponent<CanvasGroup>().alpha = 1;
        }

        bIsOpened = false;
    }

    public async void ExitToMenu()
    {
        HidePauseMenu(false);
        await Task.Delay(250);

        // save data to profile

        // exit to menu
        sceneController.ChangeScene("StartScene");
    }
}
