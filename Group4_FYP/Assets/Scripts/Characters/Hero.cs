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

    [SerializeField] private GameObject spawnPoint;

    [SerializeField] private Slider healthSlider;
    [SerializeField] private Text healthText;
    [SerializeField] private GameObject deathMessage;

    [SerializeField] private MovementControllerV2 movementController;
    [SerializeField] private AbilityManager abilityManager;
    //[SerializeField] private Slider manaSlider;
    //[SerializeField] private Text manaText;
    private float health;
    private SpriteRenderer sr;
    private bool isDead;
    private ColorGrading colorGrading;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        Camera.main.GetComponent<PostProcessVolume>().profile.TryGetSettings(out colorGrading);

        Setup();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead && Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.R)) // test
        {
            Die();
        }
    }

    private void Setup() // useful for respawn
    {
        isDead = false;
        health = heroData.health;
        healthSlider.maxValue = health;
        UpdateUI();
        transform.position = spawnPoint.transform.position;
        transform.localScale = Vector3.one;
        colorGrading.saturation.value = 0f;
        movementController.enabled = true;
        abilityManager.enabled = true;
        abilityManager.ReadyEquippedAbilities();
        deathMessage.SetActive(false);
    }

    private void TakeDamage(float damage)
    {
        if (!isDead)
        {
            health -= damage;
            UpdateUI();

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
        UpdateUI();
        colorGrading.saturation.value = -100f;
        movementController.ResetAnimatorParameters();
        movementController.enabled = false;
        abilityManager.enabled = false;

        deathMessage.SetActive(true);
        await deathMessage.transform.DOScaleX(1, 0.25f).AsyncWaitForCompletion();
        await Task.Delay(1500);
        await deathMessage.transform.DOScaleX(0, 0.25f).AsyncWaitForCompletion();
        Respawn();
    }

    private void Respawn()
    {
        Setup();
    }

    private void UpdateUI()
    {
        healthSlider.DOValue(health, 0.25f);
        healthText.text = health.ToString() + " HP";
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("MobWeaponPoint") && collision.GetComponent<WeaponPoint>())
        {
            TakeDamage(collision.GetComponent<WeaponPoint>().GetDamage(false));
            sr.color = Color.red;
        }
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        sr.color = Color.white;
    }
}
