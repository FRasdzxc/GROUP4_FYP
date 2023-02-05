using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HeroPanel : MonoBehaviour
{
    [SerializeField] private GameObject hudMainPanel;
    [SerializeField] private GameObject heroPanel;
    [SerializeField] private Text heroNameText;
    [SerializeField] private Text heroLevelText;
    [SerializeField] private Text coinText;

    private bool isOpened;
    private RectTransform heroPanelRectTransform;

    private static HeroPanel instance;
    public static HeroPanel Instance
    {
        get
        {
            return instance;
        }
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
        isOpened = false;

        heroPanel.GetComponent<CanvasGroup>().alpha = 0;
        heroPanelRectTransform = heroPanel.GetComponent<RectTransform>();
        heroPanelRectTransform.anchoredPosition = new Vector2(0, heroPanelRectTransform.sizeDelta.y / 2);
        heroPanel.SetActive(false);

        heroNameText.text = ProfileManagerJson.GetProfile(PlayerPrefs.GetString("selectedProfileName")).profileName;
    }

    // Update is called once per frame
    async void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (isOpened)
            {
                await HideHeroPanel();
            }
            else
            {
                await ShowHeroPanel();
            }

            isOpened = !isOpened;
        }
    }

    public void UpdateCoin(int coin)
    {
        coinText.text = coin.ToString("n0");
    }

    public void UpdateLevel(int level)
    {
        heroLevelText.text = "Level " + level.ToString("n0");
    }

    private async Task ShowHeroPanel()
    {
        Inventory.Instance.RefreshInventoryPanel();

        heroPanel.SetActive(true);

        hudMainPanel.GetComponent<CanvasGroup>().DOFade(0, 0.25f).SetEase(Ease.OutQuart);
        heroPanelRectTransform.DOAnchorPosY(0, 0.25f).SetEase(Ease.OutQuart);
        await heroPanel.GetComponent<CanvasGroup>().DOFade(1, 0.25f).SetEase(Ease.OutQuart).AsyncWaitForCompletion();

        hudMainPanel.SetActive(false);
    }

    private async Task HideHeroPanel()
    {
        hudMainPanel.SetActive(true);

        hudMainPanel.GetComponent<CanvasGroup>().DOFade(1, 0.25f).SetEase(Ease.OutQuart);
        heroPanelRectTransform.DOAnchorPosY(heroPanelRectTransform.sizeDelta.y / 2, 0.25f).SetEase(Ease.OutQuart);
        await heroPanel.GetComponent<CanvasGroup>().DOFade(0, 0.25f).SetEase(Ease.OutQuart).AsyncWaitForCompletion();

        heroPanel.SetActive(false);
    }
}
