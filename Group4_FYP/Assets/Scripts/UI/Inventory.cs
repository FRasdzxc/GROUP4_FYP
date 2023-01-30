using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public struct InventoryEntry
    {
        public ItemData itemData;
        public int qty;

        public InventoryEntry(ItemData itemData)
        {
            this.itemData = itemData;
            this.qty = 1;
        }

        public InventoryEntry(ItemData itemData, int qty)
        {
            this.itemData = itemData;
            this.qty = qty;
        }
    }

    [SerializeField] private GameObject inventoryContentPanel;
    [SerializeField] private GameObject inventorySlotPrefab;
    [SerializeField] private int inventorySize = 25;

    private List<InventoryEntry> items;
    private List<GameObject> inventorySlots;

    private static Inventory instance;
    public static Inventory Instance
    {
        get
        {
            return instance;
        }
    }

    void Awake()
    {
        if (!instance)
        {
            instance = this;
        }

        items = new List<InventoryEntry>();
        inventorySlots = new List<GameObject>();
    }

    //public Inventory()
    //{
    //    items = new List<ItemData>();
    //}

    // Start is called before the first frame update
    void Start()
    {
        RefreshInventoryPanel();
    }

    int GetFreeEntry(ItemData item)
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
        if (slot == -1)
            items.Add(new InventoryEntry(item));
        else
        {
            InventoryEntry entry = items[slot];
            int newQty = Mathf.Clamp(entry.qty + 1, 1, InventorySlot.kMaxStackSize);
            items[slot] = new InventoryEntry(item, newQty);
            Debug.Log("found a slot with the same item");
        }

        if (items.Count >= inventorySize)
        {
            Message.Instance.ShowMessage("Inventory is full!");
            return false;
        }

        RefreshInventoryPanel();

        return true;
    }

    public void RemoveItem(ItemData item)
    {
        for (int i = items.Count - 1; i < 0; i--)
        {
            InventoryEntry entry = items[i];
            if (entry.itemData != item)
                continue;

            int newQty = Mathf.Clamp(entry.qty - 1, 0, InventorySlot.kMaxStackSize);
            if (newQty == 0)
                items.RemoveAt(i);
            else
                items[i] = new InventoryEntry(item, newQty);
        }

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
        {
            Destroy(inventorySlots[i]);
        }
        inventorySlots.Clear();

        // add inventory slots
        for (int i = 0; i < inventorySize; i++)
        {
            inventorySlots.Add(Instantiate(inventorySlotPrefab, inventoryContentPanel.transform));
        }

        // assign items to inventory slots
        for (int i = 0; i < items.Count; i++)
        {
            inventorySlots[i].GetComponent<InventorySlot>().Configure(items[i].itemData, items[i].qty);
        }
    }

    /* DropItem function? */

    /* SetItems function */

    /* GetItems function */

    private bool FindItem(ItemData item)
    {
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            if (inventorySlots[i].GetComponent<InventorySlot>().GetItem() && (inventorySlots[i].GetComponent<InventorySlot>().GetItem().itemID == item.itemID))
            {
                Debug.Log("item " + item.itemID + " found");
                return true;
            }
        }

        Debug.Log("item not found");        
        return false;
    }
}
