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
    [SerializeField] private GameObject weaponHolder;

    [SerializeField] private MovementControllerV2 movementController;
    [SerializeField] private AbilityManager abilityManager;
    [SerializeField] private HUD hud;
    private float health;
    private SpriteRenderer sr;
    private bool isDead;
    private ColorGrading colorGrading;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        Camera.main.GetComponent<PostProcessVolume>().profile.TryGetSettings(out colorGrading);
        movementController.SetMovementSpeed(heroData.walkspeed);
        abilityManager.Initialize(hud, heroData);

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
                health += Time.deltaTime * heroData.healthRegeneration; // temporary only
                hud.UpdateHealth(health);
            }
            else
            {
                health = heroData.health;
                hud.UpdateHealth(health);
            }

            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.R))
            {
                Die();
            }
        }
    }

    private void Setup() // useful for respawn
    {
        isDead = false;
        health = heroData.health;
        hud.SetupHealth(health);
        transform.position = spawnPoint.transform.position;
        colorGrading.saturation.value = 0f;
        movementController.enabled = true;
        abilityManager.enabled = true;
        abilityManager.Setup();
        hud.SetDeathMessageActive(false);
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

        hud.SetDeathMessageActive(true);
        await Task.Delay(1500);
        Respawn();
    }

    private void Respawn()
    {
        Setup();
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
