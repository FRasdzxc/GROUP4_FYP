using UnityEngine;
using UnityEngine.UI;
using PathOfHero.Utilities;

public class Message : Singleton<Message>
{
    [SerializeField] private ScrollRect messageScrollRect;
    [SerializeField] private GameObject messageContentPanel;
    [SerializeField] private GameObject messageItemPrefab;
    [SerializeField] private Sprite defaultMessageSprite; // preventive

    public void ShowMessage(string message, Sprite sprite = null, float duration = 2.5f)
    {
        if (!messageContentPanel.activeInHierarchy)
            return;

        var clone = Instantiate(messageItemPrefab, messageContentPanel.transform);
        if (clone.TryGetComponent<MessageItem>(out var messageItem))
            messageItem.ShowMessageItem(message, duration, sprite != null ? sprite : defaultMessageSprite);
    }
}
