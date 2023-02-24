using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;

public class BuySellPanel : MonoBehaviour, IPanelConflictable
{
    [SerializeField] private GameObject buySellPanel;
    [SerializeField] private Text leftPanelTitle;
    [SerializeField] private Text rightPanelTitle;
    [SerializeField] private Transform leftPanelContentPanel;
    [SerializeField] private Transform rightPanelContentPanel;
    [SerializeField] private Text confirmButtonText;
    [SerializeField] private GameObject applyAllButton;
    [SerializeField] private GameObject revertAllButton;
    [SerializeField] private Text titleText;
    [SerializeField] private Text coinText;
    [SerializeField] private Text totalText;

    [SerializeField] private GameObject inventorySlotPrefab;
    [SerializeField] private GameItems gameItems;
    [SerializeField] private bool panelOverridable;

    private CanvasGroup buySellPanelCanvasGroup;
    private RectTransform buySellPanelRectTransform;
    private bool isOpened;

    private List<GameObject> inventorySlots;
    private List<GameObject> shopSlots;
    private List<GameObject> transferredSlots;

    private UnityAction confirmAction;
    private List<InventoryEntry> tempItems;
    private List<InventoryEntry> transferredItems;

    private BuySellType buySellType;

    private static BuySellPanel instance;
    public static BuySellPanel Instance
    {
        get => instance;
    }

    void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        buySellPanelCanvasGroup = buySellPanel.GetComponent<CanvasGroup>();
        buySellPanelRectTransform = buySellPanel.GetComponent<RectTransform>();

        buySellPanelCanvasGroup.alpha = 0;
        buySellPanelRectTransform.anchoredPosition = new Vector2(0, -buySellPanelRectTransform.rect.height / 4);
        buySellPanel.SetActive(false);

        inventorySlots = new List<GameObject>();
        shopSlots = new List<GameObject>();
        transferredSlots = new List<GameObject>();
        tempItems = new List<InventoryEntry>();
        transferredItems = new List<InventoryEntry>();
    }

    // Update is called once per frame
    void Update()
    {
        // test only
        if (Input.GetKeyDown(KeyCode.R))
        // if (Input.GetKeyDown(KeyCode.R) && InputManager.Instance.GetKeyDown(KeyCode.R))
        {
            if (!isOpened)
            {
                ShowBuySellPanel(BuySellType.Buy);
            }
            else
            {
                HideBuySellPanel();
            }

            // isOpened = !isOpened;
        }
    }

    public void ShowBuyPanel()
    {
        ShowBuySellPanel(BuySellType.Buy);
    }

    public void ShowSellPanel()
    {
        ShowBuySellPanel(BuySellType.Sell);
    }

    public async void ShowBuySellPanel(BuySellType buySellType)
    {
        if (!HideConflictingPanels())
        {
            return;
        }

        ResetPanels();
        this.buySellType = buySellType;
        coinText.text = Hero.Instance.GetStoredCoin().ToString("n0");

        tempItems = new List<InventoryEntry>(Inventory.Instance.GetItems()); // new List<>() since list is reference type?
        tempItems.AddRange(transferredItems); // used for previewing inventory-after-buy when buying items

        if (buySellType == BuySellType.Buy) // buy items
        {
            // Inventory.Instance.RefreshInventoryPanel(rightPanelContentPanel, InventoryMode.Transfer);
            RefreshInventoryPanel(false, InventoryMode.Preview);
            RefreshShopPanel();

            titleText.text = "Buy Items";
            leftPanelTitle.text = "Shop";
            rightPanelTitle.text = "Inventory";
            confirmAction = Buy;
            confirmButtonText.text = "Buy";
            applyAllButton.SetActive(true);
            revertAllButton.SetActive(false);
            /* 
                impossible to revert manually:
                transferred items might be added to existing slots by GetFreeEntry();
                if not, inventory might be full even though it isn't
            */
        }
        else // sell items
        {
            // Inventory.Instance.RefreshInventoryPanel(leftPanelContentPanel, InventoryMode.Transfer);
            RefreshInventoryPanel(true, InventoryMode.Apply);
            RefreshTransferredSlots();

            titleText.text = "Sell Items";
            leftPanelTitle.text = "Inventory";
            rightPanelTitle.text = "Selling";
            confirmAction = Sell;
            confirmButtonText.text = "Sell";
            applyAllButton.SetActive(true);
            revertAllButton.SetActive(true);
        }
        RefreshTotal();

        HUD.Instance.HideHUDMain();
        buySellPanel.SetActive(true);
        buySellPanelRectTransform.DOAnchorPosY(0, 0.25f).SetEase(Ease.OutQuart);
        await buySellPanelCanvasGroup.DOFade(1, 0.25f).SetEase(Ease.OutQuart).AsyncWaitForCompletion();

        isOpened = true;
    }

    public async void HideBuySellPanel()
    {
        transferredItems.Clear();

        HUD.Instance.ShowHUDMain();
        buySellPanelRectTransform.DOAnchorPosY(-buySellPanelRectTransform.rect.height / 4, 0.25f).SetEase(Ease.OutQuart);
        await buySellPanelCanvasGroup.DOFade(0, 0.25f).SetEase(Ease.OutQuart).AsyncWaitForCompletion();
        buySellPanel.SetActive(false);

        isOpened = false;
    }

    private void RefreshInventoryPanel(bool isLeftPanel, InventoryMode inventoryMode)
    {
        // destroy all the inventory slots
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            Destroy(inventorySlots[i]);
        }
        inventorySlots.Clear();

        // add inventory slots
        for (int i = 0; i < Inventory.Instance.GetInventorySize(); i++)
        {
            if (isLeftPanel)
            {
                inventorySlots.Add(Instantiate(inventorySlotPrefab, leftPanelContentPanel));
            }
            else
            {
                inventorySlots.Add(Instantiate(inventorySlotPrefab, rightPanelContentPanel));
            }
        }

        // assign items to inventory slot
        for (int i = 0; i < tempItems.Count; i++)
        {
            inventorySlots[i].GetComponent<InventorySlot>().Configure(tempItems[i].itemData, tempItems[i].qty, inventoryMode);
        }
    }

    private void RefreshShopPanel()
    {
        for (int i = 0; i < shopSlots.Count; i++)
        {
            Destroy(shopSlots[i]);
        }
        shopSlots.Clear();

        // for (int i = 0; i < gameItems.itemList.Length; i++)
        // {
        //     if (gameItems.itemList[i].isBuyable)
        //     {
        //         shopSlots.Add(Instantiate(inventorySlotPrefab, leftPanelContentPanel));
        //         shopSlots[i].GetComponent<InventorySlot>().Configure(gameItems.itemList[i], 1, InventoryMode.Apply);
        //     }
        // }

        int currentSlot = 0;
        foreach (ItemData item in gameItems.itemList)
        {
            if (item.isBuyable)
            {
                shopSlots.Add(Instantiate(inventorySlotPrefab, leftPanelContentPanel));
                shopSlots[currentSlot++].GetComponent<InventorySlot>().Configure(item, 1, InventoryMode.Apply);
            }
        }
    }

    private void RefreshTransferredSlots()
    {
        for (int i = 0; i < transferredSlots.Count; i++)
        {
            Destroy(transferredSlots[i]);
        }
        transferredSlots.Clear();

        for (int i = 0; i < transferredItems.Count; i++)
        {
            transferredSlots.Add(Instantiate(inventorySlotPrefab, rightPanelContentPanel));
            transferredSlots[i].GetComponent<InventorySlot>().Configure(transferredItems[i].itemData, transferredItems[i].qty, InventoryMode.Revert);
        }
    }

    private void RefreshTotal()
    {
        // get items prices
        int price = 0;
        // loop through transferred items to get price
        foreach (InventoryEntry ie in transferredItems)
        {
            for (int i = 0; i < ie.qty; i++)
            {
                if (buySellType == BuySellType.Buy)
                {
                    price += ie.itemData.buyPrice;
                }
                else
                {
                    price += ie.itemData.sellPrice;
                }
            }
        }
        totalText.text = price.ToString("n0");
    }

    private bool RemoveItem(ItemData item, List<InventoryEntry> list)
    {
        for (int i = list.Count - 1; i >= 0; i--)
        {
            InventoryEntry entry = list[i];
            if (entry.itemData != item)
                continue;

            int newQty = Mathf.Clamp(entry.qty - 1, 0, InventorySlot.kMaxStackSize);
            if (newQty == 0)
                list.RemoveAt(i);
            else
                list[i] = new InventoryEntry(item, newQty);
            
            return true;
        }

        return false;
    }

    int GetFreeEntry(ItemData item, List<InventoryEntry> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].itemData == item && list[i].qty < InventorySlot.kMaxStackSize)
                return i;
        }

        return -1;
    }

    public void ApplyTransfer(ItemData item)
    {
        Debug.Log("BuySellPanel: ApplyTransfer()");

        if (buySellType == BuySellType.Buy)
        {
            if (tempItems.Count > Inventory.Instance.GetInventorySize()) // rewrite: getfreeentry first
            {
                _ = Notification.Instance.ShowNotification("Inventory will be full!");
                return;
            }

            int slot = GetFreeEntry(item, tempItems);
            if (slot == -1)
                tempItems.Add(new InventoryEntry(item));
            else
            {
                InventoryEntry entry = tempItems[slot];
                int newQty = Mathf.Clamp(entry.qty + 1, 1, InventorySlot.kMaxStackSize);
                tempItems[slot] = new InventoryEntry(item, newQty);
            }
            transferredItems.Add(new InventoryEntry(item));

            RefreshInventoryPanel(false, InventoryMode.Preview);
        }
        else
        {
            // find item in tempitems; if not, return with error
            if (!RemoveItem(item, tempItems))
            {
                Debug.LogError($"{this.GetType().Name}.ApplyTransfer(): {item.itemName} not found in tempItems");
                return;
            }

            // getfreeentry and add to transferreditems
            int slot = GetFreeEntry(item, transferredItems);
            if (slot == -1)
                transferredItems.Add(new InventoryEntry(item));
            else
            {
                InventoryEntry entry = transferredItems[slot];
                int newQty = Mathf.Clamp(entry.qty + 1, 1, InventorySlot.kMaxStackSize);
                transferredItems[slot] = new InventoryEntry(item, newQty);
            }

            RefreshTransferredSlots();
            RefreshInventoryPanel(true, InventoryMode.Apply);
        }

        RefreshTotal();
    }

    public void RevertTransfer(ItemData item)
    {
        Debug.Log("BuySellPanel: RevertTransfer()");

        // find item in transferreditems; if not, return with error
        if (!RemoveItem(item, transferredItems))
        {
            Debug.LogError($"{this.GetType().Name}.RevertTransfer(): {item.itemName} not found in transferredItems");
            return;
        }

        // getfreeentry and add to tempitems
        int slot = GetFreeEntry(item, tempItems);
        if (slot == -1)
            tempItems.Add(new InventoryEntry(item));
        else
        {
            InventoryEntry entry = tempItems[slot];
            int newQty = Mathf.Clamp(entry.qty + 1, 1, InventorySlot.kMaxStackSize);
            tempItems[slot] = new InventoryEntry(item, newQty);
        }

        // refresh
        RefreshTransferredSlots();
        RefreshInventoryPanel(true, InventoryMode.Apply);   // sell only; not accounting for buy since reverting isn't support when buying atm
        RefreshTotal();
    }

    public void ApplyAll()
    {
        // check buySellType first
        // if Buy, ApplyTransfer() all items from gameItems with a loop, check if inventory is full too
        // if Sell, ApplyTransfer() all items from tempItems with a loop
        if (buySellType == BuySellType.Buy)
        {
            foreach (ItemData item in gameItems.itemList)
            {
                ApplyTransfer(item);
            }
        }
        else
        {
            while (tempItems.Any()) // this works, but very slow
            {
                for (int i = 0; i < tempItems[0].qty; i++)
                {
                    ApplyTransfer(tempItems[0].itemData);
                }
            }
        }
    }

    public void RevertAll()
    {
        // revert and RevertTransfer() all items from transferredItems with a loop
        while (transferredItems.Any())
        {
            for (int i = 0; i < transferredItems[0].qty; i++)
            {
                RevertTransfer(transferredItems[0].itemData);
            }
        }
    }

    public void ThrowItem(ItemData item)
    {
        Debug.Log("BuySellPanel: ThrowItem()");

        // check if item exists, then remove item
        // reference Inventory.RemoveItem()
    }

    public void Confirm()
    {
        confirmAction.Invoke();
    }

    public void Buy()
    {
        if (!transferredItems.Any())
        {
            _ = Notification.Instance.ShowNotification("You are not buying any item");
            return;
        }

        // get items prices
        int price = 0;
        // loop through transferred items to get price
        foreach (InventoryEntry ie in transferredItems)
        {
            for (int i = 0; i < ie.qty; i++)
            {
                price += ie.itemData.buyPrice;
            }
        }

        ConfirmationPanel.Instance.ShowConfirmationPanel("Buy Items", "Do you want to buy these items?" + "\n\nCost: " + price.ToString("n0") + " coins",
            () =>
            {
                // buy action here

                // if coins < price, shownotification, return
                if (Hero.Instance.GetStoredCoin() >= price)
                {
                    // deduct coins by price
                    // inventory.setitems(tempitems)
                    Hero.Instance.AddCoin(-price);
                    Inventory.Instance.SetItems(tempItems);

                    HideBuySellPanel();
                    _ = Notification.Instance.ShowNotification("Successfully bought items");
                }
                else
                {
                    _ = Notification.Instance.ShowNotification("Insufficient amount of coins");
                }
            }, true);
    }

    public void Sell()
    {
        if (!transferredItems.Any())
        {
            _ = Notification.Instance.ShowNotification("You are not selling any item");
            return;
        }

        // get items prices
        int price = 0;
        // loop through transferred items to get price
        foreach (InventoryEntry ie in transferredItems)
        {
            for (int i = 0; i < ie.qty; i++)
            {
                price += ie.itemData.sellPrice;
            }
        }

        ConfirmationPanel.Instance.ShowConfirmationPanel("Sell Items", "Do you want to sell these items?" + "\n\nGain: " + price.ToString("n0") + " coins",
            () =>
            {
                // sell action here

                // loop: inventory.removeitem(tempitems[index])
                // add coins by price
                Inventory.Instance.SetItems(tempItems);
                Hero.Instance.AddCoin(price);

                HideBuySellPanel();
                _ = Notification.Instance.ShowNotification("Successfully sold items");
            }, true);
    }

    public BuySellType GetBuySellType()
    {
        return buySellType;
    }

    private void ResetPanels() // clean up left and right content panels by destroying all childs
    {
        foreach (Transform child in leftPanelContentPanel)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in rightPanelContentPanel)
        {
            Destroy(child.gameObject);
        }
    }

    public bool HideConflictingPanels()
    {
        if (!HeroPanel.Instance.IsPanelActive())
        {
            return true;
        }

        if (HeroPanel.Instance.IsPanelOverridable())
        {
            HeroPanel.Instance.HideHeroPanel();
            return true;
        }

        return false;
    }

    public bool IsPanelOverridable()
    {
        return panelOverridable;
    }

    public bool IsPanelActive()
    {
        return buySellPanel.activeSelf;
    }
}
