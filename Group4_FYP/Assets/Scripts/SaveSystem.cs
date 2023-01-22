using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// get every ProfileData attributes then use ProjectManagerJson to save
public class SaveSystem : MonoBehaviour
{
    [SerializeField] private bool isTestScene; // useful for testing
    [SerializeField] private float autosaveDuration;
    [SerializeField] private HeroList heroList;

    [SerializeField] private Hero hero;
    [SerializeField] private AbilityManager abilityManager;
    // [SerializeField] private

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

        LoadData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadData()
    {
        hero.SetHealth(profile.health);
        hero.SetMaxHealth(profile.maxHealth);
        hero.SetHealthRegeneration(profile.healthRegeneration);
        hero.SetLevel(profile.level);
        hero.SetStoredExp(profile.storedExp);
        abilityManager.SetMana(profile.mana);
        abilityManager.SetMaxMana(profile.maxMana);
        abilityManager.SetManaRegeneration(profile.manaRegeneration);
    }

    public void SaveData()
    {
        profile.health = hero.GetHealth();
        profile.maxHealth = hero.GetMaxHealth();
        profile.healthRegeneration = hero.GetHealthRegeneration();
        profile.level = hero.GetLevel();
        profile.storedExp = hero.GetStoredExp();
        profile.mana = abilityManager.GetMana();
        profile.maxMana = abilityManager.GetMaxMana();
        profile.manaRegeneration = abilityManager.GetManaRegeneration();

        ProfileManagerJson.SaveProfile(profile);
    }

    // not yet implemented
    private void AutoSave()
    {
        
    }
}
