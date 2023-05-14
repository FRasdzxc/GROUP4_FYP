using System;
using UnityEngine;
using PathOfHero.Utilities;
using PathOfHero.Others;

// get every ProfileData attributes then use ProjectManagerJson to save
public class SaveSystem : Singleton<SaveSystem>
{
    [SerializeField] private bool isTestScene; // useful for testing
    [Tooltip("Unit: minutes")] [SerializeField] private float autosaveDuration;

    //[SerializeField] private Hero hero;
    //[SerializeField] private AbilityManager abilityManager;
    // private Hero hero;
    // private AbilityManager abilityManager;
    private GameManager gameController;
    private Inventory inventory;
    private Orb orb;
    // private WeaponManager weaponManager;

    private ProfileData profile;
    private float nextSaveTime;

    public string ProfileName => profile.profileName;

    protected override void Awake()
    {
        base.Awake();
        profile = isTestScene ? new() : ProfileManagerJson.GetProfile(PlayerPrefs.GetString("selectedProfileName"));

        // hero = GameObject.FindGameObjectWithTag("Player").GetComponent<Hero>();
        // abilityManager = GameObject.FindGameObjectWithTag("Player").GetComponent<AbilityManager>();
        gameController = GetComponent<GameManager>();
        inventory = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Inventory>();
        orb = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Orb>();
        // weaponManager = GameObject.FindGameObjectWithTag("Player").GetComponent<WeaponManager>();
        // LoadData();

        autosaveDuration *= 60f;
    }

    private void Start()
    {
        nextSaveTime = Time.unscaledTime + autosaveDuration;
    }

    private void Update()
    {
        if (Time.unscaledTime >= nextSaveTime && GameManager.Instance.MapType != MapType.Dungeon)
        {
            SaveData();
            nextSaveTime = Time.unscaledTime + autosaveDuration;
        }
    }

    public void LoadData()
    {
        Hero.Instance.SetHealth(profile.health);
        Hero.Instance.SetMaxHealth(profile.maxHealth);
        Hero.Instance.SetHealthRegeneration(profile.healthRegeneration);
        Hero.Instance.SetDefense(profile.defense);
        Hero.Instance.SetLevel(profile.level);
        Hero.Instance.SetStoredExp(profile.storedExp);
        Hero.Instance.SetStoredCoin(profile.storedCoin);
        AbilityManager.Instance.SetMana(profile.mana);
        AbilityManager.Instance.SetMaxMana(profile.maxMana);
        AbilityManager.Instance.SetManaRegeneration(profile.manaRegeneration);
        // gameController.SetMap(profile.map);
        gameController.MapId = profile.mapId;
        inventory.SetItems(profile.inventory);
        orb.SetOrbs(profile.orbs);
        orb.SetUsedOrbs(profile.usedOrbs);
        Hero.Instance.SetMaxHealthUpgrade(profile.maxHealthUpgrade);
        Hero.Instance.SetHealthRegenerationUpgrade(profile.healthRegenerationUpgrade);
        Hero.Instance.SetDefenseUpgrade(profile.defenseUpgrade);
        AbilityManager.Instance.SetMaxManaUpgrade(profile.maxManaUpgrade);
        AbilityManager.Instance.SetManaRegenerationUpgrade(profile.manaRegenerationUpgrade);
        Hero.Instance.SetExpGainMultiplierUpgrade(profile.expGainMultiplierUpgrade);
        WeaponManager.Instance.SetWeaponTier((HeroClass)Enum.Parse(typeof(HeroClass), profile.heroClass), profile.weaponId, profile.weaponTier);
        AbilityManager.Instance.SetAbilityDamageUpgrade(profile.abilityDamageUpgrade);
    }

    public void SaveData(bool showNotification = true, bool accountForMapType = true)
    {
        if (accountForMapType)
        {
            if (GameManager.Instance.MapType == MapType.Dungeon)
            {
                _ = Notification.Instance.ShowNotification("You cannot save game while engaged in a dungeon battle!");
                return;
            }
        }

        profile.health = Hero.Instance.GetHealth();
        profile.maxHealth = Hero.Instance.GetMaxHealth();
        profile.healthRegeneration = Hero.Instance.GetHealthRegeneration();
        profile.defense = Hero.Instance.GetDefense();
        profile.level = Hero.Instance.GetLevel();
        profile.storedExp = Hero.Instance.GetStoredExp();
        profile.storedCoin = Hero.Instance.GetStoredCoin();
        profile.mana = AbilityManager.Instance.GetMana();
        profile.maxMana = AbilityManager.Instance.GetMaxMana();
        profile.manaRegeneration = AbilityManager.Instance.GetManaRegeneration();
        // profile.map = gameController.GetMap();
        profile.mapId = gameController.MapId;
        profile.inventory = inventory.GetItems();
        profile.orbs = orb.GetOrbs();
        profile.usedOrbs = orb.GetUsedOrbs();
        profile.maxHealthUpgrade = Hero.Instance.GetMaxHealthUpgrade();
        profile.healthRegenerationUpgrade = Hero.Instance.GetHealthRegenerationUpgrade();
        profile.defenseUpgrade = Hero.Instance.GetDefenseUpgrade();
        profile.maxManaUpgrade = AbilityManager.Instance.GetMaxManaUpgrade();
        profile.manaRegenerationUpgrade = AbilityManager.Instance.GetManaRegenerationUpgrade();
        profile.expGainMultiplierUpgrade = Hero.Instance.GetExpGainMultiplierUpgrade();
        profile.weaponId = WeaponManager.Instance.GetWeaponId();
        profile.weaponTier = WeaponManager.Instance.GetWeaponTier();
        profile.abilityDamageUpgrade = AbilityManager.Instance.GetAbilityDamageUpgrade();

        ProfileManagerJson.SaveProfile(profile);

        if (showNotification)
            _ = Notification.Instance.ShowNotification($"Successfully saved data to Profile '<color={CustomColorStrings.green}>{profile.profileName}</color>'!");
    }

    void OnEnable()
        => GameManager.onPlayerSetUp += LoadData;

    void OnDisable()
        => GameManager.onPlayerSetUp -= LoadData;
}
