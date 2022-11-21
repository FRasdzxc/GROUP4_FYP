using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHeroData", menuName = "Game/Hero Data")]
public class HeroData : ScriptableObject
{
    public string HeroName;
    public float Health;
    public float Mana;
    public float AttackSpeed;
    public float Luck;
    public float Defense;
    public float Walkspeed;
}
