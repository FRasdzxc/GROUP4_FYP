using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Data", menuName = "Game/Weapons/Weapon Data")]

public class WeaponData : ScriptableObject
{
    public string weaponName;
    public int weaponTier;
    public float cooldown;
    public GameObject weaponGobj;
    public WeaponItemData item;
}
