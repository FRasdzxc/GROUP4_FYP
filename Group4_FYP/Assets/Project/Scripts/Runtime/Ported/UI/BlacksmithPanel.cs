using UnityEngine;
using DG.Tweening;

public class BlacksmithPanel : PanelOverride
{
    [SerializeField] private GameObject blacksmithPanel;
    private CanvasGroup blacksmithPanelCanvasGroup;
    private RectTransform blacksmithPanelRectTransform;

    private static BlacksmithPanel instance;
    public static BlacksmithPanel Instance => instance;

    protected override void Awake()
    {
        base.Awake();

        if (!instance)
            instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        blacksmithPanelCanvasGroup = blacksmithPanel.GetComponent<CanvasGroup>();
        blacksmithPanelRectTransform = blacksmithPanel.GetComponent<RectTransform>();

        blacksmithPanelCanvasGroup.alpha = 0;
        blacksmithPanelRectTransform.anchoredPosition = new Vector2(0, -blacksmithPanelRectTransform.rect.height / 4);
        blacksmithPanel.SetActive(false);
    }

#if UNITY_EDITOR
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (panelState.Equals(PanelState.Hidden))
                ShowBlacksmithPanel();
            else
                HidePanel();
        }
    }
#endif

    public void ShowBlacksmithPanel()
    {
        if (!CanShow())
            return;

        ShowPanel();
    }

    public async override void ShowPanel()
    {
        base.ShowPanel();

        //HUD.Instance.HideHUDMain();
        blacksmithPanel.SetActive(true);
        blacksmithPanelRectTransform.DOAnchorPosY(0, 0.25f).SetEase(Ease.OutQuart);
        await blacksmithPanelCanvasGroup.DOFade(1, 0.25f).SetEase(Ease.OutQuart).AsyncWaitForCompletion();

        blacksmithPanelCanvasGroup.alpha = 1;
        // isOpened = true;
        panelState = PanelState.Shown;
    }

    public async override void HidePanel()
    {
        base.HidePanel();

        //HUD.Instance.ShowHUDMain();
        blacksmithPanelRectTransform.DOAnchorPosY(-blacksmithPanelRectTransform.rect.height / 4, 0.25f).SetEase(Ease.OutQuart);
        await blacksmithPanelCanvasGroup.DOFade(0, 0.25f).SetEase(Ease.OutQuart).AsyncWaitForCompletion();
        blacksmithPanel.SetActive(false);

        Tooltip.Instance.HideTooltip(); // workaround; to be fixed

        // isOpened = false;
        panelState = PanelState.Hidden;
    }

    protected override GameObject GetPanelGobj()
        => blacksmithPanel;
}
