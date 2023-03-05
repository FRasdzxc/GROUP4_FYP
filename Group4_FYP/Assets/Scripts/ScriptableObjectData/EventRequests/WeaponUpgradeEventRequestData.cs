using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Upgrade Event Request Data", menuName = "Game/Event Requests/Weapon Upgrade")]
public class WeaponUpgradeEventRequestData : EventRequestData
{
    public override void Invoke()
    {
        WeaponUpgradePanel.Instance.ShowWeaponUpgradePanel();
    }
}
