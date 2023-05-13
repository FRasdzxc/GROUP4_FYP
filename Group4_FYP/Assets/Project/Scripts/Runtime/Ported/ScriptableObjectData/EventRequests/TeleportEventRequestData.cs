using UnityEngine;
using PathOfHero.UI;

[CreateAssetMenu(fileName = "New Teleport Event Request Data", menuName = "Game/Event Requests/Teleport")]
public class TeleportEventRequestData : EventRequestData
{
    public Vector2 destination;

    [TextArea(5, 5)]
    public string description;

    public override void Invoke()
    {
        ConfirmationPanel.Instance.ShowConfirmationPanel(
            $"Teleport",
            $"Are you sure you want to teleport?\n\n\"{description}\"",
            async () =>
            {
                await LoadingScreen.Instance.FadeInAsync();
                GameObject.FindGameObjectWithTag("Player").transform.position = destination;
                await LoadingScreen.Instance.FadeOutAsync();
            }
        );
    }

    public void SetUp(Vector2 destination, string description)
    {
        this.destination = destination;
        this.description = description;
    }
}
