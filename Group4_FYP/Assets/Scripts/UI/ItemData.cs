using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data", menuName = "Game/Item Data")]
public class ItemData : ScriptableObject, IEquatable<ItemData>
{
    public string itemID; // used for saving?
    public string itemName;
    public ItemType itemType;
    [TextArea(5, 5)] public string itemDescription;
    public int minDropSize;
    public int maxDropSize;
    public float itemValue; // used for selling and earning money?
    public Sprite itemIcon;

    public bool Equals(ItemData other)
    {
        return itemID == other.itemID;
    }

    public virtual void Use()
    {
        Debug.Log("using item " + itemName);
    }
}
