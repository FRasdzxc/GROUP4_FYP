using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTrigger : MonoBehaviour
{
    [SerializeField] private ItemData item;

    public void PickUpItem()
    {
        if (Inventory.Instance.AddItem(item))
        {
            // show message
            Message.Instance.ShowMessage("+1 " + item.itemName, item.itemIcon);

            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PickUpItem();
        }
    }
}
