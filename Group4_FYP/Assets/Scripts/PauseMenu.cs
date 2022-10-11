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

    // Start is called before the first frame update
    void Start()
    {
        bIsOpened = false;
    }

    // Update is called once per frame
    async void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (bIsOpened)
            {
                HidePauseMenu();
            }
            else
            {
                ShowPauseMenu();
            }
        }
    }

    public async void ShowPauseMenu()
    {
        hudPanel.GetComponent<Image>().DOColor(new Color32(255, 255, 255, 0), 0.25f);

        //osdPanel.SetActive(true);
        pauseMenuBackground.GetComponent<Image>().DOColor(new Color32(0, 0, 0, 128), 0.25f);
        sideMenuPanel.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 0), 0.25f).SetEase(Ease.OutQuart);

        bIsOpened = true;
    }

    public async void HidePauseMenu()
    {
        hudPanel.GetComponent<Image>().DOColor(new Color32(255, 255, 255, 0), 0.25f);

        pauseMenuBackground.GetComponent<Image>().DOColor(new Color32(0, 0, 0, 0), 0.25f);
        sideMenuPanel.GetComponent<RectTransform>().DOAnchorPos(new Vector2(-sideMenuPanel.GetComponent<RectTransform>().sizeDelta.x, 0), 0.25f).SetEase(Ease.OutQuart);
        settingsMenuPanel.GetComponent<RectTransform>().DOAnchorPos(new Vector2(settingsMenuPanel.GetComponent<RectTransform>().sizeDelta.x, 0), 0.25f).SetEase(Ease.OutQuart);

        bIsOpened = false;
    }

    public async void OpenSettingsMenuPanel()
    {
        settingsMenuPanel.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 0), 0.25f).SetEase(Ease.OutQuart);
    }
}
