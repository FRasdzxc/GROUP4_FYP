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
    public int level;
    public int storedExp;
    public int storedCoin;
    public int map;
    public List<Inventory.InventoryEntry> inventory;

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
        level = 1;
        storedExp = 0;
        storedCoin = 0;
        map = 0;
        inventory = new List<Inventory.InventoryEntry>();
    }

    public ProfileData(string profileName, HeroClass heroClass, HeroData defaultStats) // used for creating a new profile
    {
        this.profileName = profileName;
        this.heroClass = heroClass.ToString();

        health = defaultStats.health;
        maxHealth = defaultStats.health;
        healthRegeneration = defaultStats.healthRegeneration;
        mana = defaultStats.mana;
        maxMana = defaultStats.mana;
        manaRegeneration = defaultStats.manaRegeneration;
        level = 1;
        storedExp = 0;
        storedCoin = 0;
        map = 0;
        inventory = new List<Inventory.InventoryEntry>();
    }

    public ProfileData(string newProfileName, ProfileData profileData) // used for moving data when updating profiles
    {
        profileName = newProfileName;
        heroClass = profileData.heroClass;
        health = profileData.health;
        maxHealth = profileData.maxHealth;
        healthRegeneration = profileData.healthRegeneration;
        mana = profileData.mana;
        maxMana = profileData.maxMana;
        manaRegeneration = profileData.manaRegeneration;
        level = profileData.level;
        storedExp = profileData.storedExp;
        storedCoin = profileData.storedCoin;
        map = profileData.map;
        inventory = profileData.inventory;
    }
}
