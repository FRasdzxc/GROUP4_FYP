using System.Threading.Tasks;
using UnityEngine;

public class ItemTrigger : MonoBehaviour
{
    [SerializeField]
    private ItemData item;
    [Tooltip("Destroy after seconds")] [SerializeField]
    private float lifeTime = 300f; // default: 300 seconds (5 mins)

    // Start is called before the first frame update
    void Start()
    {
        DestroyGobj();
    }

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

    private async void DestroyGobj()
    {
        float interval = 0f;

        while (interval < lifeTime)
        {
            interval += Time.deltaTime;
            await Task.Yield();
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PickUpItem();
        }
    }
}
