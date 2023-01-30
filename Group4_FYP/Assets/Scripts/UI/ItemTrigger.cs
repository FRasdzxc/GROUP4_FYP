using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTrigger : MonoBehaviour
{
    [SerializeField] private ItemData item;

    public void PickUpItem()
    {
        int random = Random.Range(item.minDropSize, item.maxDropSize);
        int addedItemCount = 0;

        for (int i = 0; i < random; i++)
        {
            if (Inventory.Instance.AddItem(item))
            {
                addedItemCount++;
            }
            else
            {
                break;
            }
        }

        if (addedItemCount != 0)
        {
            Message.Instance.ShowMessage("+" + addedItemCount + " " + item.itemName, item.itemIcon);
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
