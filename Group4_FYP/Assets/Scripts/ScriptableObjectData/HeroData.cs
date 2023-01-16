using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Hero Data", menuName = "Game/Hero Data")]
public class HeroData : ScriptableObject
{
    public float health;
    public float healthRegeneration;
    public float mana;
    public float manaRegeneration;
    public float attackSpeed;
    public float luck;
    public float defense;
    public float walkspeed;
}
