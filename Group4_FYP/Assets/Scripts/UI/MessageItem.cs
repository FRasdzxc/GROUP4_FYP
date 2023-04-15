using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MessageItem : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Text messageText;
    [SerializeField] private Button hideButton;

    private bool m_Visible;
    private float m_HideTime;

    private void Awake()
        => hideButton.onClick.AddListener(() => { HideMessageItem(); });

    private void Update()
    {
        if (!m_Visible || Time.time < m_HideTime)
            return;

        StartCoroutine(AnimatedDestroy());
    }

    public void ShowMessageItem(string message, float duration, Sprite sprite)
    {
        messageText.text = message;
        image.sprite = sprite;

        gameObject.SetActive(true);
        StartCoroutine(AnimatedDisplay(duration));
    }

    public void HideMessageItem() => StartCoroutine(AnimatedDestroy());

    private IEnumerator AnimatedDisplay(float duration)
    {
        if (TryGetComponent<CanvasGroup>(out var canvasGroup))
        {
            canvasGroup.alpha = 0;
            yield return canvasGroup.DOFade(1, 0.25f).SetEase(Ease.OutQuart).WaitForCompletion();
        }

        m_HideTime = Time.time + duration;
        m_Visible = true;
    }

    private IEnumerator AnimatedDestroy()
    {
        m_Visible = false;
        if (TryGetComponent<CanvasGroup>(out var canvasGroup))
            yield return canvasGroup.DOFade(0, 0.25f).SetEase(Ease.OutQuart).WaitForCompletion();

        Destroy(gameObject);
    }
}
