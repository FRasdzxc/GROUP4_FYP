using System;
using UnityEngine;

[Serializable]
public class ProfileData
{
    public string profileName;
    public string heroClass;
    public float health;
    public float maxHealth;
    public float mana;
    public float maxMana;
    public int level;
    public int storedExp;

    public ProfileData(string profileName, HeroClass heroClass, HeroData defaultStats) // used for creating a new profile
    {
        this.profileName = profileName;
        this.heroClass = heroClass.ToString();

        health = defaultStats.health;
        maxHealth = defaultStats.health;
        mana = defaultStats.mana;
        maxMana = defaultStats.mana;
        level = 1;
        storedExp = 0;
    }

    public ProfileData(string newProfileName, ProfileData profileData) // used for moving data when updating profiles
    {
        profileName = newProfileName;
        heroClass = profileData.heroClass;
        health = profileData.health;
        mana = profileData.mana;
        level = profileData.level;
        storedExp = profileData.storedExp;
    }
}
