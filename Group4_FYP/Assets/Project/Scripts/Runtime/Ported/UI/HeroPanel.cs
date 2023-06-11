using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using DG.Tweening;
using PathOfHero.PersistentData;

public class HeroPanel : PanelOverride
{
    [SerializeField]
    private HeroProfile m_HeroProfile;

    [SerializeField]
    private GameObject heroPanel;

    [SerializeField]
    private Text heroNameText;

    [SerializeField]
    private Text heroLevelText;

    [SerializeField]
    private Text coinText;

    [SerializeField]
    private Text keyHintText;

    [SerializeField]
    private InventorySlot weaponSlot;

    [SerializeField]
    private InventorySlot armorSlot;

    [SerializeField]
    private ArmorItemData tempArmor; // temp

    [SerializeField]
    private InventorySlot badgeSlot;

    [SerializeField]
    private ItemData badge;

    [SerializeField]
    private ItemData placeholderBadge;

    [SerializeField]
    private InputReader m_InputReader;
    [SerializeField]
    private InputActionReference m_ShowInventoryAction;

    private RectTransform heroPanelRectTransform;

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
    {
        m_InputReader.ShowInventory += ToggleInventory;
        m_InputReader.HideInventory += ToggleInventory;
        m_HeroProfile.OnProfileLoaded += OnProfileLoaded;
    }

    private void OnDisable()
    {
        m_InputReader.ShowInventory -= ToggleInventory;
        m_InputReader.HideInventory -= ToggleInventory;
        m_HeroProfile.OnProfileLoaded -= OnProfileLoaded;
    }

    // Start is called before the first frame update
    void Start()
    {
        // base.Start();

        // isOpened = false;

        heroPanel.GetComponent<CanvasGroup>().alpha = 0;
        heroPanelRectTransform = heroPanel.GetComponent<RectTransform>();
        heroPanelRectTransform.anchoredPosition = new Vector2(0, -heroPanelRectTransform.rect.height / 4);
        heroPanel.SetActive(false);

        keyHintText.text = m_ShowInventoryAction.action.GetBindingDisplayString();

        // temp: will implement the whole thing in the future
        armorSlot.Configure(tempArmor, 1, InventoryMode.Preview);
        badgeSlot.Configure(placeholderBadge, 1, InventoryMode.Preview);
    }

    private void ToggleInventory()
    {
        if (panelState.Equals(PanelState.Hidden))
            ShowPanel();
        else if (panelState.Equals(PanelState.Shown))
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

    public void SetupBadgeSlot(bool badgeObtained)
    {
        badgeSlot.Clear();
        badgeSlot.Configure(badgeObtained ? badge : placeholderBadge, 1, InventoryMode.Preview);
    }

    private void OnProfileLoaded()
    {
        heroNameText.text = m_HeroProfile.DisplayName;
    }
}
