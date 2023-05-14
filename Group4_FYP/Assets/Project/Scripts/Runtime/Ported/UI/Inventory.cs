using System;
using System.Collections.Generic;
using UnityEngine;
using PathOfHero.Utilities;

[Serializable]
public class InventoryEntry
{
    public ItemData itemData;
    public string itemID;
    public int qty;

    public InventoryEntry(ItemData itemData)
    {
        this.itemData = itemData;
        this.itemID = itemData.itemID;
        this.qty = 1;
    }

    public InventoryEntry(ItemData itemData, int qty)
    {
        this.itemData = itemData;
        this.itemID = itemData.itemID;
        this.qty = qty;
    }
}

public class Inventory : Singleton<Inventory>
{
    //[SerializeField] private ItemData[] gameItems;
    [SerializeField] private GameItems gameItems;
    [SerializeField] private Transform inventoryContentPanelTransform;
    [SerializeField] private GameObject inventorySlotPrefab;
    [SerializeField] private int inventorySize = 32;
    [SerializeField] private AudioClip[] addRemoveSounds;

    private List<InventoryEntry> items = new List<InventoryEntry>();
    private List<GameObject> inventorySlots = new List<GameObject>();
    private bool messageIsShown;

    // Start is called before the first frame update
    void Start()
        => RefreshInventoryPanel();

    public int GetFreeEntry(ItemData item)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].itemData == item && items[i].qty < InventorySlot.kMaxStackSize)
                return i;
        }

        return -1;
    }

    public bool AddItem(ItemData item)
    {
        int slot = GetFreeEntry(item);

        if (items.Count >= inventorySize)
        {
            if (slot == -1) // no free slots either
            {
                if (!messageIsShown)
                {
                    Message.Instance.ShowMessage("Inventory is full!");
                    messageIsShown = true;
                }
                
                return false;
            }
        }

        if (slot == -1)
            items.Add(new InventoryEntry(item));
        else
        {
            InventoryEntry entry = items[slot];
            int newQty = Mathf.Clamp(entry.qty + 1, 1, InventorySlot.kMaxStackSize);
            items[slot] = new InventoryEntry(item, newQty);
        }

        if (addRemoveSounds.Length > 0)
            AudioManager.Instance.PlaySound(addRemoveSounds[UnityEngine.Random.Range(0, addRemoveSounds.Length)]);
        
        RefreshInventoryPanel();
        return true;
    }

    public void RemoveItem(ItemData item)
    {
        for (int i = items.Count - 1; i >= 0; i--)
        {
            InventoryEntry entry = items[i];
            if (entry.itemData != item)
                continue;

            int newQty = Mathf.Clamp(entry.qty - 1, 0, InventorySlot.kMaxStackSize);
            if (newQty == 0)
                items.RemoveAt(i);
            else
                items[i] = new InventoryEntry(item, newQty);
            
            break;
        }
        
        if (addRemoveSounds.Length > 0)
            AudioManager.Instance.PlaySound(addRemoveSounds[UnityEngine.Random.Range(0, addRemoveSounds.Length)]);

        RefreshInventoryPanel();
    }

    //public void RemoveAllItems()
    //{
    //    for (int i = 0; i < items.Count; i++)
    //    {
    //        items.RemoveAt(0);
    //    }
    //}

    public void RefreshInventoryPanel()
    {
        // destroy all the inventory slots
        for (int i = 0; i < inventorySlots.Count; i++)
            Destroy(inventorySlots[i]);
        inventorySlots.Clear();

        // add inventory slots
        for (int i = 0; i < inventorySize; i++)
            inventorySlots.Add(Instantiate(inventorySlotPrefab, inventoryContentPanelTransform));

        // assign items to inventory slots
        for (int i = 0; i < items.Count; i++)
            inventorySlots[i].GetComponent<InventorySlot>().Configure(items[i].itemData, items[i].qty);

        // check if inventory isn't full
        if (items.Count < inventorySlots.Count)
            messageIsShown = false;
    }

    /* DropItem function? */

    /* SetItems function */
    public void SetItems(List<InventoryEntry> items)
    {
        this.items.Clear();

        foreach (InventoryEntry ie in items)
        {
            foreach (ItemData i in gameItems.itemList)
            {
                if (ie.itemID.Equals(i.itemID))
                    this.items.Add(new InventoryEntry(i, ie.qty));
            }
        }

        RefreshInventoryPanel();
    }

    /* GetItems function */
    public List<InventoryEntry> GetItems()
    {
        // clone new list to prevent pass-by-reference
        return new List<InventoryEntry>(items);
    }

    public int GetInventorySize()
    {
        return inventorySize;
    }

    public bool IsFull(int alteredValue = 0)
    {
        return ((items.Count + alteredValue) >= inventorySize);
    }

    // private bool FindItem(ItemData item)
    // {
    //     for (int i = 0; i < inventorySlots.Count; i++)
    //     {
    //         if (inventorySlots[i].GetComponent<InventorySlot>().GetItem() && (inventorySlots[i].GetComponent<InventorySlot>().GetItem().itemID == item.itemID))
    //         {
    //             Debug.Log("item " + item.itemID + " found");
    //             return true;
    //         }
    //     }

    //     Debug.Log("item not found");        
    //     return false;
    // }
}
