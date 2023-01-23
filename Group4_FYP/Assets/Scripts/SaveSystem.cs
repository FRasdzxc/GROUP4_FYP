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
        abilityManager.SetMana(profile.mana);
        abilityManager.SetMaxMana(profile.maxMana);
        abilityManager.SetManaRegeneration(profile.manaRegeneration);
        gameController.SetMap(profile.map);
    }

    public void SaveData(bool showNotification)
    {
        profile.health = hero.GetHealth();
        profile.maxHealth = hero.GetMaxHealth();
        profile.healthRegeneration = hero.GetHealthRegeneration();
        profile.level = hero.GetLevel();
        profile.storedExp = hero.GetStoredExp();
        profile.mana = abilityManager.GetMana();
        profile.maxMana = abilityManager.GetMaxMana();
        profile.manaRegeneration = abilityManager.GetManaRegeneration();
        profile.map = gameController.GetMap();

        ProfileManagerJson.SaveProfile(profile);

        if (showNotification)
        {
            _ = Notification.Instance.ShowNotification("Sucessfully saved data to Profile \"" + profile.profileName + "\"!");
        }
    }

    // not yet implemented
    private void AutoSave()
    {
        
    }
}
