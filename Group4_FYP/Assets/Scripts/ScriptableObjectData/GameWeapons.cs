using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Game Weapons List", menuName = "Game/Game Weapons List")]
public class GameWeapons : ScriptableObject
{
    public ClassWeaponEntry[] weaponList;
}

[Serializable]
public class WeaponEntry
{
    public WeaponData weaponData;
}

[Serializable]
public class ClassWeaponEntry
{
    public HeroClass heroClass;
    [Tooltip("Order from lowest tier to highest tier")] // actually doesn't matter
    public WeaponEntry[] classWeapons;
}
