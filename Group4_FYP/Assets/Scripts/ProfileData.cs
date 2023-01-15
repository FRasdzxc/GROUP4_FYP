using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProfileData
{
    public string profileName;
    public string heroClass;
    public float health;
    public float mana;
    public int level;
    public int storedExp;

    public ProfileData(string profileName) // used for creating new profiles
    {
        this.profileName = profileName;
        
        // not finished ofc
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

    public ProfileData(Hero hero) // used for saving
    {
        profileName = hero.GetProfileName();
        health = hero.GetHealth();

        // not finished
    }
}
