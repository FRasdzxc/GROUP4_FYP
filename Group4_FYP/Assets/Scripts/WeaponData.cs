using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponData", menuName = "Game/Weapon Data")]
public class WeaponData : ScriptableObject
{
    public string WeaponName;
    public float WeaponMass;
    public float AttackDamage;
    public float AttackSpeed;
}
