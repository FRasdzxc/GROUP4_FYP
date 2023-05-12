using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PathOfHero.Telemetry;
using PathOfHero.Utilities;

public class Orb : Singleton<Orb>
{
    [SerializeField] private int baseResetPrice = 150;
    [SerializeField] private Text orbText;
    [SerializeField] private OrbUpgradeList orbUpgradeList;
    [SerializeField] private Transform upgradeItemContainer;
    [SerializeField] private GameObject upgradeItemPrefab;
    [SerializeField] private Sprite orbSprite;

    private float requiredResetPrice;
    private List<GameObject> upgradeButtons;
 
    private int _orbs;
    public int Orbs
    {
        get => _orbs;
        private set
        {
            _orbs = value;
            orbText.text = _orbs.ToString("n0");
        }
    }

    private int usedOrbs;

    protected override void Awake()
    {
        base.Awake();
        upgradeButtons = new List<GameObject>();
    }

    public void RefreshUpgradeItemContainer()
    {
        // destroy all the upgrade buttons
        foreach (var b in upgradeButtons)
        {
            Destroy(b);
        }
        upgradeButtons.Clear();

        // add upgrade buttons
        foreach (var upgrade in orbUpgradeList.orbUpgrades)
        {
            GameObject clone = Instantiate(upgradeItemPrefab, upgradeItemContainer.transform);

            Transform upgradeValueText = RecursiveFindChild(clone.transform, "UpgradeValueText");
            Transform upgradeItemText = RecursiveFindChild(clone.transform, "UpgradeItemText");

            upgradeValueText.GetComponent<Text>().text = "+ " + upgrade.value.ToString();
            upgradeItemText.GetComponent<Text>().text = upgrade.orbUpgradeName;

            clone.GetComponent<Button>().onClick.AddListener(() => { Upgrade(upgrade.type, upgrade.value); });

            TooltipTrigger tooltipTrigger = clone.AddComponent<TooltipTrigger>();
            string[] attributes;
            string currentAttribute = "Current: ";
            string upgradedAttribute = "Upgraded: ";
            switch (upgrade.type)
            {
                case OrbUpgradeType.MaxHealth:
                    float currentMaxHealth = Hero.Instance.GetMaxHealth() + Hero.Instance.GetMaxHealthUpgrade();
                    currentAttribute += currentMaxHealth;
                    upgradedAttribute += currentMaxHealth + upgrade.value;
                    break;
                case OrbUpgradeType.HealthRegeneration:
                    float currentHealthRegeneration = Hero.Instance.GetHealthRegeneration() + Hero.Instance.GetHealthRegenerationUpgrade();
                    currentAttribute += currentHealthRegeneration;
                    upgradedAttribute += currentHealthRegeneration + upgrade.value;
                    break;
                case OrbUpgradeType.MaxMana:
                    float currentMaxMana = AbilityManager.Instance.GetMaxMana() + AbilityManager.Instance.GetMaxManaUpgrade();
                    currentAttribute += currentMaxMana;
                    upgradedAttribute += currentMaxMana + upgrade.value;
                    break;
                case OrbUpgradeType.ManaRegeneration:
                    float currentManaRegeneration = AbilityManager.Instance.GetManaRegeneration() + AbilityManager.Instance.GetManaRegenerationUpgrade();
                    currentAttribute += currentManaRegeneration;
                    upgradedAttribute += currentManaRegeneration + upgrade.value;
                    break;
                case OrbUpgradeType.ExpGainMultiplier:
                    float currentExpGainMultiplier = Hero.Instance.GetExpGainMultiplierUpgrade();
                    currentAttribute += currentExpGainMultiplier;
                    upgradedAttribute += currentExpGainMultiplier + upgrade.value;
                    break;
                case OrbUpgradeType.Defense:
                    float currentDefense = Hero.Instance.GetDefense() + Hero.Instance.GetDefenseUpgrade();
                    currentAttribute += currentDefense;
                    upgradedAttribute += currentDefense + upgrade.value;
                    break;
                case OrbUpgradeType.AbilityDamage:
                    float currentAbilityDamage = AbilityManager.Instance.GetAbilityDamageUpgrade();
                    currentAttribute += currentAbilityDamage;
                    upgradedAttribute += currentAbilityDamage + upgrade.value;
                    break;
            }
            attributes = new string[] { currentAttribute, upgradedAttribute };
            // tooltipTrigger.SetupTooltip(upgrade.orbUpgradeName, upgrade.description, attributes); // use Tooltip.Instance.ShowTooltip() instead?
            tooltipTrigger.SetupTooltip(upgrade.orbUpgradeName, "Orb Upgrade", upgrade.description, attributes, new TooltipHintType[] { TooltipHintType.Use });

            upgradeButtons.Add(clone);
        }
    }

    public void Upgrade(OrbUpgradeType orbUpgradeType, float value)
    {
        if (Orbs <= 0)
        {
            _ = Notification.Instance.ShowNotification("You do not have any Orbs");
            return;
        }

        //DataCollector.Instance?.OrbUpgradeUsed(orbUpgradeType.ToString());
        switch (orbUpgradeType)
        {
            case OrbUpgradeType.MaxHealth:
                Hero.Instance.AddMaxHealthUpgrade(value);
                break;
            case OrbUpgradeType.HealthRegeneration:
                Hero.Instance.AddHealthRegenerationUpgrade(value);
                break;
            case OrbUpgradeType.MaxMana:
                AbilityManager.Instance.AddMaxManaUpgrade(value);
                break;
            case OrbUpgradeType.ManaRegeneration:
                AbilityManager.Instance.AddManaRegenerationUpgrade(value);
                break;
            case OrbUpgradeType.ExpGainMultiplier:
                Hero.Instance.AddExpGainMultiplierUpgrade(value);
                break;
            case OrbUpgradeType.Defense:
                Hero.Instance.AddDefenseUpgrade(value);
                break;
            case OrbUpgradeType.AbilityDamage:
                AbilityManager.Instance.AddAbilityDamageUpgrade(value);
                break;
        }

        Orbs--;
        usedOrbs++;

        RefreshUpgradeItemContainer();
    }

    public void ResetOrbs()
    {
        if (usedOrbs <= 0)
        {
            _ = Notification.Instance.ShowNotification("You have not spent any Orbs yet");
            return;
        }

        requiredResetPrice = baseResetPrice * (1 + usedOrbs * 0.1f);

        ConfirmationPanel.Instance.ShowConfirmationPanel(
            "Reset Orbs",
            "This will reset all your upgrades to health or mana etc. Do you wish to continue?\n\nCost: " + requiredResetPrice.ToString("n0") + " Coins",
            () =>
            {
                if (Hero.Instance.GetStoredCoin() >= requiredResetPrice)
                {
                    Hero.Instance.AddCoin(-(int)requiredResetPrice);
                    Orbs += usedOrbs;
                    usedOrbs = 0;

                    // reset all upgrades
                    Hero.Instance.SetMaxHealthUpgrade(0);
                    Hero.Instance.SetHealthRegenerationUpgrade(0);
                    AbilityManager.Instance.SetMaxManaUpgrade(0);
                    AbilityManager.Instance.SetManaRegenerationUpgrade(0);
                    Hero.Instance.SetExpGainMultiplierUpgrade(1);
                    Hero.Instance.SetDefenseUpgrade(1);
                    AbilityManager.Instance.SetAbilityDamageUpgrade(1);

                    _ = Notification.Instance.ShowNotification("Reset Orb upgrades");
                }
                else
                {
                    _ = Notification.Instance.ShowNotification("Insufficient amount of Coins");
                }
            },
            true);

        RefreshUpgradeItemContainer();
    }

    #region Setters/Getters/Add
    public void SetOrbs(int value)
        => Orbs = value;

    public void SetUsedOrbs(int value)
        => usedOrbs = value;

    public int GetOrbs()
        => Orbs;

    public int GetUsedOrbs()
        => usedOrbs;

    public void AddOrbs(int value)
    {
        Orbs += value;
        Message.Instance.ShowMessage("+" + value + " Orb", orbSprite);
    }
    #endregion

    private Transform RecursiveFindChild(Transform parent, string childName)
    {
        foreach (Transform child in parent)
        {
            if (child.name == childName)
            {
                return child;
            }
            else
            {
                Transform child2 = RecursiveFindChild(child, childName);
                if (child2 != null)
                {
                    return child2;
                }
            }
        }

        return null;
    }

    void OnEnable()
        => GameManager.onPlayerSetUp += SetUp;

    void OnDisable()
        => GameManager.onPlayerSetUp -= SetUp;

    private void SetUp()
        => RefreshUpgradeItemContainer();
}
