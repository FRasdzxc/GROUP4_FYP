using UnityEngine;

[CreateAssetMenu(fileName = "New Blacksmith Panel Event Request Data", menuName = "Game/Event Requests/Blacksmith Panel")]
public class BlacksmithPanelEventRequestData : EventRequestData
{
    public enum ActionType { ShowPanel, HidePanel }
    public ActionType actionType;

    public override void Invoke()
    {
        if (actionType.Equals(ActionType.ShowPanel))
            BlacksmithPanel.Instance.ShowPanel();
        else
            BlacksmithPanel.Instance.HidePanel();
    }
}
