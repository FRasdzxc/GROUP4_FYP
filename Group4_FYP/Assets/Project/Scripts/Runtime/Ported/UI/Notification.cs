using System.Threading.Tasks;
using UnityEngine;
using PathOfHero.Utilities;

public class Notification : Singleton<Notification>
{
    [SerializeField] private GameObject notificationPanel;
    [SerializeField] private AudioClip notificationSound;

    private GameObject clone;

    public async Task ShowNotification(string notification, float duration = 2f)
    {
        if (clone)
        {
            Destroy(clone);
        }

        clone = Instantiate(notificationPanel, gameObject.transform);
        AudioManager.Instance.PlaySound(notificationSound);
        await clone.GetComponent<NotificationPanel>().ShowNotificationPanel(notification, duration);
        Destroy(clone);
    }
}
