using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Item Data", menuName = "Game/Items/Weapon Item Data")]
public class WeaponItemData : ItemData
{
    public WeaponData weapon;

    public override void Use()
    {
        base.Use();

        WeaponManager.Instance.EquipWeapon(weapon);
    }
}
