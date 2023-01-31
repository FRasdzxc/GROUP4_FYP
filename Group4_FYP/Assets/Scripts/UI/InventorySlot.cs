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
        
        // if item != null && this.item == item, stack items?
        // else return? or swap item to mouse?
    }

    public void ClearSlot()
    {
        item = null;
        StackSize = 0;

        // get item to follow mouse, remove item from this.item

        isOccupied = false;
    }

    public void UseItem()
    {
        Debug.Log("use item: " + item.itemName);

        // if stackSize > 1, stackSize--, else ClearSlot()
        // remove 1 item from inventory.items
        // call Inventory.Instance.RefreshInventoryPanel(); ?
        // above are not finished

        // use occupied item
        item.Use();
    }

    public void DropItem()
    {
        Debug.Log("drop item: " + item.itemName);
        
        // drop item to scene
        // clear slot
        // remove item from inventory.items
        // refresh inventory panel
    }

    public ItemData GetItem()
    {
        return item;
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
        if (eventData.pointerClick == gameObject && item && item.isUsable)
        {
            if (eventData.button == PointerEventData.InputButton.Left) // LMB: use item
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
