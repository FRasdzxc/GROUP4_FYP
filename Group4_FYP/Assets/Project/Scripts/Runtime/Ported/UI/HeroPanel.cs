using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using DG.Tweening;

public class HeroPanel : PanelOverride
{
    [SerializeField] private GameObject heroPanel;
    [SerializeField] private Text heroNameText;
    [SerializeField] private Text heroLevelText;
    [SerializeField] private Text coinText;
    [SerializeField] private Text keyHintText;
    [SerializeField] private InventorySlot weaponSlot;
    [SerializeField] private InventorySlot armorSlot;
    [SerializeField] private ArmorItemData tempArmor; // temp
    
    private RectTransform heroPanelRectTransform;
    private InputAction showInventoryAction;
    private InputAction hideInventoryAction;

    private static HeroPanel instance;
    public static HeroPanel Instance => instance;

    // protected override void Awake()
    protected override void Awake()
    {
        base.Awake();

        if (!instance)
            instance = this;
    }

    private void OnEnable()
        => GameManager.onPlayerSetUp += SetUp;

    private void OnDisable()
        => GameManager.onPlayerSetUp -= SetUp;

    // Start is called before the first frame update
    void Start()
    {
        // base.Start();

        // isOpened = false;

        heroPanel.GetComponent<CanvasGroup>().alpha = 0;
        heroPanelRectTransform = heroPanel.GetComponent<RectTransform>();
        heroPanelRectTransform.anchoredPosition = new Vector2(0, -heroPanelRectTransform.rect.height / 4);
        heroPanel.SetActive(false);

        heroNameText.text = SaveSystem.Instance.ProfileName;

        // temp: will implement the whole thing in the future
        armorSlot.Configure(tempArmor, 1, InventoryMode.Preview);
    }

    // Update is called once per frame
    void Update()
    {
        if (panelState.Equals(PanelState.Hidden) && showInventoryAction.triggered)
            ShowPanel();
        else if (panelState.Equals(PanelState.Shown) && hideInventoryAction.triggered)
            HidePanel();
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
        if (!CanShow())
            return;

        base.ShowPanel();

        Inventory.Instance.RefreshInventoryPanel();
        Orb.Instance.RefreshUpgradeItemContainer();

        heroPanel.SetActive(true);
        heroPanelRectTransform.DOAnchorPosY(0, 0.25f).SetEase(Ease.OutQuart);
        await heroPanel.GetComponent<CanvasGroup>().DOFade(1, 0.25f).SetEase(Ease.OutQuart).AsyncWaitForCompletion();

        heroPanel.GetComponent<CanvasGroup>().alpha = 1;
        // isOpened = true;
        panelState = PanelState.Shown;
    }

    public async override void HidePanel()
    {
        base.HidePanel();

        heroPanelRectTransform.DOAnchorPosY(-heroPanelRectTransform.rect.height / 4, 0.25f).SetEase(Ease.OutQuart);
        await heroPanel.GetComponent<CanvasGroup>().DOFade(0, 0.25f).SetEase(Ease.OutQuart).AsyncWaitForCompletion();
        heroPanel.SetActive(false);

        Tooltip.Instance.HideTooltip(); // workaround; to be fixed

        // isOpened = false;
        panelState = PanelState.Hidden;
    }

    public async void OnHidePanel()
    {
        heroPanelRectTransform.DOAnchorPosY(-heroPanelRectTransform.rect.height / 4, 0.25f).SetEase(Ease.OutQuart);
        await heroPanel.GetComponent<CanvasGroup>().DOFade(0, 0.25f).SetEase(Ease.OutQuart).AsyncWaitForCompletion();
        heroPanel.SetActive(false);

        // isOpened = false;
        panelState = PanelState.Hidden;

        Tooltip.Instance.HideTooltip(); // workaround; to be fixed
    }

    protected override GameObject GetPanelGobj()
        => heroPanel;

    public void SetupWeaponSlot(ItemData weaponItem)
    {
        weaponSlot.Clear();
        weaponSlot.Configure(weaponItem, 1, InventoryMode.Preview);
    }

    protected override void SetUp()
    {
        base.SetUp();

        showInventoryAction = playerInput.actions["ShowInventory"];
        hideInventoryAction = playerInput.actions["HideInventory"];
        showInventoryAction.Enable();
        hideInventoryAction.Enable();
        keyHintText.text = showInventoryAction.GetBindingDisplayString();
    }
}
