using UnityEngine;

[CreateAssetMenu(fileName = "New Projectile Weapon Data", menuName = "Game/Weapons/Projectile Weapon Data")]
public class ProjectileWeaponData : WeaponData
{
    //public Sprite Icon;
    //public string WeaponName;
    //public float WeaponMass; // mass that could slow down the character
    //public float AttackDamageMultiplier; // AttackDamageMultiplier * projectile's damage = total damage
    //public float AttackCooldown; // seconds until next attack
    //public ScriptableObject[] Projectiles;
    //public Sprite WeaponPrefab;
    //public GameObject AutoAttack;

    public float projectileLifeTime;
    public float projectileSpeed;
    public Vector2 projectileOffset = Vector2.zero;
}
