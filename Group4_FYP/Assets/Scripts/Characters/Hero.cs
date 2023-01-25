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
    private float health;
    private float maxHealth;
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
            //if (health < heroData.health)
            if (health < maxHealth)
            {
                health += Time.deltaTime * healthRegeneration;
                hud.UpdateHealth(health);
            }
            else
            {
                //health = heroData.health;
                health = maxHealth;
                hud.UpdateHealth(health);
            }
            
            if (Input.GetKeyDown(KeyCode.Backspace)) // test only
            {
                TakeDamage(15);
            }

            // xp
            hud.UpdateXP(level, storedExp);
            requiredExp = (int)(level * 100 * 1.25);
            if(storedExp >= requiredExp)
            {
                storedExp -= requiredExp;
                hud.SetupXP(level, requiredExp);
                level++;
                _ = Notification.Instance.ShowNotification("Level Up! - " + level);
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

    public void AddEXP(int exp)
    {
        storedExp += exp;
        hud.UpdateXP(level, storedExp);
    }

    public void AddCoin(int coin)
    {
        storedCoin += coin;
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

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("MobWeaponTrigger") && collision.GetComponent<WeaponTrigger>())
        {
            TakeDamage(collision.GetComponent<WeaponTrigger>().GetDamage(false));
            sr.color = Color.red;
        }
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        sr.color = Color.white;
    }
}
