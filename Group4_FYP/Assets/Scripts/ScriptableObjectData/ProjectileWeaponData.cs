using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NormalWeaponData", menuName = "ScriptableObjects/Weapons/ProjectileWeaponData")]
public class ProjectileWeaponData : ScriptableObject
{
    public Sprite Icon;
    public string WeaponName;
    public float WeaponMass; // mass that could slow down the character
    public float AttackDamage;
    public float AttackCooldown; // seconds until next attack
    public ScriptableObject[] Projectiles;
    public GameObject WeaponPrefab;
    public GameObject AutoAttack;
}
