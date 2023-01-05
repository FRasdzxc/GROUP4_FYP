using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// used for player/enemy weapon that will collide with player/enemy

[CreateAssetMenu(fileName = "New Weapon Point Data", menuName = "Game/Weapons/Weapon Point Data")]
public class WeaponPointData : ScriptableObject
{
    public string weaponPointName;

    public float damage;
    public float criticalDamage;
}
