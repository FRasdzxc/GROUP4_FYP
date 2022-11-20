using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory Item", menuName = "ScriptableObjects/InventoryItemData")]
public class InventoryItemData : ScriptableObject
{
    public string Name;
    [TextArea(10, 10)] public string Description;
    public Sprite Sprite; // item sprite
}
