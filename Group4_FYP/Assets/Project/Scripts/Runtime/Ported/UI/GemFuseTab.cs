using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PathOfHero.Others;

public class GemFuseTab : Tab
{
    [SerializeField]
    private GemItemData gemItem;

    [SerializeField]
    private Transform fragmentSlotsContainer;

    [SerializeField]
    private GameObject fragmentSlotPrefab;

    [SerializeField]
    private InventorySlot resultSlot;

    [SerializeField]
    private Image equalsImage;

    [SerializeField]
    private Sprite equalsSprite;

    [SerializeField]
    private Sprite notEqualsSprite;

    [SerializeField]
    private Text warningText;

    [SerializeField]
    private AudioClip gemFuseSound;

    private List<InventoryEntry> tempItems;

    private bool fusible;

    protected override void PrepareTab()
    {
        // prepare tab
        ResetTab();
        tempItems = Inventory.Instance.GetItems();
        fusible = true;

        resultSlot.Configure(gemItem, 1, InventoryMode.Preview);

        // set up slots for the required fragments
        foreach (ItemData requiredFragment in gemItem.fragments)
        {
            if (!FindFragment(requiredFragment))
                fusible = false;
        }

        if (fusible)
        {
            warningText.text = $"Fusible! Click 'Fuse' to fuse";
            equalsImage.sprite = equalsSprite;
        }
        else
        {
            warningText.text = $"You do not have all the required Fragments";
            equalsImage.sprite = notEqualsSprite;
        }
    }

    public void Fuse()
    {
        if (Inventory.Instance.IsFull())
        {
            _ = Notification.Instance.ShowNotificationAsync("Your Inventory is full! Please leave at least one empty slot");
            return;
        }

        // do checking
        if (fusible)
        {
            ConfirmationPanel.Instance.ShowConfirmationPanel
            (
                "Fuse Fragments",
                $"Do you want to fuse Fragments and obtain a Gem?",
                () =>
                {
                    if (Hero.Instance.GetStoredCoin() >= gemItem.fusePrice)
                    {
                        Hero.Instance.AddCoin(-gemItem.fusePrice);
                        Inventory.Instance.SetItems(tempItems);
                        Inventory.Instance.AddItem(gemItem);

                        PrepareTab();
                        _ = Notification.Instance.ShowNotificationAsync("You've obtained a Gem!");
                        AudioManager.Instance.PlaySound(gemFuseSound);
                    }
                    else
                        _ = Notification.Instance.ShowNotificationAsync("Insufficient amount of coins");
                },
                false,
                $"<color={CustomColorStrings.yellow}>You have:</color> {Hero.Instance.GetStoredCoin().ToString("n0")} coins",
                $"<color={CustomColorStrings.yellow}>Cost:</color> {gemItem.fusePrice.ToString("n0")} coins, {gemItem.fragments.Length} Fragments"
            );
        }
    }

    private void RemoveItem(ItemData item)
    {
        for (int i = tempItems.Count - 1; i >= 0; i--)
        {
            InventoryEntry entry = tempItems[i];
            if (entry.itemData != item)
                continue;

            int newQty = Mathf.Clamp(entry.qty - 1, 0, InventorySlot.kMaxStackSize);
            if (newQty == 0)
                tempItems.RemoveAt(i);
            else
                tempItems[i] = new InventoryEntry(item, newQty);
            
            break;
        }
    }

    private bool FindFragment(ItemData fragment)
    {
        GameObject slotClone = Instantiate(fragmentSlotPrefab, fragmentSlotsContainer);

        foreach (InventoryEntry ie in tempItems)
        {
            if (ie.itemData.Equals(fragment))
            {
                slotClone.GetComponent<InventorySlot>().Configure(fragment, 1, InventoryMode.Preview);
                RemoveItem(fragment);
                return true;
            }
        }

        return false;
    }

    private void ResetTab()
    {
        foreach (Transform child in fragmentSlotsContainer)
            Destroy(child.gameObject);
        resultSlot.Clear();
        equalsImage.sprite = equalsSprite;
    }
}
