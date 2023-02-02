using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TinyScript;

public class Mob : MonoBehaviour
{
    [SerializeField] protected MobData mobData;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private LootDrop loot;
    [SerializeField] private int randomDropCount = 1;
    [SerializeField] private float dropRange = 0.5f;
    [SerializeField] private AudioClip[] damageSoundClips;
    [SerializeField] private AudioClip[] dieSoundClips;
    [SerializeField] private AudioSource audioSource;
    protected float health;
    protected float sightDistance;
    protected float attackDistance;
    protected float speed;
    protected GameObject player;
    protected bool isDead;
    protected SpriteRenderer sr;
    protected Rigidbody2D rb2D;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        health = mobData.health;
        sightDistance = mobData.sightDistance;
        attackDistance = mobData.attackDistance;
        speed = mobData.speed;
        player = GameObject.FindWithTag("Player");
        isDead = false; // preventive
        sr = GetComponent<SpriteRenderer>();
        rb2D = GetComponent<Rigidbody2D>();
        healthSlider.maxValue = health;
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            if (Vector2.Distance(transform.position, player.transform.position) <= attackDistance)
            {
                AttackPlayer();
            }
            else if (Vector2.Distance(transform.position, player.transform.position) <= sightDistance)
            {
                ChasePlayer();
            }
            else
            {
                WalkAround();
            }
        }
        else
        {
            sr.color = Color.red;
        }
    }

    protected virtual void AttackPlayer()
    {

    }

    protected virtual void AttackMethod()
    {

    }

    protected virtual void ChasePlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }

    private void WalkAround()
    {

    }

    private void TakeDamage(float damage)
    {
        health -= damage;
        UpdateUI();
        PlaySound(damageSoundClips[Random.Range(0, damageSoundClips.Length)]);

        if (health <= 0)
        {
            Die();
        }
    }

    private async void Die()
    {
        isDead = true;
        health = 0;
        PlaySound(dieSoundClips[Random.Range(0, dieSoundClips.Length)]);
        UpdateUI();
        await transform.DOScale(0, 0.5f).AsyncWaitForCompletion();

        if (this) // trying to prevent MissingReferenceException
        {
            loot.SpawnDrop(transform, randomDropCount, dropRange);
            Destroy(gameObject);
        }
    }

    private void UpdateUI()
    {
        healthSlider.DOValue(health, 0.25f).SetEase(Ease.OutQuart);
    }

    private void PlaySound(AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        audioSource.Play();
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.CompareTag("HeroWeaponTrigger") || collision.CompareTag("HeroWeaponTriggerStronger")) && collision.GetComponent<WeaponTrigger>())
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
