using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data", menuName = "Game/Item Data")]
public class ItemData : ScriptableObject
{
    public string itemID; // used for saving?
    public string itemName;
    public ItemType itemType;
    [TextArea(5, 5)] public string itemDescription;
    public float itemValue; // used for selling and earning money?
    public Sprite itemIcon;
}
