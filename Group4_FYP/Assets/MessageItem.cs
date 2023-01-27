using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MessageItem : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Text messageText;
    [SerializeField] private Button hideButton;

    // Start is called before the first frame update
    void Start()
    {
        hideButton.onClick.AddListener(() => { HideMessageItem(); });
    }

    public async void ShowMessageItem(string message, float duration, Sprite sprite)
    {
        gameObject.GetComponent<CanvasGroup>().alpha = 0;
        messageText.text = message;
        image.sprite = sprite;

        gameObject.SetActive(true);
        await gameObject.GetComponent<CanvasGroup>().DOFade(1, 0.25f).SetEase(Ease.OutQuart).AsyncWaitForCompletion();

        await Task.Delay((int)(duration * 1000));
        HideMessageItem();
    }

    public async void HideMessageItem()
    {
        await gameObject.GetComponent<CanvasGroup>().DOFade(0, 0.25f).SetEase(Ease.OutQuart).AsyncWaitForCompletion();
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
