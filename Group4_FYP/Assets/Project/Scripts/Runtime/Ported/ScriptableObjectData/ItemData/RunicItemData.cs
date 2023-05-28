using UnityEngine;

[CreateAssetMenu(fileName = "New Runic Item Data", menuName = "Game/Items/Runic Item Data")]
public class RunicItemData : ItemData
{
    public int tier; // upgrade weapon with corresponding tier + 1
    public int upgradePrice;
}
