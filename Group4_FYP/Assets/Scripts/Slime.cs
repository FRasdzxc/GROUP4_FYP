using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Slime : MonoBehaviour
{
    public MobData m_data;
    public string mobName;
    public int health;
    public int defense;
    public int attack;
    public float attackSpeed;
    public float speed;
    void Start()
    {

        health = m_data.health;
        defense = m_data.defense;
        attack = m_data.attack;
        attackSpeed = m_data.attackSpeed;
        speed = m_data.speed;

    }
    // Update is called once per frame
    async void Update()
    {

    }
}
