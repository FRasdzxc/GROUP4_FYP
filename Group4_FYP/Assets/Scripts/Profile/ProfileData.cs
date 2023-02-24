using System;
using System.Collections.Generic;

[Serializable]
public class ProfileData
{
    public string profileName;
    public string heroClass;
    public float health;
    public float maxHealth;
    public float healthRegeneration;
    public float mana;
    public float maxMana;
    public float manaRegeneration;
    public float defense;
    public int level;
    public int storedExp;
    public int storedCoin;
    public int orbs;
    public int usedOrbs;
    public float maxHealthUpgrade;
    public float healthRegenerationUpgrade;
    public float defenseUpgrade;
    public float maxManaUpgrade;
    public float manaRegenerationUpgrade;
    public float expGainMultiplierUpgrade;
    public int map;
    public List<InventoryEntry> inventory;
    public int weaponTier;

    public ProfileData() // profile for test scenes
    {
        profileName = "_testprofile";
        heroClass = HeroClass.Mage.ToString();
        health = 200;
        maxHealth = 200;
        healthRegeneration = 5;
        mana = 200;
        maxMana = 200;
        manaRegeneration = 5;
        defense = 5;
        level = 1;
        storedExp = 0;
        storedCoin = 0;
        orbs = 0;
        usedOrbs = 0;
        maxHealthUpgrade = 0;
        healthRegenerationUpgrade = 0;
        defenseUpgrade = 0;
        maxManaUpgrade = 0;
        manaRegenerationUpgrade = 0;
        expGainMultiplierUpgrade = 1;
        map = 0;
        inventory = new List<InventoryEntry>();
        weaponTier = 0;
    }

    public ProfileData(string profileName, HeroClass heroClass, HeroData defaultStats) // used for creating a new profile
    {
        this.profileName = profileName;
        this.heroClass = heroClass.ToString();

        health = defaultStats.health;
        maxHealth = defaultStats.health;
        healthRegeneration = defaultStats.healthRegeneration;
        defense = defaultStats.defense;
        mana = defaultStats.mana;
        maxMana = defaultStats.mana;
        manaRegeneration = defaultStats.manaRegeneration;
        level = 1;
        storedExp = 0;
        storedCoin = 0;
        orbs = 0;
        usedOrbs = 0;
        maxHealthUpgrade = 0;
        healthRegenerationUpgrade = 0;
        defenseUpgrade = 0;
        maxManaUpgrade = 0;
        manaRegenerationUpgrade = 0;
        expGainMultiplierUpgrade = 1;
        map = 0;
        inventory = new List<InventoryEntry>();
        weaponTier = 0;
    }

    public ProfileData(string newProfileName, ProfileData profileData) // used for moving data when updating profiles
    {
        profileName = newProfileName;
        heroClass = profileData.heroClass;
        health = profileData.health;
        maxHealth = profileData.maxHealth;
        healthRegeneration = profileData.healthRegeneration;
        defense = profileData.defense;
        mana = profileData.mana;
        maxMana = profileData.maxMana;
        manaRegeneration = profileData.manaRegeneration;
        level = profileData.level;
        storedExp = profileData.storedExp;
        storedCoin = profileData.storedCoin;
        orbs = profileData.orbs;
        usedOrbs = profileData.usedOrbs;
        maxHealthUpgrade = profileData.maxHealthUpgrade;
        healthRegenerationUpgrade = profileData.healthRegenerationUpgrade;
        defenseUpgrade = profileData.defenseUpgrade;
        maxManaUpgrade = profileData.maxManaUpgrade;
        manaRegenerationUpgrade = profileData.manaRegenerationUpgrade;
        expGainMultiplierUpgrade = profileData.expGainMultiplierUpgrade;
        map = profileData.map;
        inventory = profileData.inventory;
        weaponTier = profileData.weaponTier;
    }
}
