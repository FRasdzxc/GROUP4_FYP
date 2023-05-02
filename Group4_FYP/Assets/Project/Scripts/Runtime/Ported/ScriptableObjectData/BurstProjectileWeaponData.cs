using UnityEngine;

[CreateAssetMenu(fileName = "New Burst Projectile Weapon Data", menuName = "Game/Weapons/Burst Projectile Weapon Data")]
public class BurstProjectileWeaponData : ProjectileWeaponData
{
    public int projectilePerBurst;
    public float burstCooldown;
}
