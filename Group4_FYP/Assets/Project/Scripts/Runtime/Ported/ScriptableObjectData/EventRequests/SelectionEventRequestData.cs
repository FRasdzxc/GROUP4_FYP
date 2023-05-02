using UnityEngine;

[CreateAssetMenu(fileName = "New Selection Event Request Data", menuName = "Game/Event Requests/Selection")]
public class SelectionEventRequestData : EventRequestData
{
    public SelectionType selectionType;

    public override void Invoke()
    {
        SelectionPanel.Instance.ShowSelectionPanel(selectionType);
    }
}
