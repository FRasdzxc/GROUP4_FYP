using UnityEngine;

[CreateAssetMenu(fileName = "New Relic Item Data", menuName = "Game/Items/Relic Item Data")]
public class RelicItemData : ItemData
{
    public int tier; // upgrade weapon with corresponding tier + 1
    public int upgradePrice;
}
