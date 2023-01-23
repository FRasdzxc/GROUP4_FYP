using System.Threading.Tasks;
using UnityEngine;

public class Notification : MonoBehaviour
{
    [SerializeField] private GameObject notificationPanel;
    private GameObject clone;

    private static Notification instance;
    public static Notification Instance
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

    public async Task ShowNotification(string notification, float duration = 2f)
    {
        if (clone)
        {
            Destroy(clone);
        }

        clone = Instantiate(notificationPanel, gameObject.transform);
        await clone.GetComponent<NotificationPanel>().ShowNotificationPanel(notification, duration);
        Destroy(clone);
    }
}
