using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    //[SerializeField] private InventoryPanel inventoryPanel;

    private List<Item> items;

    public Inventory()
    {
        items = new List<Item>();
    }

    public void AddItem(Item item)
    {
        items.Add(item);

        //InventoryPanel.UpdateInventoryPanel(items);
    }

    public void RemoveItem()
    {
        //items.RemoveAt();

        //InventoryPanel.UpdateInventoryPanel(items);
    }

    public void RemoveAllItems()
    {
        for (int i = 0; i < items.Count; i++)
        {
            items.RemoveAt(0);
        }
    }
}
