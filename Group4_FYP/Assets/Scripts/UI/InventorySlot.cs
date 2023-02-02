using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public const int kMaxStackSize = 99;

    [SerializeField] private InventorySlotType inventorySlotType; // for weapon/armor/ability inventory slots that only accept one type of item?
    [SerializeField] private Image image;
    [SerializeField] private Text stackSizeText;
    [SerializeField] private GameObject useHint;

    private int _stackSize = 0;
    public int StackSize
    {
        get => _stackSize;
        private set
        {
            _stackSize = Mathf.Clamp(value, 0, kMaxStackSize);
            if (stackSizeText != null)
            {
                stackSizeText.gameObject.SetActive(_stackSize > 1);
                stackSizeText.text = _stackSize.ToString();
            }
        }
    }

    private ItemData item = null; // change to use list/array instead to support stacked items?
    public ItemData ItemData
    {
        get => item;
        private set
        {
            item = value;
            image.enabled = item != null;
            image.sprite = item != null ? item.itemIcon : null;
        }
    }

    private bool isOccupied = false;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Configure(ItemData item, int stackSize)
    {
        if (this.item != null && this.item != item)
        {
            Debug.LogError("Mismatched item!");
            return;
        }

        ItemData = item;
        StackSize = stackSize;
    }

    public bool AddItem(ItemData item)
    {
        if (this.item != null && this.item != item)
            return false;

        ItemData = item;
        StackSize++;
        Debug.Log($"{item.itemName}:{StackSize}");

        isOccupied = true;
        return true;
    }

    // public void ClearSlot()
    // {
    //     item = null;
    //     StackSize = 0;

    //     // get item to follow mouse, remove item from this.item

    //     isOccupied = false;
    // }

    public void UseItem()
    {
        Debug.Log("use item: " + ItemData.itemName);

        // use occupied item
        ItemData.Use();
        Inventory.Instance.RemoveItem(ItemData);

        StackSize--;
    }

    public void DropItem()
    {
        Debug.Log("drop item: " + ItemData.itemName);
        
        // drop item to scene
        // clear slot
        // remove item from inventory.items
        // refresh inventory panel
        
        Inventory.Instance.RemoveItem(ItemData);

        StackSize--;
    }

    public ItemData GetItem()
    {
        return ItemData;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerEnter == gameObject)
        {
            Debug.Log("cursor enters inventory slot");

            if (item && item.isUsable)
            {
                useHint.SetActive(true);
            }
        }
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerEnter == gameObject)
        {
            Debug.Log("cursor exits inventory slot");

            useHint.SetActive(false);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.pointerClick == gameObject && item)
        {
            if (eventData.button == PointerEventData.InputButton.Left && item.isUsable) // LMB: use item
            {
                UseItem();
            }
            else if (eventData.button == PointerEventData.InputButton.Right) // RMB: drop item
            {
                DropItem();
            }
        }
    }
}
