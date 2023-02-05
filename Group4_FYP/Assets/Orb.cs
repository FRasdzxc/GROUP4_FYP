using UnityEngine;
using UnityEngine.UI;

public class Orb : MonoBehaviour
{
    [SerializeField] private int baseResetPrice = 100;
    [SerializeField] private Text orbText;
    [SerializeField] private OrbUpgradeList orbUpgradeList;
    [SerializeField] private Transform upgradeItemContainer;
    [SerializeField] private GameObject upgradeItemPrefab;

    private float requiredResetPrice;
 
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
    private Hero hero;

    private static Orb instance;
    public static Orb Instance
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

        hero = GameObject.FindGameObjectWithTag("Player").GetComponent<Hero>();
    }

    // Start is called before the first frame update
    void Start()
    {
        RefreshUpgradeItemContainer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RefreshUpgradeItemContainer()
    {
        foreach (var upgrade in orbUpgradeList.orbUpgrades)
        {
            GameObject clone = Instantiate(upgradeItemPrefab, upgradeItemContainer.transform);

            Transform upgradeValueText = RecursiveFindChild(clone.transform, "UpgradeValueText");
            Transform upgradeItemText = RecursiveFindChild(clone.transform, "UpgradeItemText");

            upgradeValueText.GetComponent<Text>().text = "+ " + upgrade.value.ToString();
            upgradeItemText.GetComponent<Text>().text = upgrade.orbUpgradeName;

            clone.GetComponent<Button>().onClick.AddListener(() => { Upgrade(upgrade.type, upgrade.value); });
        }
    }

    public void Upgrade(OrbUpgradeType orbUpgradeType, float value)
    {
        if (Orbs <= 0)
        {
            _ = Notification.Instance.ShowNotification("You do not have any Orbs");
            return;
        }

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
        }

        Orbs--;
        usedOrbs++;
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
            "This will reset all your upgrades to health or mana etc. Do you wish to continue?\nCost: " + requiredResetPrice + " Coins",
            () =>
            {
                if (hero.GetStoredCoin() >= requiredResetPrice)
                {
                    hero.AddCoin(-(int)requiredResetPrice);
                    Orbs += usedOrbs;
                    usedOrbs = 0;

                    // reset all upgrades
                    Hero.Instance.SetMaxHealthUpgrade(0);
                    Hero.Instance.SetHealthRegenerationUpgrade(0);
                    AbilityManager.Instance.SetMaxManaUpgrade(0);
                    AbilityManager.Instance.SetManaRegenerationUpgrade(0);
                    Hero.Instance.SetExpGainMultiplierUpgrade(1);

                    _ = Notification.Instance.ShowNotification("Reset Orb upgrades");
                }
                else
                {
                    _ = Notification.Instance.ShowNotification("Insufficient amount of Coins");
                }
            },
            true);
    }

    #region Setters/Getters/Add
    public void SetOrbs(int value)
    {
        this.Orbs = value;
    }

    public void SetUsedOrbs(int value)
    {
        this.usedOrbs = value;
    }

    public int GetOrbs()
    {
        return Orbs;
    }

    public int GetUsedOrbs()
    {
        return usedOrbs;
    }

    public void AddOrbs(int value)
    {
        Orbs += value;
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
}
