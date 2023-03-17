using UnityEngine;

[CreateAssetMenu(fileName = "New Armor Item Data", menuName = "Game/Items/Armor Item Data")]
public class ArmorItemData : ItemData
{
    public float defense; // not yet usable; unit: percentage

    public override void Use()
    {
        base.Use();

        // implement logic
    }
}
