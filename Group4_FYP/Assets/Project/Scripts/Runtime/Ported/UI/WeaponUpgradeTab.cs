using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PathOfHero.Others;

public class WeaponUpgradeTab : Tab
{
    [SerializeField] private GameWeapons gameWeapons;
    [SerializeField] private InventorySlot weaponSlot;
    [SerializeField] private InventorySlot runicSlot;
    [SerializeField] private InventorySlot resultSlot;
    [SerializeField] private Image equalsImage;
    [SerializeField] private Sprite equalsSprite;
    [SerializeField] private Sprite notEqualsSprite;
    [SerializeField] private Text warningText;
    [SerializeField] private AudioClip weaponUpgradeSound;

    private List<InventoryEntry> tempItems;
    private bool upgradable;
    private int price;
    private string weaponId;
    private int targetTier;

    protected override void PrepareTab()
    {
        // prepare tab here
        ResetTab();
        tempItems = Inventory.Instance.GetItems();
        upgradable = false;

        weaponId = WeaponManager.Instance.GetWeaponId();
        targetTier = WeaponManager.Instance.GetWeaponTier() + 1;

        weaponSlot.Configure(WeaponManager.Instance.GetWeapon().item, 1, InventoryMode.Preview);

        // check if weapon is of the highest tier
        foreach (ClassWeaponEntry cwe in gameWeapons.weaponList)
        {
            if (cwe.heroClass == WeaponManager.Instance.GetHeroClass())
            {
                foreach (WeaponEntry we in cwe.classWeapons)
                {
                    if (weaponId == we.weaponId)
                    {
                        // if (targetTier < cwe.classWeapons.Length)
                        if (targetTier < we.weaponTiers.Length)
                        {
                            // if weapon is not at the highest tier, find if tempItems contains runic for weapontier + 1
                            if (FindRunic())
                            {
                                warningText.gameObject.SetActive(true);
                                warningText.text = $"Your weapon is upgradable!";
                                upgradable = true;
                            }
                            else
                            {
                                warningText.gameObject.SetActive(true);
                                warningText.text = String.Format("You do not have the required Runic - \"Runic (Tier {0})\" for this upgrade", targetTier);
                                equalsImage.sprite = notEqualsSprite;
                            }
                            resultSlot.Configure(we.weaponTiers[targetTier].item, 1, InventoryMode.Preview);
                        }
                        else
                        {
                            warningText.gameObject.SetActive(true);
                            warningText.text = "Your weapon is of the highest tier already!";
                            equalsImage.sprite = notEqualsSprite;
                            runicSlot.Clear();
                            resultSlot.Clear();
                        }
                    }
                }
            }
        }
    }

    public void Upgrade()
    {
        // do checking
        if (upgradable)
        {
            ConfirmationPanel.Instance.ShowConfirmationPanel
            (
                "Upgrade Weapon",
                $"Do you want to upgrade this weapon to <color={CustomColorStrings.green}>Tier {targetTier}</color>?",
                () =>
                {
                    if (Hero.Instance.GetStoredCoin() >= price)
                    {
                        Hero.Instance.AddCoin(-price);
                        WeaponManager.Instance.UpgradeWeapon(targetTier);
                        Inventory.Instance.SetItems(tempItems);

                        PrepareTab();
                        _ = Notification.Instance.ShowNotificationAsync("Successfully upgraded weapon!");
                        AudioManager.Instance.PlaySound(weaponUpgradeSound);
                    }
                    else
                        _ = Notification.Instance.ShowNotificationAsync("Insufficient amount of coins");
                },
                false,
                $"<color={CustomColorStrings.yellow}>You have:</color> {Hero.Instance.GetStoredCoin().ToString("n0")} coins",
                $"<color={CustomColorStrings.yellow}>Cost:</color> {price.ToString("n0")} coins, Runic (Tier {targetTier})"
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

    private bool FindRunic()
    {
        foreach (InventoryEntry ie in tempItems)
        {
            if (ie.itemData is RunicItemData)
            {
                RunicItemData runicItem = ie.itemData as RunicItemData;
                if (runicItem.tier == targetTier)
                {
                    runicSlot.Configure(runicItem, 1, InventoryMode.Preview);
                    RemoveItem(runicItem);
                    price = runicItem.upgradePrice;
                    return true;
                }
            }
        }

        return false;
    }

    private void ResetTab()
    {
        weaponSlot.Clear();
        runicSlot.Clear();
        resultSlot.Clear();
        equalsImage.sprite = equalsSprite;
    }
}
