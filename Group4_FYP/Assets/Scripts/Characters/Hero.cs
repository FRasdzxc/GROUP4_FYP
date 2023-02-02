using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using DG.Tweening;

public class Hero : MonoBehaviour
{
    [SerializeField] private HeroData heroData;

    [SerializeField] private GameObject weaponHolder;

    [SerializeField] private MovementControllerV2 movementController;
    [SerializeField] private AbilityManager abilityManager;
    //[SerializeField] private MaskingCanvas maskingCanvas;

    private HUD hud;
    private float healthRegeneration;
    private SpriteRenderer sr;
    private bool isDead;
    private ColorGrading colorGrading;
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
    int storedCoin = 0;

    private float _health;
    public float health
    {
        get
        {
            return _health;
        }
        private set
        {
            _health = value;
            // hud.UpdateHealth(value);
        }
    }
    private float maxHealth;

    void Awake()
    {
        maskingCanvas = GameObject.FindGameObjectWithTag("MaskingCanvas").GetComponent<MaskingCanvas>();
        hud = GameObject.FindGameObjectWithTag("Canvas").GetComponent<HUD>();
        spawnPoint = GameObject.FindGameObjectWithTag("Respawn");
    }

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        Camera.main.GetComponent<PostProcessVolume>().profile.TryGetSettings(out colorGrading);
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
            if (health < maxHealth)
            {
                health += Time.deltaTime * healthRegeneration;
            }
            health = Mathf.Clamp(health, 0, maxHealth);
            hud.UpdateHealth(health);

            // xp
            hud.UpdateXP(level, storedExp);
            requiredExp = (int)(level * 100 * 1.25);
            if(storedExp >= requiredExp)
            {
                storedExp -= requiredExp;
                hud.SetupXP(level, requiredExp);
                HeroPanel.Instance.UpdateLevel(level);
                level++;
                _ = Notification.Instance.ShowNotification("Level Up! - " + level);
            }

            if (GameController.Instance.GetGameState() == GameState.Playing)
            {
                // testonly
                if (Input.GetKeyDown(KeyCode.Backspace))
                {
                    TakeDamage(15);
                }
                if (Input.GetKeyDown(KeyCode.Equals))
                {
                    ChangeHealth(25f);
                }
            }
        }
    }

    public void Setup() // useful for respawn
    {
        isDead = false;
        hud.SetupHealth(health, maxHealth);
        transform.position = spawnPoint.transform.position;
        colorGrading.saturation.value = 0f;
        movementController.enabled = true;
        abilityManager.enabled = true;
        abilityManager.Setup();
        weaponHolder.SetActive(true);

        // xp
        requiredExp = (int)(level * 100 * 1.25);
        hud.SetupXP(level, requiredExp);
        HeroPanel.Instance.UpdateLevel(level);

        HeroPanel.Instance.UpdateCoin(storedCoin);
    }

    private void TakeDamage(float damage)
    {
        if (!isDead)
        {
            health -= damage;
            hud.UpdateHealth(health);

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
        hud.UpdateHealth(health);
        colorGrading.saturation.value = -100f;
        movementController.ResetAnimatorParameters();
        movementController.enabled = false;
        abilityManager.enabled = false;
        weaponHolder.SetActive(false);

        await hud.ShowHugeMessage("You Died", Color.red);
        Respawn();
    }

    private async void Respawn()
    {
        await maskingCanvas.ShowMaskingCanvas(true);
        health = maxHealth;
        Setup();
        await maskingCanvas.ShowMaskingCanvas(false);
    }

    public void Spawn()
    {
        spawnPoint = GameObject.FindGameObjectWithTag("Respawn");
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

    public void SetHealthRegeneration(float healthRegeneration)
    {
        this.healthRegeneration = healthRegeneration;
    }

    public void SetLevel(int level)
    {
        this.level = level;
    }

    public void SetStoredExp(int storedExp)
    {
        this.storedExp = storedExp;
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

    public float GetHealthRegeneration()
    {
        return healthRegeneration;
    }

    public int GetLevel()
    {
        return level;
    }
    
    public int GetStoredExp()
    {
        return storedExp;
    }

    public int GetStoredCoin()
    {
        return storedCoin;
    }
    #endregion

    #region AddDeductChange
    public void ChangeHealth(float value)
    {
        health += value;
    }

    public void AddEXP(int exp)
    {
        storedExp += exp;
        hud.UpdateXP(level, storedExp);
    }

    public void AddCoin(int coin)
    {
        storedCoin += coin;
        HeroPanel.Instance.UpdateCoin(storedCoin);
    }

    public void DeductEXP(int exp)
    {
        storedExp -= exp;
        hud.UpdateXP(level, storedExp);
    }

    public void DeductCoin(int coin)
    {
        storedCoin -= coin;
    }
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
