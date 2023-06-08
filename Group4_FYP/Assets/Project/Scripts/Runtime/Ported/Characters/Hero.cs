using System.Collections;
using UnityEngine;
using PathOfHero.Controllers;
using PathOfHero.Managers.Data;
using PathOfHero.UI;
using PathOfHero.Utilities;
using PathOfHero.Others;
using PathOfHero.PersistentData;

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

    [SerializeField]
    private ScoreEventChannel m_ScoreEventChannel;

    private HUD hud;
    private SpriteRenderer sr;
    public bool IsDead { get; private set; }
    private GameObject spawnPoint;

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

    private AudioSource audioSource;

    private bool badgeObtained;

    public static event System.Action onHeroDeath;


    protected override void Awake()
    {
        base.Awake();

        hud = GameObject.FindGameObjectWithTag("Canvas").GetComponent<HUD>();
        spawnPoint = GameObject.FindGameObjectWithTag("Respawn");
        audioSource = GetComponent<AudioSource>();
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
                    health += Time.deltaTime * (healthRegeneration + healthRegenerationUpgrade);
            }
            health = Mathf.Clamp(health, 0, upgradedMaxHealth);
            hud.UpdateHealth(health, upgradedMaxHealth);

            // xp
            requiredExp = (int)(level * 100 * 1.25);
            if(storedExp >= requiredExp)
            {
                storedExp -= requiredExp;
                level++;
                
                _ = Notification.Instance.ShowNotificationAsync($"Level Up! - <color={CustomColorStrings.green}>{level.ToString("n0")}</color>");

                if (level % orbObtainLevel == 0)
                    Orb.Instance.AddOrbs(1);
            }
            hud.UpdateXP(level, storedExp, requiredExp);
            HeroPanel.Instance.UpdateLevel(level);
            HeroPanel.Instance.UpdateCoin(storedCoin);

#if UNITY_EDITOR
            if (GameManager.Instance.IsPlayingHostile())
            {
                // testonly
                if (Input.GetKey(KeyCode.Backspace))
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
            m_ScoreEventChannel.DamageTaken(amount);
            health = Mathf.Clamp(health - damage, 0, upgradedMaxHealth);

            if (damageSoundClips.Length > 0)
                PlaySound(damageSoundClips[UnityEngine.Random.Range(0, damageSoundClips.Length)]);

            if (health <= 0)
                StartCoroutine(Die());
        }
    }

    public IEnumerator Die()
    {
        onHeroDeath?.Invoke();

        IsDead = true;
        health = 0;
        hud.UpdateHealth(health, upgradedMaxHealth);
        PostProcessController.Instance?.ChangeVolume(PostProcessController.ProfileType.Death);
        movementController.ResetAnimatorParameters();
        movementController.enabled = false;
        abilityManager.enabled = false;
        weaponHolder.SetActive(false);

        if (dieSoundClips.Length > 0)
            PlaySound(dieSoundClips[UnityEngine.Random.Range(0, dieSoundClips.Length)]);

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
        => audioSource.PlayOneShot(audioClip);
    #region Setters
    public void SetHealth(float health)
        => this.health = health;

    public void SetMaxHealth(float maxHealth)
        => this.maxHealth = maxHealth;

    public void SetMaxHealthUpgrade(float value)
        => this.MaxHealthUpgrade = value;

    public void SetHealthRegeneration(float healthRegeneration)
        => this.healthRegeneration = healthRegeneration;

    public void SetHealthRegenerationUpgrade(float value)
        => this.healthRegenerationUpgrade = value;

    public void SetDefense(float value)
    {
        if (value < 1f) // preventive
            defense = 1f;
        else
            defense = value;
    }

    public void SetDefenseUpgrade(float value)
        => DefenseUpgrade = value;

    public void SetLevel(int level)
        => this.level = level;

    public void SetStoredExp(int storedExp)
        => this.storedExp = storedExp;

    public void SetExpGainMultiplierUpgrade(float value)
        => this.expGainMultiplierUpgrade = value;

    public void SetStoredCoin(int coin)
        => this.storedCoin = coin;

    public void SetBadgeObtained(bool value)
    {
        badgeObtained = value;
        HeroPanel.Instance.SetupBadgeSlot(value);
    }
    #endregion

    #region Getters
    public float GetHealth()
        => health;

    public float GetMaxHealth()
        => maxHealth;

    public float GetUpgradedMaxHealth()
        => upgradedMaxHealth;

    public float GetMaxHealthUpgrade()
        => MaxHealthUpgrade;

    public float GetHealthRegeneration()
        => healthRegeneration;

    public float GetHealthRegenerationUpgrade()
        => healthRegenerationUpgrade;

    public float GetDefense()
        => defense;

    public float GetDefenseUpgrade()
        => DefenseUpgrade;

    public int GetLevel()
        => level;
    
    public int GetStoredExp()
        => storedExp;

    public float GetExpGainMultiplierUpgrade()
        => expGainMultiplierUpgrade;

    public int GetStoredCoin()
        => storedCoin;

    public bool GetBadgeObtained()
        => badgeObtained;
    #endregion

    #region AddDeduct
    public void AddHealth(float value)
        => health += value;

    public void AddMaxHealthUpgrade(float value)
    {
        MaxHealthUpgrade += value;
        // hud.SetupHealth(health, upgradedMaxHealth);
    }

    public void AddHealthRegenerationUpgrade(float value)
        => healthRegenerationUpgrade += value;

    public void AddDefenseUpgrade(float value)
        => DefenseUpgrade += value;

    public void AddEXP(int exp, bool accountForExpGainMultiplier = true)
    {
        var amount = accountForExpGainMultiplier ? (int)(exp * expGainMultiplierUpgrade) : exp;
        storedExp += amount;
    }

    public void AddExpGainMultiplierUpgrade(float value)
        => expGainMultiplierUpgrade += value;

    public void AddCoin(int coin)
        => storedCoin += coin;
    #endregion

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.CompareTag("MobWeaponTrigger") || collision.CompareTag("MobWeaponTriggerStronger")) && collision.GetComponent<WeaponTrigger>())
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
