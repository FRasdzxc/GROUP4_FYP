using System;
using UnityEngine;
using PathOfHero.Utilities;

// get every ProfileData attributes then use ProjectManagerJson to save
public class SaveSystem : Singleton<SaveSystem>
{
    [SerializeField] private bool isTestScene; // useful for testing
    [Tooltip("Unit: minutes")] [SerializeField] private float autosaveDuration;

    //[SerializeField] private Hero hero;
    //[SerializeField] private AbilityManager abilityManager;
    private Hero hero;
    private AbilityManager abilityManager;
    private GameManager gameController;
    private Inventory inventory;
    private Orb orb;
    private WeaponManager weaponManager;

    private ProfileData profile;
    private float nextSaveTime;

    public string ProfileName => profile.profileName;

    protected override void Awake()
    {
        base.Awake();
        profile = isTestScene ? new() : ProfileManagerJson.GetProfile(PlayerPrefs.GetString("selectedProfileName"));

        hero = GameObject.FindGameObjectWithTag("Player").GetComponent<Hero>();
        abilityManager = GameObject.FindGameObjectWithTag("Player").GetComponent<AbilityManager>();
        gameController = GetComponent<GameManager>();
        inventory = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Inventory>();
        orb = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Orb>();
        weaponManager = GameObject.FindGameObjectWithTag("Player").GetComponent<WeaponManager>();
        LoadData();

        autosaveDuration *= 60f;
    }

    private void Start()
    {
        nextSaveTime = Time.unscaledTime + autosaveDuration;
    }

    private void Update()
    {
        if (Time.unscaledTime >= nextSaveTime && (GameManager.Instance.MapType != MapType.Dungeon || GameManager.Instance.MapType != MapType.WaveDungeon))
        {
            SaveData();
            nextSaveTime = Time.unscaledTime + autosaveDuration;
        }
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
        gameController.MapId = profile.mapId;
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
        abilityManager.SetAbilityOutputUpgrade(profile.abilityOutputUpgrade);
    }

    public void SaveData(bool showNotification = true, bool accountForMapType = true)
    {
        if (accountForMapType)
        {
            if (GameManager.Instance.MapType == MapType.Dungeon || GameManager.Instance.MapType == MapType.WaveDungeon)
            {
                _ = Notification.Instance.ShowNotification("You cannot save game while engaged in a dungeon battle");
                return;
            }
        }

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
        profile.mapId = gameController.MapId;
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
        profile.abilityOutputUpgrade = abilityManager.GetAbilityOutputUpgrade();

        ProfileManagerJson.SaveProfile(profile);

        if (showNotification)
            _ = Notification.Instance.ShowNotification("Successfully saved data to Profile \"" + profile.profileName + "\"!");
    }
}
