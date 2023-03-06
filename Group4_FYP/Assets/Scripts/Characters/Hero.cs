using UnityEngine;
// using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Hero : MonoBehaviour
{
    [SerializeField] private HeroData heroData;

    [SerializeField] private GameObject weaponHolder;

    [SerializeField] private MovementControllerV2 movementController;
    [SerializeField] private AbilityManager abilityManager;
    //[SerializeField] private MaskingCanvas maskingCanvas;

    private HUD hud;
    private SpriteRenderer sr;
    private bool isDead;
    // private ColorGrading colorGrading;
    private ColorAdjustments colorAdjustments;
    private GameObject spawnPoint;
    private MaskingCanvas maskingCanvas;
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

        maskingCanvas = GameObject.FindGameObjectWithTag("MaskingCanvas").GetComponent<MaskingCanvas>();
        hud = GameObject.FindGameObjectWithTag("Canvas").GetComponent<HUD>();
        spawnPoint = GameObject.FindGameObjectWithTag("Respawn");
    }

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        // Camera.main.GetComponent<PostProcessVolume>().profile.TryGetSettings(out colorGrading);
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<Volume>().profile.TryGet(out colorAdjustments);
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

                if (level % 5 == 0)
                {
                    Orb.Instance.AddOrbs(1);
                }
            }
            hud.UpdateXP(level, storedExp, requiredExp);

            if (GameManager.Instance.IsPlayingHostile())
            {
                // testonly
                if (Input.GetKeyDown(KeyCode.Backspace))
                {
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        Die();
                    }
                    else
                    {
                        TakeDamage(15, false);
                    }
                }
                if (Input.GetKeyDown(KeyCode.Equals))
                {
                    AddHealth(25f);
                }
            }
        }
    }

    public void Setup() // useful for respawn
    {
        isDead = false;
        // hud.SetupHealth(health, upgradedMaxHealth);
        hud.UpdateHealth(health, upgradedMaxHealth);
        spawnPoint = GameObject.FindGameObjectWithTag("Respawn");
        transform.position = spawnPoint.transform.position;
        // colorGrading.saturation.value = 0f;
        colorAdjustments.saturation.value = 0f;
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
            {
                Die();
            }
        }
    }

    private async void Die()
    {
        isDead = true;
        health = 0;
        // hud.UpdateHealth(health);
        hud.UpdateHealth(health, upgradedMaxHealth);
        // colorGrading.saturation.value = -100f;
        colorAdjustments.saturation.value = -100f;
        movementController.ResetAnimatorParameters();
        movementController.enabled = false;
        abilityManager.enabled = false;
        weaponHolder.SetActive(false);

        await hud.ShowHugeMessage("You Died", Color.red);
        Respawn();
    }

    public async void Respawn()
    {
        await maskingCanvas.ShowMaskingCanvas(true);
        health = upgradedMaxHealth;
        Setup();
        await maskingCanvas.ShowMaskingCanvas(false);
    }

    public void Spawn()
    {
        spawnPoint = GameObject.FindGameObjectWithTag("Respawn");
        transform.position = spawnPoint.transform.position;
        Debug.Log(spawnPoint);
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
