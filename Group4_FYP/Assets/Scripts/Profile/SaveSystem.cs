using System;
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
    private GameManager gameController;
    private Inventory inventory;
    private Orb orb;
    private WeaponManager weaponManager;

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
        gameController = GetComponent<GameManager>();
        inventory = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Inventory>();
        orb = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Orb>();
        weaponManager = GameObject.FindGameObjectWithTag("Player").GetComponent<WeaponManager>();
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
        hero.SetDefense(profile.defense);
        hero.SetLevel(profile.level);
        hero.SetStoredExp(profile.storedExp);
        hero.SetStoredCoin(profile.storedCoin);
        abilityManager.SetMana(profile.mana);
        abilityManager.SetMaxMana(profile.maxMana);
        abilityManager.SetManaRegeneration(profile.manaRegeneration);
        // gameController.SetMap(profile.map);
        gameController.SetMapId(profile.mapId);
        inventory.SetItems(profile.inventory);
        orb.SetOrbs(profile.orbs);
        orb.SetUsedOrbs(profile.usedOrbs);
        hero.SetMaxHealthUpgrade(profile.maxHealthUpgrade);
        hero.SetHealthRegenerationUpgrade(profile.healthRegenerationUpgrade);
        hero.SetDefenseUpgrade(profile.defenseUpgrade);
        abilityManager.SetMaxManaUpgrade(profile.maxManaUpgrade);
        abilityManager.SetManaRegenerationUpgrade(profile.manaRegenerationUpgrade);
        hero.SetExpGainMultiplierUpgrade(profile.expGainMultiplierUpgrade);
        weaponManager.SetWeaponTier((HeroClass)Enum.Parse(typeof(HeroClass), profile.heroClass), profile.weaponId, profile.weaponTier);
    }

    public void SaveData(bool showNotification)
    {
        profile.health = hero.GetHealth();
        profile.maxHealth = hero.GetMaxHealth();
        profile.healthRegeneration = hero.GetHealthRegeneration();
        profile.defense = hero.GetDefense();
        profile.level = hero.GetLevel();
        profile.storedExp = hero.GetStoredExp();
        profile.storedCoin = hero.GetStoredCoin();
        profile.mana = abilityManager.GetMana();
        profile.maxMana = abilityManager.GetMaxMana();
        profile.manaRegeneration = abilityManager.GetManaRegeneration();
        // profile.map = gameController.GetMap();
        profile.mapId = gameController.GetMapId();
        profile.inventory = inventory.GetItems();
        profile.orbs = orb.GetOrbs();
        profile.usedOrbs = orb.GetUsedOrbs();
        profile.maxHealthUpgrade = hero.GetMaxHealthUpgrade();
        profile.healthRegenerationUpgrade = hero.GetHealthRegenerationUpgrade();
        profile.defenseUpgrade = hero.GetDefenseUpgrade();
        profile.maxManaUpgrade = abilityManager.GetMaxManaUpgrade();
        profile.manaRegenerationUpgrade = abilityManager.GetManaRegenerationUpgrade();
        profile.expGainMultiplierUpgrade = hero.GetExpGainMultiplierUpgrade();
        profile.weaponId = weaponManager.GetWeaponId();
        profile.weaponTier = weaponManager.GetWeaponTier();

        ProfileManagerJson.SaveProfile(profile);

        if (showNotification)
        {
            _ = Notification.Instance.ShowNotification("Successfully saved data to Profile \"" + profile.profileName + "\"!");
        }
    }

    // not yet implemented
    private void AutoSave()
    {
        // check if saving is allowed first, then save, restart the timer
    }
}
