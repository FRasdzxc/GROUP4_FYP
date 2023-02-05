using UnityEngine;

// get every ProfileData attributes then use ProjectManagerJson to save
public class SaveSystem : MonoBehaviour
{
    [SerializeField] private bool isTestScene; // useful for testing
    [SerializeField] private float autosaveDuration;

    //[SerializeField] private Hero hero;
    //[SerializeField] private AbilityManager abilityManager;
    private Hero hero;
    private AbilityManager abilityManager;
    private GameController gameController;
    private Inventory inventory;
    private Orb orb;

    private ProfileData profile;

    private static SaveSystem instance;
    public static SaveSystem Instance
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

        if (!isTestScene)
        {
            profile = ProfileManagerJson.GetProfile(PlayerPrefs.GetString("selectedProfileName"));
        }
        else
        {
            profile = new ProfileData();
        }

        hero = GameObject.FindGameObjectWithTag("Player").GetComponent<Hero>();
        abilityManager = GameObject.FindGameObjectWithTag("Player").GetComponent<AbilityManager>();
        gameController = GetComponent<GameController>();
        inventory = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Inventory>();
        orb = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Orb>();
        LoadData();
    }

    // Update is called once per frame
    void Update()
    {
        // autosave operation?
    }

    public void LoadData()
    {
        hero.SetHealth(profile.health);
        hero.SetMaxHealth(profile.maxHealth);
        hero.SetHealthRegeneration(profile.healthRegeneration);
        hero.SetLevel(profile.level);
        hero.SetStoredExp(profile.storedExp);
        hero.SetStoredCoin(profile.storedCoin);
        abilityManager.SetMana(profile.mana);
        abilityManager.SetMaxMana(profile.maxMana);
        abilityManager.SetManaRegeneration(profile.manaRegeneration);
        gameController.SetMap(profile.map);
        inventory.SetItems(profile.inventory);
        orb.SetOrbs(profile.orbs);
        orb.SetUsedOrbs(profile.usedOrbs);
        hero.SetMaxHealthUpgrade(profile.maxHealthUpgrade);
        hero.SetHealthRegenerationUpgrade(profile.healthRegenerationUpgrade);
        abilityManager.SetMaxManaUpgrade(profile.maxManaUpgrade);
        abilityManager.SetManaRegenerationUpgrade(profile.manaRegenerationUpgrade);
        hero.SetExpGainMultiplierUpgrade(profile.expGainMultiplierUpgrade);
    }

    public void SaveData(bool showNotification)
    {
        profile.health = hero.GetHealth();
        profile.maxHealth = hero.GetMaxHealth();
        profile.healthRegeneration = hero.GetHealthRegeneration();
        profile.level = hero.GetLevel();
        profile.storedExp = hero.GetStoredExp();
        profile.storedCoin = hero.GetStoredCoin();
        profile.mana = abilityManager.GetMana();
        profile.maxMana = abilityManager.GetMaxMana();
        profile.manaRegeneration = abilityManager.GetManaRegeneration();
        profile.map = gameController.GetMap();
        profile.inventory = inventory.GetItems();
        profile.orbs = orb.GetOrbs();
        profile.usedOrbs = orb.GetUsedOrbs();
        profile.maxHealthUpgrade = hero.GetMaxHealthUpgrade();
        profile.healthRegenerationUpgrade = hero.GetHealthRegenerationUpgrade();
        profile.maxManaUpgrade = abilityManager.GetMaxManaUpgrade();
        profile.manaRegenerationUpgrade = abilityManager.GetManaRegenerationUpgrade();
        profile.expGainMultiplierUpgrade = hero.GetExpGainMultiplierUpgrade();

        ProfileManagerJson.SaveProfile(profile);

        if (showNotification)
        {
            _ = Notification.Instance.ShowNotification("Sucessfully saved data to Profile \"" + profile.profileName + "\"!");
        }
    }

    // not yet implemented
    private void AutoSave()
    {
        // check if saving is allowed first, then save, restart the timer
    }
}
