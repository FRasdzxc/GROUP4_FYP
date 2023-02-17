using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BuySellPanel : MonoBehaviour, IPanelConflictable
{
    [SerializeField] private GameObject buySellPanel;
    [SerializeField] private Text leftPanelTitle;
    [SerializeField] private Text rightPanelTitle;
    [SerializeField] private Transform leftPanelContentPanel;
    [SerializeField] private Transform rightPanelContentPanel;

    [SerializeField] private GameObject inventorySlotPrefab;
    [SerializeField] private GameItems gameItems;
    [SerializeField] private bool panelOverridable;

    private CanvasGroup buySellPanelCanvasGroup;
    private RectTransform buySellPanelRectTransform;
    private bool isOpened;

    private List<GameObject> inventorySlots;

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

    public async void ShowBuySellPanel(BuySellType buySellType)
    {
        // if (!CloseConflictingPanels())
        // {
        //     return;
        // }

        ResetPanels();

        if (buySellType == BuySellType.Buy) // buy items
        {
            leftPanelTitle.text = "Items";
            rightPanelTitle.text = "Buying";
        }
        else // sell items
        {
            // SetupInventory(true);
            Inventory.Instance.RefreshInventoryPanel(leftPanelContentPanel, InventoryMode.Transfer);
            leftPanelTitle.text = "Inventory";
            rightPanelTitle.text = "Selling";
        }

        buySellPanel.SetActive(true);

        buySellPanelRectTransform.DOAnchorPosY(0, 0.25f).SetEase(Ease.OutQuart);
        await buySellPanelCanvasGroup.DOFade(1, 0.25f).SetEase(Ease.OutQuart).AsyncWaitForCompletion();

        isOpened = true;
    }

    public async void HideBuySellPanel()
    {
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

    public bool CloseConflictingPanels()
    {
        if (HeroPanel.Instance.IsPanelOverridable())
        {
            _ = HeroPanel.Instance.HideHeroPanel();
            return true;
        }

        return false;
    }

    public bool IsPanelOverridable()
    {
        return panelOverridable;
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
