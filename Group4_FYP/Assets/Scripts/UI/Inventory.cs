using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private GameObject inventoryContentPanel;
    [SerializeField] private GameObject inventorySlotPrefab;
    [SerializeField] private int inventorySize = 25;
    
    private List<ItemData> items;
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

        items = new List<ItemData>();
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

    public bool AddItem(ItemData item)
    {
        FindItem(item);

        if (items.Count >= inventorySize)
        {
            Message.Instance.ShowMessage("Inventory is full!");
            return false;
        }
        items.Add(item);

        RefreshInventoryPanel();

        return true;
    }

    public void RemoveItem(ItemData item)
    {
        items.Remove(item);

        RefreshInventoryPanel();
    }

    //public void RemoveAllItems()
    //{
    //    for (int i = 0; i < items.Count; i++)
    //    {
    //        items.RemoveAt(0);
    //    }
    //}

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

    private void RefreshInventoryPanel()
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
            inventorySlots[i].GetComponent<InventorySlot>().AddItem(items[i]);
        }
    }
}
