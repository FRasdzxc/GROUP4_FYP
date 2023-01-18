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
    int level = 1;
    int requiredExp;
    int storedExp = 0;
    int storedCoin = 0;

    void Awake()
    {
        maskingCanvas = GameObject.FindGameObjectWithTag("MaskingCanvas").GetComponent<MaskingCanvas>();
    }

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        hud = GameObject.FindGameObjectWithTag("Canvas").GetComponent<HUD>();
        Camera.main.GetComponent<PostProcessVolume>().profile.TryGetSettings(out colorGrading);
        movementController.SetMovementSpeed(heroData.walkspeed);
        abilityManager.Initialize(hud, heroData);
        spawnPoint = GameObject.FindGameObjectWithTag("Respawn");

        // xp
        requiredExp = (int)(level * 100 * 1.25);
        hud.SetupXP(requiredExp);

        profile = ProfileManagerJson.GetProfile(PlayerPrefs.GetString("selectedProfileName"));
        health = profile.health;
        maxHealth = profile.maxHealth;

        Setup();
    }

    // Update is called once per frame
    void Update()
    {
        // test respawn
        if (!isDead)
        {
            if (health < heroData.health)
            {
                health += Time.deltaTime * heroData.healthRegeneration; // temporary only?
                hud.UpdateHealth(health);
            }
            else
            {
                health = heroData.health;
                hud.UpdateHealth(health);
            }

            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.R)) // test only
            {
                Die();
            }
            
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                TakeDamage(5);
            }

            // xp
            hud.UpdateXP(storedExp);
            requiredExp = (int)(level * 100 * 1.25);
            if(storedExp >= requiredExp)
            {
                storedExp -= requiredExp;
                hud.SetupXP(requiredExp);
                level++;
            }
        }
    }

    private void Setup() // useful for respawn
    {
        isDead = false;
        health = heroData.health;
        hud.SetupHealth(health, maxHealth);
        transform.position = spawnPoint.transform.position;
        colorGrading.saturation.value = 0f;
        movementController.enabled = true;
        abilityManager.enabled = true;
        abilityManager.Setup();
        weaponHolder.SetActive(true);
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

        await hud.ShowHugeMessage("You Died", 1.5f, Color.red);
        Respawn();
    }

    private async void Respawn()
    {
        await maskingCanvas.ShowMaskingCanvas(true);
        Setup();
        await maskingCanvas.ShowMaskingCanvas(false);
    }

    // public float GetHealth()
    // {
    //     return health;
    // }

    // public float GetMaxHealth()
    // {
    //     return maxHealth;
    // }

    // public string GetProfileName()
    // {
    //     return profileName;
    // }

    public void SaveProfile() // test only
    {
        // ProfileManagerJson.SaveProfile(profile.profileName, this);
        profile.health = health;
    }

    public int GetLevel()
    {
        return level;
    }
    
    public int GetStoredXP()
    {
        return storedExp;
    }

    public void AddEXP(int exp)
    {
        storedExp += exp;
        hud.UpdateXP(storedExp);
    }

    public void AddCoin(int coin)
    {
        storedCoin += coin;
    }

    public void DeductEXP(int exp)
    {
        storedExp -= exp;
        hud.UpdateXP(storedExp);
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
