using UnityEngine;

[CreateAssetMenu(fileName = "New BuySell Event Request Data", menuName = "Game/Event Requests/BuySell")]
public class BuySellEventRequestData : EventRequestData
{
    public BuySellType buySellType;

    public override void Invoke()
    {
        BuySellPanel.Instance.ShowBuySellPanel(buySellType);
    }
}
