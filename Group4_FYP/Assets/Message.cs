using UnityEngine;

public class Message : MonoBehaviour
{
    [SerializeField] private GameObject messagePanel;
    [SerializeField] private GameObject messageItemPrefab;
    [SerializeField] private Sprite defaultMessageSprite; // preventive
    private GameObject clone;

    private static Message instance;
    public static Message Instance
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

    public void ShowMessage(string message, Sprite sprite = null, float duration = 1.5f)
    {
        clone = Instantiate(messageItemPrefab, messagePanel.transform);

        if (sprite)
        {
            clone.GetComponent<MessageItem>().ShowMessageItem(message, duration, sprite);
        }
        else
        {
            clone.GetComponent<MessageItem>().ShowMessageItem(message, duration, defaultMessageSprite);
        }
    }
}
