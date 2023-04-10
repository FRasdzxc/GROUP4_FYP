using UnityEngine;
using UnityEngine.InputSystem;
using PathOfHero.Controllers;
using System.Collections;
using PathOfHero.UI;

public class Hero : MonoBehaviour
{
    [SerializeField] private HeroData heroData;

    [SerializeField] private GameObject weaponHolder;

    [SerializeField] private MovementControllerV2 movementController;
    [SerializeField] private AbilityManager abilityManager;
    [SerializeField] private int orbObtainLevel = 5;

    private HUD hud;
    private SpriteRenderer sr;
    private bool isDead;
    private GameObject spawnPoint;
    private string profileName;
    private ProfileData profile;

    // mana
    private float mana;

    // xp
    int level;
    int requiredExp;
    int storedExp;
    float expGainMultiplierUpgrade;
    int storedCoin = 0;

    private float health;
    private float maxHealth;
    private float _maxHealthUpgrade;
    public float MaxHealthUpgrade
    {
        get => _maxHealthUpgrade;
        private set
        {
            _maxHealthUpgrade = value;
            upgradedMaxHealth = maxHealth + _maxHealthUpgrade;
        }
    }
    private float upgradedMaxHealth;
    private float healthRegeneration;
    private float healthRegenerationUpgrade;
    private float defense;
    private float _defenseUpgrade;
    public float DefenseUpgrade
    {
        get => _defenseUpgrade;
        private set
        {
            _defenseUpgrade = value;
            upgradedDefense = defense + _defenseUpgrade;
        }
    }
    private float upgradedDefense;

    private PlayerInput playerInput;
    private InputAction takeDamageAction;

    private static Hero instance;
    public static Hero Instance
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

        hud = GameObject.FindGameObjectWithTag("Canvas").GetComponent<HUD>();
        spawnPoint = GameObject.FindGameObjectWithTag("Respawn");

        playerInput = GetComponent<PlayerInput>();
        takeDamageAction = playerInput.actions["TakeDamage"];
        takeDamageAction.Enable();
    }

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        movementController.SetMovementSpeed(heroData.walkspeed);

        Setup();
    }

    // Update is called once per frame
    void Update()
    {
        // test respawn
        if (!isDead)
        {
            // health
            if (health < upgradedMaxHealth)
            {
                health += Time.deltaTime * (healthRegeneration + healthRegenerationUpgrade);
            }
            health = Mathf.Clamp(health, 0, upgradedMaxHealth);
            // hud.UpdateHealth(health);
            hud.UpdateHealth(health, upgradedMaxHealth);

            // xp
            requiredExp = (int)(level * 100 * 1.25);
            if(storedExp >= requiredExp)
            {
                storedExp -= requiredExp;
                //hud.SetupXP(level, requiredExp);
                level++;
                HeroPanel.Instance.UpdateLevel(level);
                _ = Notification.Instance.ShowNotification("Level Up! - " + level.ToString("n0"));

                if (level % orbObtainLevel == 0)
                {
                    Orb.Instance.AddOrbs(1);
                }
            }
            hud.UpdateXP(level, storedExp, requiredExp);

            if (GameManager.Instance.IsPlayingHostile())
            {
                // testonly
                // if (Input.GetKeyDown(KeyCode.Backspace))
                if (takeDamageAction.triggered)
                {
                    if (Input.GetKey(KeyCode.LeftShift))
                        TakeDamage(health, false);
                    else
                        TakeDamage(15, false);
                }
                if (Input.GetKeyDown(KeyCode.Equals))
                    AddHealth(25f);
            }
        }
    }

    public void Setup() // useful for respawn
    {
        isDead = false;
        // hud.SetupHealth(health, upgradedMaxHealth);
        hud.UpdateHealth(health, upgradedMaxHealth);
        PostProcessController.Instance?.ChangeVolume(PostProcessController.ProfileType.Default, false);
        sr.color = Color.white;
        movementController.enabled = true;
        abilityManager.enabled = true;
        abilityManager.Setup();
        weaponHolder.SetActive(true);

        // xp
        requiredExp = (int)(level * 100 * 1.25);
        //hud.SetupXP(level, requiredExp);
        hud.UpdateXP(level, storedExp, requiredExp);
        HeroPanel.Instance.UpdateLevel(level);
        HeroPanel.Instance.UpdateCoin(storedCoin);
    }

    private void TakeDamage(float damage, bool accountForDefenseUpgrade = true)
    {
        if (!isDead && GameManager.Instance.IsPlayingHostile())
        {
            if (accountForDefenseUpgrade)
            {
                health -= damage / upgradedDefense;
            }
            else
            {
                health -= damage;
            }

            if (health <= 0)
                StartCoroutine(Die());
        }
    }

    private IEnumerator Die()
    {
        isDead = true;
        health = 0;
        // hud.UpdateHealth(health);
        hud.UpdateHealth(health, upgradedMaxHealth);
        PostProcessController.Instance?.ChangeVolume(PostProcessController.ProfileType.Death);
        movementController.ResetAnimatorParameters();
        movementController.enabled = false;
        abilityManager.enabled = false;
        weaponHolder.SetActive(false);

        yield return StartCoroutine(hud.ShowHugeMessage("You Died", Color.red));

        var loadingScreen = LoadingScreen.Instance;
        if (loadingScreen != null)
            yield return StartCoroutine(loadingScreen.FadeIn());

        health = upgradedMaxHealth;
        Setup();
        Spawn();

        if (GameManager.Instance.GetCurrentMapType() == MapType.Dungeon)
        {
            GameManager.Instance.LoadMap("map_town");   // cannot respawn in dungeon so player will be teleported back to town
            SaveSystem.Instance.LoadData();             // revert all stats earned in dungeon
        }

        yield return loadingScreen.FadeOut();
    }

    public void Spawn()
    {
        spawnPoint = GameObject.FindGameObjectWithTag("Respawn");
        if (spawnPoint == null)
        {
            Debug.LogWarning("[Hero] Spawn point not found.");
            return;
        }

        transform.position = spawnPoint.transform.position;
    }

    #region Setters
    public void SetHealth(float health)
    {
        this.health = health;
    }

    public void SetMaxHealth(float maxHealth)
    {
        this.maxHealth = maxHealth;
    }

    public void SetMaxHealthUpgrade(float value)
    {
        this.MaxHealthUpgrade = value;
    }

    public void SetHealthRegeneration(float healthRegeneration)
    {
        this.healthRegeneration = healthRegeneration;
    }

    public void SetHealthRegenerationUpgrade(float value)
    {
        this.healthRegenerationUpgrade = value;
    }

    public void SetDefense(float value)
    {
        if (value < 1f) // preventive
        {
            defense = 1f;
        }
        else
        {
            defense = value;
        }
    }

    public void SetDefenseUpgrade(float value)
    {
        DefenseUpgrade = value;
    }

    public void SetLevel(int level)
    {
        this.level = level;
    }

    public void SetStoredExp(int storedExp)
    {
        this.storedExp = storedExp;
    }

    public void SetExpGainMultiplierUpgrade(float value)
    {
        this.expGainMultiplierUpgrade = value;
    }

    public void SetStoredCoin(int coin)
    {
        this.storedCoin = coin;
    }
    #endregion

    #region Getters
    public float GetHealth()
    {
        return health;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public float GetMaxHealthUpgrade()
    {
        return MaxHealthUpgrade;
    }

    public float GetHealthRegeneration()
    {
        return healthRegeneration;
    }

    public float GetHealthRegenerationUpgrade()
    {
        return healthRegenerationUpgrade;
    }

    public float GetDefense()
    {
        return defense;
    }

    public float GetDefenseUpgrade()
    {
        return DefenseUpgrade;
    }

    public int GetLevel()
    {
        return level;
    }
    
    public int GetStoredExp()
    {
        return storedExp;
    }

    public float GetExpGainMultiplierUpgrade()
    {
        return expGainMultiplierUpgrade;
    }

    public int GetStoredCoin()
    {
        return storedCoin;
    }
    #endregion

    #region AddDeduct
    public void AddHealth(float value)
    {
        health += value;
    }

    public void AddMaxHealthUpgrade(float value)
    {
        MaxHealthUpgrade += value;
        // hud.SetupHealth(health, upgradedMaxHealth);
    }

    public void AddHealthRegenerationUpgrade(float value)
    {
        healthRegenerationUpgrade += value;
    }

    public void AddDefenseUpgrade(float value)
    {
        DefenseUpgrade += value;
    }

    public void AddEXP(int exp, bool accountForExpGainMultiplier = true)
    {
        if (accountForExpGainMultiplier)
        {
            storedExp += (int)(exp * expGainMultiplierUpgrade);
        }
        else
        {
            storedExp += exp;
        }
        //hud.UpdateXP(level, storedExp);
    }

    public void AddExpGainMultiplierUpgrade(float value)
    {
        expGainMultiplierUpgrade += value;
    }

    public void AddCoin(int coin)
    {
        storedCoin += coin;
        HeroPanel.Instance.UpdateCoin(storedCoin);
    }

    // public void DeductEXP(int exp)
    // {
    //     storedExp -= exp;
    //     hud.UpdateXP(level, storedExp);
    // }

    // public void DeductCoin(int coin)
    // {
    //     storedCoin -= coin;
    // }
    #endregion

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("MobWeaponTrigger") && collision.GetComponent<WeaponTrigger>())
        {
            TakeDamage(collision.GetComponent<WeaponTrigger>().GetDamage(false));
            collision.GetComponent<WeaponTrigger>().push(gameObject);
            sr.color = Color.red;
        }

        if (collision.CompareTag("Coin")) // ?
        {
            AddCoin(15);
        }
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        sr.color = Color.white;
    }
}
