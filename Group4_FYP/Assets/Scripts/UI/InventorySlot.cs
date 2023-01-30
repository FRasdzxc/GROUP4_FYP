using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private InventorySlotType inventorySlotType; // for weapon/armor/ability inventory slots that only accept one type of item?
    [SerializeField] private Image image;
    [SerializeField] private Text stackSizeText;

    private ItemData item; // change to use list/array instead to support stacked items?
    private int stackSize;

    // Start is called before the first frame update
    void Start()
    {
        item = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddItem(ItemData item)
    {
        image.enabled = true;
        image.sprite = item.itemIcon;

        this.item = item;

        // if item != null && this.item == item, stack items?
        // else return? or swap item to mouse?
    }

    public void RemoveItem()
    {
        image.enabled = false;
        image.sprite = null;
        stackSizeText = null;

        item = null;

        // get item to follow mouse, remove item from this.item
    }

    public ItemData GetItem()
    {
        return item;
    }
}
