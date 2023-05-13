using UnityEngine;

[CreateAssetMenu(fileName = "New Item Event Request Data", menuName = "Game/Event Requests/Item")]
public class ItemEventRequestData : EventRequestData
{
    public enum ActionType { Add, Remove }
    public ActionType actionType;
    public ItemData item;

    public override void Invoke()
    {
        if (actionType.Equals(ActionType.Add))
            Inventory.Instance.AddItem(item);
        else
            Inventory.Instance.RemoveItem(item);
    }
}
