using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Mob Data", menuName = "Game/Mob Data")]
public class MobData : ScriptableObject
{
    public string mobName;
    public int health;
    public int defense;
    public int attack;
    public float attackSpeed;
    public float speed;
    public float sightDistance;
    public float attackDistance;
    public float walkAroundDistance;
}
