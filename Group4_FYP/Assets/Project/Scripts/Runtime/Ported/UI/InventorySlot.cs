using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// rewrite this class?
public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public const int kMaxStackSize = 99;

    [SerializeField] private InventorySlotType inventorySlotType; // for weapon/armor/ability inventory slots that only accept one type of item? // not used?
    [SerializeField] private Image image;
    [SerializeField] private Text stackSizeText;
    [SerializeField] private GameObject hint;
    [SerializeField] private Text hintText;

    private int m_StackSize = 0;
    public int StackSize
    {
        get => m_StackSize;
        private set
        {
            if (m_StackSize == value)
                return;

            m_StackSize = Mathf.Clamp(value, 0, kMaxStackSize);
            stackSizeText.gameObject.SetActive(m_StackSize > 1);
            stackSizeText.text = m_StackSize.ToString();
        }
    }

    private ItemData m_Item;
    public ItemData ItemData
    {
        get => m_Item;
        private set
        {
            if (m_Item == value)
                return;

            m_Item = value;
            image.enabled = m_Item != null;
            image.sprite = m_Item != null ? m_Item.itemIcon : null;
        }
    }

    // private bool isOccupied = false;
    private InventoryMode inventoryMode;

    public void Configure(ItemData item, int stackSize, InventoryMode inventoryMode = InventoryMode.Normal)
    {
        if (m_Item != null && m_Item != item)
        {
            Debug.LogError("[Inventory Slot] Switching item data is not allowed.");
            return;
        }

        ItemData = item;
        StackSize = stackSize;
        this.inventoryMode = inventoryMode;

        switch (inventoryMode)
        {
            case InventoryMode.Normal:
                hintText.text = "Use";
                break;
            case InventoryMode.Apply:
            case InventoryMode.Revert:
                hintText.text = "Transfer";
                break;
            case InventoryMode.Throw:
                hintText.text = "Throw";
                break;
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

    public void Clear()
    {
        ItemData = null;
        StackSize = 0;
        inventoryMode = InventoryMode.Preview;
    }

    public void UseItem()
    {
        // Debug.Log("use item: " + ItemData.itemName);

        // use occupied item
        ItemData.Use();

        if (!(ItemData is WeaponItemData))
        {
            Inventory.Instance.RemoveItem(ItemData);
            StackSize--;
        }

        // WeaponManager.cs will remove the item instead
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

    public void OnPointerEnter(PointerEventData eventData) // rewrite this with switch case? it looks messy
    {
        if (m_Item == null)
            return;

        List<string> attributes = new List<string>();
        List<TooltipHintType> hints = new List<TooltipHintType>();

        if (ItemData is ConsumableItemData)
        {
            var consumable = ItemData as ConsumableItemData;
            foreach (var e in consumable.effects)
            {
                attributes.Add(e.ToString());
            }
        }
        else if (ItemData is WeaponItemData)
        {
            var weapon = ItemData as WeaponItemData;
            attributes.Add($"Tier: {weapon.weapon.weaponTier}");
            attributes.Add($"Cooldown: {weapon.weapon.cooldown} seconds");

            if (weapon.weapon is ProjectileWeaponData)
            {
                var projectileWeapon = weapon.weapon as ProjectileWeaponData;
                attributes.Add($"Projectile Speed: {projectileWeapon.projectileSpeed}");
            }
        }
        else if (ItemData is ArmorItemData)
        {
            var armor = ItemData as ArmorItemData;
            attributes.Add($"Defense: {armor.defense}");
        }
        else if (ItemData is RelicItemData)
        {
            var relic = ItemData as RelicItemData;
            attributes.Add($"Tier: {relic.tier}");
        }

        if (inventoryMode == InventoryMode.Normal)
        {
            if (ItemData.isUsable) // check whether class is consumableitem instead?
                hints.AddRange(new List<TooltipHintType>() { TooltipHintType.Use, TooltipHintType.UseAll });
            hints.AddRange(new List<TooltipHintType>() { TooltipHintType.Drop, TooltipHintType.DropAll });
        }
        else if (inventoryMode == InventoryMode.Apply || inventoryMode == InventoryMode.Revert)
        {
            if (BuySellPanel.Instance.GetBuySellType() == BuySellType.Buy)
                attributes.Add($"Buy Price: {ItemData.buyPrice}");
            else
                attributes.Add($"Sell Price: {ItemData.sellPrice}");

            hints.Add(TooltipHintType.Transfer);
            hints.Add(TooltipHintType.TransferAll);
        }

        Tooltip.Instance.ShowTooltip(m_Item.itemName, m_Item.itemType.ToString(), m_Item.itemDescription, attributes.ToArray(), hints.ToArray());

        if (inventoryMode == InventoryMode.Normal)
        {
            if (m_Item && m_Item.isUsable)
                hint.SetActive(true);
        }
        else if (inventoryMode == InventoryMode.Apply || inventoryMode == InventoryMode.Revert || inventoryMode == InventoryMode.Throw) // transfer
        {
            hint.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (m_Item == null)
            return;

        Tooltip.Instance.HideTooltip();
        hint.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData) // rewrite this with switch case? it looks messy
    {
        if (m_Item == null)
            return;

        int tempStackSize = StackSize;
        if (inventoryMode == InventoryMode.Normal) // use and drop items
        {
            if (eventData.button == PointerEventData.InputButton.Left && m_Item.isUsable) // LMB: use item
            {
                if (ItemData.useSound)
                    AudioManager.Instance.PlaySound(ItemData.useSound);

                if (Input.GetKey(KeyCode.LeftShift)) // use all
                {
                    for (int i = 0; i < tempStackSize; i++)
                        UseItem();
                }
                else // use one
                    UseItem();
            }
            else if (eventData.button == PointerEventData.InputButton.Right) // RMB: drop item
            {
                if (Input.GetKey(KeyCode.LeftShift)) // drop all
                {
                    for (int i = 0; i < tempStackSize; i++)
                        DropItem();
                }
                else // drop one
                    DropItem();
            }
        }
        else if (inventoryMode == InventoryMode.Apply) // apply
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                if (Input.GetKey(KeyCode.LeftShift)) // apply all
                {
                    for (int i = 0; i < StackSize; i++)
                        BuySellPanel.Instance.ApplyTransfer(ItemData);
                }
                else // apply one
                    BuySellPanel.Instance.ApplyTransfer(ItemData);
            }
        }
        else if (inventoryMode == InventoryMode.Revert) // revert
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                if (Input.GetKey(KeyCode.LeftShift)) // revert all
                {
                    for (int i = 0; i < StackSize; i++)
                        BuySellPanel.Instance.RevertTransfer(ItemData);
                }
                else // revert one
                    BuySellPanel.Instance.RevertTransfer(ItemData);
            }
        }
        else if (inventoryMode == InventoryMode.Throw)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
                BuySellPanel.Instance.ThrowItem(ItemData);
        }
    }
}
