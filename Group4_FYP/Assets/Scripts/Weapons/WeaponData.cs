using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Data", menuName = "Game/Weapons/Weapon Data")]

public class WeaponData : ScriptableObject
{
    public string weaponName;
    public float cooldown;
    public int weaponTier;
    public GameObject weaponGobj;
}
