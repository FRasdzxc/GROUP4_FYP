using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Data", menuName = "Game/Weapons/Weapon Data")]

public class WeaponData : ScriptableObject
{
    public string weaponName;
    public float cooldown;
}
