using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public const int kMaxStackSize = 99;

    [SerializeField] private InventorySlotType inventorySlotType; // for weapon/armor/ability inventory slots that only accept one type of item?
    [SerializeField] private Image image;
    [SerializeField] private Text stackSizeText;
    [SerializeField] private Button useButton;
    [SerializeField] private Text useButtonText;

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
            useButton.onClick.AddListener(() => { UseItem(); });
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
        // use occupied item
        // if stackSize > 1, stackSize--, else ClearSlot()
        // call Inventory.Instance.RefreshInventoryPanel(); ?
    }

    public ItemData GetItem()
    {
        return item;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("cursor enters inventory slot");

        if (item)
        {
            useButton.gameObject.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("cursor exits inventory slot");

        useButton.gameObject.SetActive(false);
    }
}
