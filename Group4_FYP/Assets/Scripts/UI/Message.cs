using UnityEngine;
using PathOfHero.Utilities;

public class Message : Singleton<Message>
{
    [SerializeField] private GameObject messagePanel;
    [SerializeField] private GameObject messageItemPrefab;
    [SerializeField] private Sprite defaultMessageSprite; // preventive

    public void ShowMessage(string message, Sprite sprite = null, float duration = 1.5f)
    {
        var clone = Instantiate(messageItemPrefab, messagePanel.transform);
        if (clone.TryGetComponent<MessageItem>(out var messageItem))
            messageItem.ShowMessageItem(message, duration, sprite != null ? sprite : defaultMessageSprite);
    }
}
