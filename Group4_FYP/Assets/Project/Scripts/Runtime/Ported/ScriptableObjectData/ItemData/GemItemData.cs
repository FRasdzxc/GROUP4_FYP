using UnityEngine;

[CreateAssetMenu(fileName = "New Gem Item Data", menuName = "Game/Items/Gem Item Data")]
public class GemItemData : ItemData
{
    public ItemData[] fragments;
    public int fusePrice;

    public void Fuse()
    {
        // confirm fusing
            // check price
            // give gem
            // remove fragments
            // deduct coin
    }
}
