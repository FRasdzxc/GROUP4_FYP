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

    [SerializeField] private GameObject inventorySlotPrefab;
    [SerializeField] private GameItems gameItems;
    [SerializeField] private bool panelOverridable;

    private CanvasGroup buySellPanelCanvasGroup;
    private RectTransform buySellPanelRectTransform;
    private bool isOpened;

    private List<GameObject> inventorySlots;

    private UnityAction confirmAction;
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
                ShowBuySellPanel(BuySellType.Sell);
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
            Inventory.Instance.RefreshInventoryPanel(rightPanelContentPanel, InventoryMode.Transfer);
            leftPanelTitle.text = "Items";
            rightPanelTitle.text = "Buying";
            confirmAction = Buy;
            confirmButtonText.text = "Buy";
        }
        else // sell items
        {
            // SetupInventory(true);
            Inventory.Instance.RefreshInventoryPanel(leftPanelContentPanel, InventoryMode.Transfer);
            leftPanelTitle.text = "Inventory";
            rightPanelTitle.text = "Selling";
            confirmAction = Sell;
            confirmButtonText.text = "Sell";
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

    // private void SetupInventory(bool isLeftPanel)
    // {
    //     List<InventoryEntry> items = Inventory.Instance.GetItems();

    //     // destroy all the inventory slots
    //     for (int i = 0; i < inventorySlots.Count; i++)
    //     {
    //         Destroy(inventorySlots[i]);
    //     }
    //     inventorySlots.Clear();

    //     // add inventory slots
    //     for (int i = 0; i < items.Count; i++)
    //     {
    //         if (isLeftPanel)
    //         {
    //             inventorySlots.Add(Instantiate(inventorySlotPrefab, leftPanelContentPanel));
    //         }
    //         else
    //         {
    //             inventorySlots.Add(Instantiate(inventorySlotPrefab, rightPanelContentPanel));
    //         }

    //        inventorySlots[i].GetComponent<InventorySlot>().Configure(items[i].itemData, items[i].qty);
    //     }
    // }

    public void TransferItem(ItemData item/*, BuySellType buySellType*/)
    {
        Debug.Log("BuySellPanel: TransferItem()");

        transferredItems.Add(item);

        // check buyselltype to determine which panel to transfer to
    }

    public void Confirm()
    {
        confirmAction.Invoke();
    }

    

    public void Buy()
    {
        // get items prices
        int price = 0;

        ConfirmationPanel.Instance.ShowConfirmationPanel("Buy Items", "Do you want to buy these items?" + "\n\nCost: " + price.ToString("n0") + " coins",
            () =>
            {
                // buy action here
            });
    }

    public void Sell()
    {
        // get items prices
        int price = 0;

        ConfirmationPanel.Instance.ShowConfirmationPanel("Sell Items", "Do you want to sell these items?" + "\n\nGain: " + price.ToString("n0") + " coins",
            () =>
            {
                // sell action here
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
