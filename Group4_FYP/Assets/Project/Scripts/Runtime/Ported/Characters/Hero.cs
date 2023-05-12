using UnityEngine;
using UnityEngine.InputSystem;
using PathOfHero.Controllers;
using System.Collections;
using PathOfHero.Telemetry;
using PathOfHero.UI;
using PathOfHero.Utilities;

public class Hero : Singleton<Hero>
{
    [SerializeField] private HeroData heroData;

    [SerializeField] private GameObject weaponHolder;

    [SerializeField] private MovementControllerV2 movementController;
    [SerializeField] private AbilityManager abilityManager;
    [SerializeField] private int orbObtainLevel = 2;
    [SerializeField] private AudioClip[] damageSoundClips;
    [SerializeField] private AudioClip[] dieSoundClips;
    [SerializeField] private bool canRegenerateHealth = true;

    private HUD hud;
    private SpriteRenderer sr;
    public bool IsDead { get; private set; }
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

    private AudioSource audioSource;

    protected override void Awake()
    {
        base.Awake();

        hud = GameObject.FindGameObjectWithTag("Canvas").GetComponent<HUD>();
        spawnPoint = GameObject.FindGameObjectWithTag("Respawn");
        audioSource = GetComponent<AudioSource>();

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
        if (!IsDead)
        {
            if (canRegenerateHealth)
            {
                // health
                if (health < upgradedMaxHealth)
                {
                    health += Time.deltaTime * (healthRegeneration + healthRegenerationUpgrade);
                }
            }
            health = Mathf.Clamp(health, 0, upgradedMaxHealth);
            hud.UpdateHealth(health, upgradedMaxHealth);

            // xp
            requiredExp = (int)(level * 100 * 1.25);
            if(storedExp >= requiredExp)
            {
                storedExp -= requiredExp;
                level++;
                
                _ = Notification.Instance.ShowNotification("Level Up! - " + level.ToString("n0"));

                if (level % orbObtainLevel == 0)
                {
                    Orb.Instance.AddOrbs(1);
                }
            }
            hud.UpdateXP(level, storedExp, requiredExp);
            // DataCollector.Instance?.HeroLevel(level);
            HeroPanel.Instance.UpdateLevel(level);
            HeroPanel.Instance.UpdateCoin(storedCoin);

#if UNITY_EDITOR
            if (GameManager.Instance.IsPlayingHostile())
            {
                // testonly
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
#endif
        }
    }

    public void Setup() // useful for respawn
    {
        IsDead = false;
        hud.UpdateHealth(health, upgradedMaxHealth);
        PostProcessController.Instance?.ChangeVolume(PostProcessController.ProfileType.Default, false);
        sr.color = Color.white;
        movementController.enabled = true;
        abilityManager.enabled = true;
        abilityManager.Setup();
        weaponHolder.SetActive(true);

        // xp
        requiredExp = (int)(level * 100 * 1.25);
        hud.UpdateXP(level, storedExp, requiredExp);
        HeroPanel.Instance.UpdateLevel(level);
        HeroPanel.Instance.UpdateCoin(storedCoin);
    }

    public void TakeDamage(float damage, bool accountForDefenseUpgrade = true)
    {
        if (!IsDead && GameManager.Instance.IsPlayingHostile())
        {
            var amount = accountForDefenseUpgrade ? damage / upgradedDefense : damage;
            // DataCollector.Instance?.DamageTaken(amount);
            health = Mathf.Clamp(health - damage, 0, upgradedMaxHealth);

            if (damageSoundClips.Length > 0)
                PlaySound(damageSoundClips[Random.Range(0, damageSoundClips.Length)]);

            if (health <= 0)
                StartCoroutine(Die());
        }
    }

    public IEnumerator Die()
    {
        IsDead = true;
        health = 0;
        hud.UpdateHealth(health, upgradedMaxHealth);
        PostProcessController.Instance?.ChangeVolume(PostProcessController.ProfileType.Death);
        movementController.ResetAnimatorParameters();
        movementController.enabled = false;
        abilityManager.enabled = false;
        weaponHolder.SetActive(false);

        if (dieSoundClips.Length > 0)
            PlaySound(dieSoundClips[Random.Range(0, dieSoundClips.Length)]);

        // DataCollector.Instance?.PlayerDied();
        yield return StartCoroutine(hud.ShowHugeMessage("You Died", Color.red));

        if (!GameManager.Instance.GetMapType().Equals(MapType.Dungeon))
        {
            var loadingScreen = LoadingScreen.Instance;
            if (loadingScreen != null)
                yield return StartCoroutine(loadingScreen.FadeIn());

            health = upgradedMaxHealth;
            Setup();
            Spawn();

            yield return loadingScreen.FadeOut();
        }
        
        GameManager.Instance.GiveUp(); // return to town and lose all progress
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

    private void PlaySound(AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        audioSource.Play();
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

    public float GetUpgradedMaxHealth()
    {
        return upgradedMaxHealth;
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
        var amount = accountForExpGainMultiplier ? (int)(exp * expGainMultiplierUpgrade) : exp;
        storedExp += amount;
        // DataCollector.Instance?.ExpGained(amount);
    }

    public void AddExpGainMultiplierUpgrade(float value)
    {
        expGainMultiplierUpgrade += value;
    }

    public void AddCoin(int coin)
    {
        storedCoin += coin;

        //if (coin > 0)
        //    // DataCollector.Instance?.CoinsEarned(coin);
        //else if (coin < 0)
        //    // DataCollector.Instance?.CoinsSpent(coin * -1);
    }
    #endregion

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("MobWeaponTrigger") && collision.GetComponent<WeaponTrigger>())
        {
            TakeDamage(collision.GetComponent<WeaponTrigger>().GetDamage(false));
            collision.GetComponent<WeaponTrigger>().push(gameObject);
            sr.color = Color.red;
        }
        else if (collision.CompareTag("MobWeaponTriggerDeadly") && collision.GetComponent<WeaponTrigger>())
            StartCoroutine(Die());

        if (collision.CompareTag("Coin")) // better way?
            AddCoin(15);
    }
    
    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("MobWeaponTriggerDeadly") && collision.collider.GetComponent<WeaponTrigger>())
            StartCoroutine(Die());
    }

    protected void OnTriggerExit2D(Collider2D collision)
        => sr.color = Color.white;
}
