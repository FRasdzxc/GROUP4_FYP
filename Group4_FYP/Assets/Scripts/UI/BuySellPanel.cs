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
    private List<ItemData> transferredItems;

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
        transferredItems = new List<ItemData>();
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

        if (buySellType == BuySellType.Buy) // buy items
        {
            // Inventory.Instance.RefreshInventoryPanel(rightPanelContentPanel, InventoryMode.Transfer);
            RefreshInventory(false, InventoryMode.Preview);
            RefreshShop();
            leftPanelTitle.text = "Shop";
            rightPanelTitle.text = "Inventory";
            confirmAction = Buy;
            confirmButtonText.text = "Buy";
            applyAllButton.SetActive(true);
            revertAllButton.SetActive(false);
        }
        else // sell items
        {
            // Inventory.Instance.RefreshInventoryPanel(leftPanelContentPanel, InventoryMode.Transfer);
            RefreshInventory(true, InventoryMode.Transfer);
            leftPanelTitle.text = "Inventory";
            rightPanelTitle.text = "Selling";
            confirmAction = Sell;
            confirmButtonText.text = "Sell";
            applyAllButton.SetActive(true);
            revertAllButton.SetActive(true);
        }

        buySellPanel.SetActive(true);

        buySellPanelRectTransform.DOAnchorPosY(0, 0.25f).SetEase(Ease.OutQuart);
        await buySellPanelCanvasGroup.DOFade(1, 0.25f).SetEase(Ease.OutQuart).AsyncWaitForCompletion();

        isOpened = true;
    }

    public async void HideBuySellPanel()
    {
        transferredItems.Clear();

        buySellPanelRectTransform.DOAnchorPosY(-buySellPanelRectTransform.rect.height / 4, 0.25f).SetEase(Ease.OutQuart);
        await buySellPanelCanvasGroup.DOFade(0, 0.25f).SetEase(Ease.OutQuart).AsyncWaitForCompletion();

        buySellPanel.SetActive(false);

        isOpened = false;
    }

    private void RefreshInventory(bool isLeftPanel, InventoryMode inventoryMode)
    {
        tempItems = Inventory.Instance.GetItems();

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

    private void RefreshShop()
    {
        for (int i = 0; i < shopSlots.Count; i++)
        {
            Destroy(shopSlots[i]);
        }
        shopSlots.Clear();

        for (int i = 0; i < gameItems.itemList.Length; i++)
        {
            shopSlots.Add(Instantiate(inventorySlotPrefab, leftPanelContentPanel));

            shopSlots[i].GetComponent<InventorySlot>().Configure(gameItems.itemList[i], 1, InventoryMode.Transfer);
        }
    }

    private void RefreshTransferredSlots()
    {
        for (int i = 0; i < transferredSlots.Count; i++)
        {
            Destroy(transferredSlots[i]);
        }
        transferredSlots.Clear();

        for (int i = 0; i < transferredSlots.Count; i++)
        {
            transferredSlots.Add(Instantiate(inventorySlotPrefab, rightPanelContentPanel));

            transferredSlots[i].GetComponent<InventorySlot>().Configure(transferredItems[i], 1, InventoryMode.Transfer);
        }
    }

    public void TransferItem(ItemData item)
    {
        Debug.Log("BuySellPanel: TransferItem()");

        transferredItems.Add(item);

        // check buyselltype to determine which panel to transfer to
        RefreshTransferredSlots();
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
            _ = Notification.Instance.ShowNotification("You are not buying any items");
            return;
        }

        // get items prices
        int price = 0;
        // loop through transferred items to get price

        ConfirmationPanel.Instance.ShowConfirmationPanel("Buy Items", "Do you want to buy these items?" + "\n\nCost: " + price.ToString("n0") + " coins",
            () =>
            {
                // buy action here

                // if coins < price, shownotification, return
                // deduct coins by price
                // inventory.setitems(tempitems)
            });
    }

    public void Sell()
    {
        if (!transferredItems.Any())
        {
            _ = Notification.Instance.ShowNotification("You are not selling any items");
            return;
        }

        // get items prices
        int price = 0;
        // loop through transferred items to get price

        ConfirmationPanel.Instance.ShowConfirmationPanel("Sell Items", "Do you want to sell these items?" + "\n\nGain: " + price.ToString("n0") + " coins",
            () =>
            {
                // sell action here

                // loop: inventory.removeitem(tempitems[index])
                // add coins by price
            });
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
}
