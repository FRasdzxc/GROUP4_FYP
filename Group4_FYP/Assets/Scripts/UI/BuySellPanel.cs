using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BuySellPanel : MonoBehaviour
{
    [SerializeField] private GameObject buySellPanel;
    [SerializeField] private Text leftPanelTitle;
    [SerializeField] private Text rightPanelTitle;
    [SerializeField] private GameObject leftPanelContentPanel;
    [SerializeField] private GameObject rightPanelContentPanel;

    [SerializeField] private GameObject inventorySlotPrefab;
    [SerializeField] private GameItems gameItems;

    private CanvasGroup buySellPanelCanvasGroup;
    private RectTransform buySellPanelRectTransform;

    private List<GameObject> inventorySlots;

    // Start is called before the first frame update
    void Start()
    {
        buySellPanelCanvasGroup = buySellPanel.GetComponent<CanvasGroup>();
        buySellPanelRectTransform = buySellPanel.GetComponent<RectTransform>();

        buySellPanelRectTransform.anchoredPosition = new Vector2(0, buySellPanelRectTransform.sizeDelta.y / 2);
        buySellPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // test only
        if (Input.GetKeyDown(KeyCode.R))
        {
            ShowBuySellPanel(BuySellType.Sell);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            HideBuySellPanel();
        }
    }

    

    public async void ShowBuySellPanel(BuySellType buySellType)
    {
        if (buySellType == BuySellType.Buy) // buy items
        {

        }
        else // sell items
        {
            SetupInventory();
        }

        buySellPanel.SetActive(true);

        buySellPanelRectTransform.DOAnchorPosY(0, 0.25f).SetEase(Ease.OutQuart);
        await buySellPanelCanvasGroup.DOFade(1, 0.25f).SetEase(Ease.OutQuart).AsyncWaitForCompletion();
    }

    private async void HideBuySellPanel()
    {
        buySellPanelRectTransform.DOAnchorPosY(buySellPanelRectTransform.sizeDelta.y / 2, 0.25f).SetEase(Ease.OutQuart);
        await buySellPanelCanvasGroup.DOFade(0, 0.25f).SetEase(Ease.OutQuart).AsyncWaitForCompletion();

        buySellPanel.SetActive(false);
    }

    private void SetupInventory()
    {
        // destroy all the inventory slots
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            Destroy(inventorySlots[i]);
        }
        inventorySlots.Clear();

        //foreach ()
        //{
        //    inventorySlots.Add(Instantiate(inventorySlotPrefab, leftPanelContentPanel.transform));
        //    inventorySlots[i].GetComponent<InventorySlot>().Configure(items[i].itemData, items[i].qty);
        //}
    }
}
