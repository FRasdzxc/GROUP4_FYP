using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Hero : MonoBehaviour
{
    [SerializeField] private HeroData heroData;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Text healthText;
    [SerializeField] private GameObject spawnPoint;
    //[SerializeField] private Slider manaSlider;
    //[SerializeField] private Text manaText;
    private float health;
    private SpriteRenderer sr;
    private bool isDead;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        Setup();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead && Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.R)) // test
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
    }

    private void TakeDamage(float damage)
    {
        health -= damage;
        UpdateUI();

        if (health <= 0)
        {
            
            Die();
        }
    }

    private async void Die()
    {
        isDead = true;
        health = 0;
        UpdateUI();
        await transform.DOScale(0, 0.5f).AsyncWaitForCompletion();
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
