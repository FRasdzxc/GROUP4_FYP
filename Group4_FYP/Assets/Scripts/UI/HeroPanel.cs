using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HeroPanel : PanelOverride/*, IPanelConflictable*/
{
    // [SerializeField] private GameObject hudMainPanel;
    [SerializeField] private GameObject heroPanel;
    [SerializeField] private Text heroNameText;
    [SerializeField] private Text heroLevelText;
    [SerializeField] private Text coinText;
    [SerializeField] private InventorySlot weaponSlot;
    // [SerializeField] private bool panelOverridable;

    private bool isOpened;
    private RectTransform heroPanelRectTransform;

    private static HeroPanel instance;
    public static HeroPanel Instance
    {
        get
        {
            return instance;
        }
    }

    void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        isOpened = false;

        heroPanel.GetComponent<CanvasGroup>().alpha = 0;
        heroPanelRectTransform = heroPanel.GetComponent<RectTransform>();
        heroPanelRectTransform.anchoredPosition = new Vector2(0, -heroPanelRectTransform.rect.height / 4);
        heroPanel.SetActive(false);

        heroNameText.text = ProfileManagerJson.GetProfile(PlayerPrefs.GetString("selectedProfileName")).profileName;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        // if (Input.GetKeyDown(KeyCode.Q) && InputManager.Instance.GetKeyDown(KeyCode.Q))
        // if (Input.GetKeyDown(KeyCode.Q) && InputManager.Instance.GetKeyDown(KeyCode.Q))
        {
            if (isOpened)
            {
                // HideHeroPanel();
                HidePanel();
            }
            else
            {
                ShowHeroPanel();
            }

            // isOpened = !isOpened;
        }
    }

    public void UpdateCoin(int coin)
    {
        coinText.text = coin.ToString("n0");
    }

    public void UpdateLevel(int level)
    {
        heroLevelText.text = "Level " + level.ToString("n0");
    }

    public async void ShowHeroPanel()
    {
        // if (!HideConflictingPanels())
        // {
        //     return;
        // }
        if (!OverridePanel())
        {
            return;
        }

        Inventory.Instance.RefreshInventoryPanel();

        HUD.Instance.HideHUDMain();
        heroPanel.SetActive(true);
        heroPanelRectTransform.DOAnchorPosY(0, 0.25f).SetEase(Ease.OutQuart);
        await heroPanel.GetComponent<CanvasGroup>().DOFade(1, 0.25f).SetEase(Ease.OutQuart).AsyncWaitForCompletion();

        heroPanel.GetComponent<CanvasGroup>().alpha = 1;
        isOpened = true;
    }

    public async override void HidePanel() //public async void HideHeroPanel()
    {
        HUD.Instance.ShowHUDMain();
        heroPanelRectTransform.DOAnchorPosY(-heroPanelRectTransform.rect.height / 4, 0.25f).SetEase(Ease.OutQuart);
        await heroPanel.GetComponent<CanvasGroup>().DOFade(0, 0.25f).SetEase(Ease.OutQuart).AsyncWaitForCompletion();
        heroPanel.SetActive(false);

        isOpened = false;

        Tooltip.Instance.HideTooltip(); // workaround; to be fixed
    }

    protected override GameObject GetPanel()
    {
        return heroPanel;
    }

    public void SetupWeaponSlot(ItemData weaponItem)
    {
        weaponSlot.Clear();
        weaponSlot.Configure(weaponItem, 1, InventoryMode.Preview);
    }

    // public bool HideConflictingPanels()
    // {
    //     if (!BuySellPanel.Instance.IsPanelActive())
    //     {
    //         return true;
    //     }

    //     if (BuySellPanel.Instance.IsPanelOverridable())
    //     {
    //         BuySellPanel.Instance.HideBuySellPanel();
    //         return true;
    //     }

    //     return false;
    // }

    // public bool IsPanelOverridable()
    // {
    //     return panelOverridable;
    // }

    // public bool IsPanelActive()
    // {
    //     return heroPanel.activeSelf;
    // }
}
