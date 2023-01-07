using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Mob : MonoBehaviour
{
    [SerializeField] protected MobData mobData;
    [SerializeField] private Slider healthSlider;
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
        Debug.Log("AttackPlayer()");
    }

    private void ChasePlayer()
    {
        Debug.Log("ChasePlayer()");

        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }

    private void WalkAround()
    {
        Debug.Log("WalkAround()");
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

        if (this) // trying to prevent MissingReferenceException
        {
            Destroy(gameObject);
        }
    }

    private void UpdateUI()
    {
        healthSlider.DOValue(health, 0.25f);
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("HeroWeaponPoint") && collision.GetComponent<WeaponPoint>())
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
