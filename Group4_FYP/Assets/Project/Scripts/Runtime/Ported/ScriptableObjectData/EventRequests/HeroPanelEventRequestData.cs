using UnityEngine;

[CreateAssetMenu(fileName = "New Hero Panel Event Request Data", menuName = "Game/Event Requests/Hero Panel")]
public class HeroPanelEventRequestData : EventRequestData
{
    public enum ActionType { ShowPanel, HidePanel }
    public ActionType actionType;

    public override void Invoke()
    {
        if (actionType.Equals(ActionType.ShowPanel))
            HeroPanel.Instance.ShowPanel();
        else
            HeroPanel.Instance.HidePanel();
    }
}
