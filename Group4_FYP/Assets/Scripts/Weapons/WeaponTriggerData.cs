using UnityEngine;

// used for player/enemy weapon that will collide with player/enemy

[CreateAssetMenu(fileName = "New Weapon Trigger Data", menuName = "Game/Weapons/Weapon Trigger Data")]
public class WeaponTriggerData : ScriptableObject
{
    public string weaponTriggerName;

    public float damage;
    public float criticalDamage;
    public float projectileSpeed;
    public bool splitable;
    public GameObject splitProjectile;
    public int splitAmount;
    public float splitProjectileSpeed = 1;
    public float splitTime; //split after splitTime
}
