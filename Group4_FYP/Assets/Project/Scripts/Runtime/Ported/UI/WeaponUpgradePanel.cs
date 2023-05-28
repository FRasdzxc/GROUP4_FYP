using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using PathOfHero.Others;

public class WeaponUpgradePanel : PanelOverride
{
    [SerializeField] private GameWeapons gameWeapons;
    [SerializeField] private GameObject weaponUpgradePanel;
    [SerializeField] private InventorySlot weaponSlot;
    [SerializeField] private InventorySlot runicSlot;
    [SerializeField] private InventorySlot resultSlot;
    [SerializeField] private Image equalsImage;
    [SerializeField] private Sprite equalsSprite;
    [SerializeField] private Sprite notEqualsSprite;
    [SerializeField] private Text warningText;
    [SerializeField] private Text coinText;
    [SerializeField] private Text costText;
    [SerializeField] private AudioClip weaponUpgradeSound;
    private CanvasGroup weaponUpgradePanelCanvasGroup;
    private RectTransform weaponUpgradePanelRectTransform;

    private List<InventoryEntry> tempItems;
    private bool upgradable;
    private int price;
    private string weaponId;
    private int targetTier;

    private static WeaponUpgradePanel instance;
    public static WeaponUpgradePanel Instance
    {
        get => instance;
    }

    protected override void Awake()
    {
        base.Awake();

        if (!instance)
            instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        weaponUpgradePanelCanvasGroup = weaponUpgradePanel.GetComponent<CanvasGroup>();
        weaponUpgradePanelRectTransform = weaponUpgradePanel.GetComponent<RectTransform>();

        weaponUpgradePanelCanvasGroup.alpha = 0;
        weaponUpgradePanelRectTransform.anchoredPosition = new Vector2(0, -weaponUpgradePanelRectTransform.rect.height / 4);
        weaponUpgradePanel.SetActive(false);

        tempItems = new List<InventoryEntry>();
    }

#if UNITY_EDITOR
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (panelState.Equals(PanelState.Hidden))
                ShowWeaponUpgradePanel();
            else
                HidePanel();
        }
    }
#endif

    public void ShowWeaponUpgradePanel()
    {
        if (!CanShow())
            return;

        // prepare panel here
        ResetPanel();
        tempItems = new List<InventoryEntry>(Inventory.Instance.GetItems());
        coinText.text = Hero.Instance.GetStoredCoin().ToString("n0");
        upgradable = false;

        weaponId = WeaponManager.Instance.GetWeaponId();
        targetTier = WeaponManager.Instance.GetWeaponTier() + 1;

        weaponSlot.Configure(WeaponManager.Instance.GetWeapon().item, 1, InventoryMode.Preview);

        // check if weapon is of the highest tier
        foreach (ClassWeaponEntry cwe in gameWeapons.weaponList)
        {
            if (cwe.heroClass == WeaponManager.Instance.GetHeroClass())
            {
                foreach (WeaponEntry we in cwe.classWeapons)
                {
                    if (weaponId == we.weaponId)
                    {
                        // if (targetTier < cwe.classWeapons.Length)
                        if (targetTier < we.weaponTiers.Length)
                        {
                            // if weapon is not at the highest tier, find if tempItems contains runic for weapontier + 1
                            if (FindRunic())
                            {
                                warningText.gameObject.SetActive(false);
                                equalsImage.sprite = equalsSprite;
                                upgradable = true;
                            }
                            else
                            {
                                warningText.gameObject.SetActive(true);
                                warningText.text = String.Format("You do not have the required Runic - \"Runic (Tier {0})\" for this upgrade", targetTier);
                                equalsImage.sprite = notEqualsSprite;
                            }
                            resultSlot.Configure(we.weaponTiers[targetTier].item, 1, InventoryMode.Preview);
                        }
                        else
                        {
                            warningText.gameObject.SetActive(true);
                            warningText.text = "Your weapon is of the highest tier already!";
                            equalsImage.sprite = notEqualsSprite;
                            runicSlot.Clear();
                            resultSlot.Clear();
                        }
                    }
                }
            }
        }

        ShowPanel();
    }

    public async override void ShowPanel()
    {
        base.ShowPanel();

        //HUD.Instance.HideHUDMain();
        weaponUpgradePanel.SetActive(true);
        weaponUpgradePanelRectTransform.DOAnchorPosY(0, 0.25f).SetEase(Ease.OutQuart);
        await weaponUpgradePanelCanvasGroup.DOFade(1, 0.25f).SetEase(Ease.OutQuart).AsyncWaitForCompletion();

        weaponUpgradePanelCanvasGroup.alpha = 1;
        // isOpened = true;
        panelState = PanelState.Shown;
    }

    public async override void HidePanel()
    {
        base.HidePanel();

        //HUD.Instance.ShowHUDMain();
        weaponUpgradePanelRectTransform.DOAnchorPosY(-weaponUpgradePanelRectTransform.rect.height / 4, 0.25f).SetEase(Ease.OutQuart);
        await weaponUpgradePanelCanvasGroup.DOFade(0, 0.25f).SetEase(Ease.OutQuart).AsyncWaitForCompletion();
        weaponUpgradePanel.SetActive(false);

        Tooltip.Instance.HideTooltip(); // workaround; to be fixed

        // isOpened = false;
        panelState = PanelState.Hidden;
    }

    public void Upgrade()
    {
        // do checking
        if (upgradable)
        {
            ConfirmationPanel.Instance.ShowConfirmationPanel
            (
                "Upgrade Weapon",
                $"Do you want to upgrade this weapon to <color={CustomColorStrings.green}>Tier {targetTier}</color>?",
                () =>
                {
                    if (Hero.Instance.GetStoredCoin() >= price)
                    {
                        Hero.Instance.AddCoin(-price);
                        WeaponManager.Instance.UpgradeWeapon(targetTier);
                        Inventory.Instance.SetItems(tempItems);

                        HidePanel();
                        _ = Notification.Instance.ShowNotification("Successfully upgraded weapon!");
                        AudioManager.Instance.PlaySound(weaponUpgradeSound);
                    }
                    else
                        _ = Notification.Instance.ShowNotification("Insufficient amount of coins");
                },
                false,
                $"<color={CustomColorStrings.yellow}>Cost:</color> {price.ToString("n0")} coins, Runic (Tier {targetTier})"
            );
            // ConfirmationPanel.Instance.ShowConfirmationPanel("hi", "hi", () => {});
        }

        // do WeaponManager.Instance.UpgradeWeapon();
    }

    private void RemoveItem(ItemData item)
    {
        for (int i = tempItems.Count - 1; i >= 0; i--)
        {
            InventoryEntry entry = tempItems[i];
            if (entry.itemData != item)
                continue;

            int newQty = Mathf.Clamp(entry.qty - 1, 0, InventorySlot.kMaxStackSize);
            if (newQty == 0)
                tempItems.RemoveAt(i);
            else
                tempItems[i] = new InventoryEntry(item, newQty);
            
            break;
        }
    }

    private bool FindRunic()
    {
        foreach (InventoryEntry ie in tempItems)
        {
            if (ie.itemData is RunicItemData)
            {
                RunicItemData runicItem = ie.itemData as RunicItemData;
                if (runicItem.tier == targetTier)
                {
                    runicSlot.Configure(runicItem, 1, InventoryMode.Preview);
                    RemoveItem(runicItem);
                    price = runicItem.upgradePrice;
                    costText.text = price.ToString("n0");
                    return true;
                }
            }
        }

        return false;
    }

    private void ResetPanel()
    {
        weaponSlot.Clear();
        runicSlot.Clear();
        resultSlot.Clear();
        equalsImage.sprite = equalsSprite;
    }

    protected override GameObject GetPanelGobj()
        => weaponUpgradePanel;
}
