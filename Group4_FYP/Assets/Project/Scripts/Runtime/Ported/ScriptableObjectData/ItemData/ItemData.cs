using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data", menuName = "Game/Items/Item Data")]
public class ItemData : ScriptableObject, IEquatable<ItemData>
{
    public string itemID; // used for saving?
    public string itemName;
    public ItemType itemType;
    [TextArea(5, 5)]
    public string itemDescription;
    public int minDropSize;
    public int maxDropSize;
    [Tooltip("Used for selling and earning money")]
    public int sellPrice; // used for selling and earning money?
    public int buyPrice;
    public Sprite itemIcon;
    public AudioClip useSound;
    public bool isUsable;
    public bool isBuyable; // won't show up in shop if false

    public bool Equals(ItemData other)
        => itemID == other.itemID;

    public virtual void Use() { }
}
