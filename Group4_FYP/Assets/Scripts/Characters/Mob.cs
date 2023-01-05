using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : MonoBehaviour
{
    [SerializeField] private MobData mobData;
    private float health;
    private float sightDistance;
    private float attackDistance;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        health = mobData.health;
        sightDistance = mobData.sightDistance;
        attackDistance = mobData.attackDistance;
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
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

    private void AttackPlayer()
    {
        Debug.Log("AttackPlayer()");
    }

    private void ChasePlayer()
    {
        Debug.Log("ChasePlayer()");
    }

    private void WalkAround()
    {
        Debug.Log("WalkAround()");
    }

    private void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerWeaponPoint") && collision.GetComponent<WeaponPoint>())
        {
            TakeDamage(collision.GetComponent<WeaponPoint>().GetDamage(false));
        }
    }
}
