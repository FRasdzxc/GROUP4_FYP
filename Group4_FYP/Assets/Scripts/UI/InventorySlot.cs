using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public const int kMaxStackSize = 99;

    [SerializeField] private InventorySlotType inventorySlotType; // for weapon/armor/ability inventory slots that only accept one type of item? // not used?
    [SerializeField] private Image image;
    [SerializeField] private Text stackSizeText;
    [SerializeField] private GameObject hint;
    [SerializeField] private Text hintText;

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

    // private bool isOccupied = false;
    private InventoryMode inventoryMode;

    public void Configure(ItemData item, int stackSize, InventoryMode inventoryMode = InventoryMode.Normal)
    {
        if (this.item != null && this.item != item)
        {
            Debug.LogError("Mismatched item!");
            return;
        }

        ItemData = item;
        StackSize = stackSize;
        this.inventoryMode = inventoryMode;

        if (inventoryMode == InventoryMode.Normal)
        {
            hintText.text = "Use";
        }
        else
        {
            hintText.text = "Transfer";
        }
    }

    //public bool AddItem(ItemData item)
    //{
    //    if (this.item != null && this.item != item)
    //        return false;

    //    ItemData = item;
    //    StackSize++;
    //    Debug.Log($"{item.itemName}:{StackSize}");

    //    isOccupied = true;
    //    return true;
    //}

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

            List<string> attributes = new List<string>();
            List<TooltipHintType> hints = new List<TooltipHintType>();
            if (ItemData is ConsumableItemData)
            {
                var consumable = item as ConsumableItemData;
                foreach (var e in consumable.effects)
                {
                    attributes.Add(e.ToString());
                }
                hints.AddRange(new List<TooltipHintType>() { TooltipHintType.Use, TooltipHintType.UseAll });
            }
            hints.AddRange(new List<TooltipHintType>() { TooltipHintType.Drop, TooltipHintType.DropAll });

            Tooltip.Instance.ShowTooltip(item.itemName, item.itemDescription, attributes.ToArray(), hints.ToArray());

            if (inventoryMode == InventoryMode.Normal)
            {
                if (item && item.isUsable)
                {
                    hint.SetActive(true);
                }
            }
            else // transfer
            {
                hint.SetActive(true);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerEnter == gameObject)
        {
            Debug.Log("cursor exits inventory slot");
            Tooltip.Instance.HideTooltip();

            hint.SetActive(false);
        }        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.pointerClick == gameObject && item)
        {
            if (inventoryMode == InventoryMode.Normal) // use and drop items
            {
                if (eventData.button == PointerEventData.InputButton.Left && item.isUsable) // LMB: use item
                {
                    UseItem();

                    if (Input.GetKey(KeyCode.LeftShift)) // use all
                    {
                        while (StackSize > 0)
                        {
                            UseItem();
                        }
                    }
                }
                else if (eventData.button == PointerEventData.InputButton.Right) // RMB: drop item
                {
                    DropItem();

                    if (Input.GetKey(KeyCode.LeftShift)) // drop all
                    {
                        while (StackSize > 0)
                        {
                            DropItem();
                        }
                    }
                }
            }
            else // transfer
            {
                if (eventData.button == PointerEventData.InputButton.Left)
                {
                    BuySellPanel.Instance.TransferItem(ItemData);
                }
            }
        }
    }
}
