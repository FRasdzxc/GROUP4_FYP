using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMobData", menuName = "Game/Mob")]
public class MobData : ScriptableObject
{
    public string mobName;
    public int health;
    public int defense;
    public int attack;
    public float attackSpeed;
    public float speed;
}
