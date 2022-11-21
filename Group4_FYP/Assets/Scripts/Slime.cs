using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TinyScript;

public class Slime : MonoBehaviour
{
    public MobData m_data;
    public string mobName;
    public float health;
    public float defense;
    public float attack;
    public float attackSpeed;
    public float speed;
    public float takeDamage;
    public bool takenDamage = false;
    public LootDrop Loot;
    public int RandomDropCount = 1;
    public float DropRange = .5f;
    void Start()
    {
        health = m_data.health;
        defense = m_data.defense;
        attack = m_data.attack;
        attackSpeed = m_data.attackSpeed;
        speed = m_data.speed;

    }
    // Update is called once per frame
    void Update()
    {
        if(health <= 0)
        {
            Loot.SpawnDrop(this.transform, RandomDropCount, DropRange);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        takeDamage = collision.gameObject.GetComponent<ProjectilesController>().damage;
        health -= takeDamage;
        takenDamage = true;
        Debug.Log(health);
    }
}
