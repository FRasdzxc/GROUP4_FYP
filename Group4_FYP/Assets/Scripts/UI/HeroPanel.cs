using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using DG.Tweening;

public class HeroPanel : PanelOverride/*, IPanelConflictable*/
{
    // [SerializeField] private GameObject hudMainPanel;
    [SerializeField] private GameObject heroPanel;
    [SerializeField] private Text heroNameText;
    [SerializeField] private Text heroLevelText;
    [SerializeField] private Text coinText;
    [SerializeField] private Text keyHintText;
    [SerializeField] private InventorySlot weaponSlot;
    [SerializeField] private InventorySlot armorSlot;
    [SerializeField] private ArmorItemData tempArmor; // temp
    // [SerializeField] private bool panelOverridable;

    private bool isOpened;
    private RectTransform heroPanelRectTransform;
    private InputAction showInventoryAction;
    private InputAction hideInventoryAction;

    private static HeroPanel instance;
    public static HeroPanel Instance
    {
        get
        {
            return instance;
        }
    }

    // protected override void Awake()
    protected override void Awake()
    {
        base.Awake();

        if (!instance)
        {
            instance = this;
        }
    }

    private void OnEnable()
    {
        // OnHide += OnHidePanel;
    }

    private void OnDisable()
    {
        // OnHide -= OnHidePanel;
    }

    // Start is called before the first frame update
    // protected override void Start()
    void Start()
    {
        // base.Start();

        isOpened = false;

        heroPanel.GetComponent<CanvasGroup>().alpha = 0;
        heroPanelRectTransform = heroPanel.GetComponent<RectTransform>();
        heroPanelRectTransform.anchoredPosition = new Vector2(0, -heroPanelRectTransform.rect.height / 4);
        heroPanel.SetActive(false);

        heroNameText.text = ProfileManagerJson.GetProfile(PlayerPrefs.GetString("selectedProfileName")).profileName;

        showInventoryAction = playerInput.actions["ShowInventory"];
        hideInventoryAction = playerInput.actions["HideInventory"];
        showInventoryAction.Enable();
        hideInventoryAction.Enable();
        keyHintText.text = showInventoryAction.GetBindingDisplayString();
        Debug.Log("show inventory action binding " + showInventoryAction.GetBindingDisplayString());

        // temp: will implement the whole thing in the future
        armorSlot.Configure(tempArmor, 1, InventoryMode.Preview);
    }

    // Update is called once per frame
    // protected override void Update()
    void Update()
    {
        // base.Update();

        ////if (Input.GetKeyDown(KeyCode.Q))
        //if (showInventoryAction.triggered)
        //// if (Input.GetKeyDown(KeyCode.Q) && InputManager.Instance.GetKeyDown(KeyCode.Q))
        //// if (Input.GetKeyDown(KeyCode.Q) && InputManager.Instance.GetKeyDown(KeyCode.Q))
        //{
        //    if (isOpened)
        //    {
        //        // HideHeroPanel();
        //        HidePanel();
        //    }
        //    else
        //    {
        //        ShowPanel();
        //    }

        //    // isOpened = !isOpened;
        //}

        if (showInventoryAction.triggered)
        {
            ShowPanel();
        }
        else if (hideInventoryAction.triggered)
        {
            HidePanel();
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

    public async override void ShowPanel()
    {
        base.ShowPanel();

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

    public async override void HidePanel()
    {
        base.HidePanel();

        HUD.Instance.ShowHUDMain();
        heroPanelRectTransform.DOAnchorPosY(-heroPanelRectTransform.rect.height / 4, 0.25f).SetEase(Ease.OutQuart);
        await heroPanel.GetComponent<CanvasGroup>().DOFade(0, 0.25f).SetEase(Ease.OutQuart).AsyncWaitForCompletion();
        heroPanel.SetActive(false);

        isOpened = false;

        Tooltip.Instance.HideTooltip(); // workaround; to be fixed
    }

    public async void OnHidePanel()
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
