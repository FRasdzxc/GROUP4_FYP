using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

public class HeroPanel : MonoBehaviour
{
    [SerializeField] private GameObject hudPanel;
    [SerializeField] private GameObject heroPanel;

    private bool isOpened;
    private RectTransform heroPanelRectTransform;

    // Start is called before the first frame update
    void Start()
    {
        isOpened = false;

        heroPanel.GetComponent<CanvasGroup>().alpha = 0;
        heroPanelRectTransform = heroPanel.GetComponent<RectTransform>();
        heroPanelRectTransform.anchoredPosition = new Vector2(0, heroPanelRectTransform.sizeDelta.y / 2);
        heroPanel.SetActive(false);
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

    private async Task ShowHeroPanel()
    {
        heroPanel.SetActive(true);

        hudPanel.GetComponent<CanvasGroup>().DOFade(0, 0.25f).SetEase(Ease.OutQuart);
        heroPanelRectTransform.DOAnchorPosY(0, 0.25f).SetEase(Ease.OutQuart);
        await heroPanel.GetComponent<CanvasGroup>().DOFade(1, 0.25f).SetEase(Ease.OutQuart).AsyncWaitForCompletion();

        hudPanel.SetActive(false);
    }

    private async Task HideHeroPanel()
    {
        hudPanel.SetActive(true);

        hudPanel.GetComponent<CanvasGroup>().DOFade(1, 0.25f).SetEase(Ease.OutQuart);
        heroPanelRectTransform.DOAnchorPosY(heroPanelRectTransform.sizeDelta.y / 2, 0.25f).SetEase(Ease.OutQuart);
        await heroPanel.GetComponent<CanvasGroup>().DOFade(0, 0.25f).SetEase(Ease.OutQuart).AsyncWaitForCompletion();

        heroPanel.SetActive(false);
    }
}
