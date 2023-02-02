using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class NotificationPanel : MonoBehaviour
{
    [SerializeField] private Text notificationText;
    [SerializeField] private Button hideButton;

    // Start is called before the first frame update
    void Start()
    {
        hideButton.onClick.AddListener(() => { _ = HideNotificationPanel(); });
    }

    public async Task ShowNotificationPanel(string notification, float duration)
    {
        gameObject.GetComponent<CanvasGroup>().alpha = 0;
        notificationText.text = notification;

        gameObject.SetActive(true);
        await gameObject.GetComponent<CanvasGroup>().DOFade(1, 0.25f).SetEase(Ease.OutQuart).AsyncWaitForCompletion();

        await Task.Delay((int)(duration * 1000));
        await HideNotificationPanel();
    }

    public async Task HideNotificationPanel()
    {
        await gameObject.GetComponent<CanvasGroup>().DOFade(0, 0.25f).SetEase(Ease.OutQuart).AsyncWaitForCompletion();
        gameObject.SetActive(false);
    }
}
