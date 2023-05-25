using UnityEngine;

[CreateAssetMenu(fileName = "New Selection Event Request Data", menuName = "Game/Event Requests/Selection")]
public class SelectionEventRequestData : EventRequestData
{
    public MapSelectionType selectionType;

    public override void Invoke()
    {
        MapSelectionPanel.Instance.ShowMapSelectionPanel(selectionType);
    }
}
