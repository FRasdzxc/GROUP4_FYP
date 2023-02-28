using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Game Weapons List", menuName = "Game/Game Weapons List")]
public class GameWeapons : ScriptableObject
{
    public ClassWeaponEntry[] weaponList;

    public void AssignWeaponId()
    {
        foreach (ClassWeaponEntry cwe in weaponList)
        {
            foreach (WeaponEntry we in cwe.classWeapons)
            {
                foreach (WeaponData wd in we.weaponTiers)
                {
                    wd.weaponId = we.weaponId;
                }
            }
        }
    }
}

[Serializable]
public class ClassWeaponEntry
{
    public HeroClass heroClass;
    [Tooltip("Order from lowest tier to highest tier")] // actually doesn't matter
    public WeaponEntry[] classWeapons;
}

[Serializable]
public class WeaponEntry
{
    [Tooltip("Weapon ID. (e.g. weapon_wand)")]
    public string weaponId;
    public WeaponData[] weaponTiers;
}
