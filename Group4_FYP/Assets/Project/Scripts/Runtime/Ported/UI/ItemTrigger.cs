using System.Collections;
using UnityEngine;

public class ItemTrigger : Interaction
{
    [SerializeField]
    private ItemData item;
    [Tooltip("Destroy after seconds")] [SerializeField]
    private float lifeTime = 300f; // default: 300 seconds (5 mins)

    protected override void Start()
    {
        base.Start();
        StartCoroutine(TimeoutDestroy(Time.time + lifeTime));
    }

    protected override void Interact() // previously: public void PickUpItem()
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

    private IEnumerator TimeoutDestroy(float lifeTime)
    {
        while (Time.time < lifeTime)
            yield return null;

        Destroy(gameObject);
    }

    // private void OnTriggerEnter2D(Collider2D collision)
    // {
    //     if (collision.CompareTag("Player"))
    //     {
    //         PickUpItem();
    //     }
    // }
}
