using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(fileName = "New Hostile Map Data", menuName = "Game/Map Data/Hostile Map Data")]
public class HostileMapData : MapData
{
    public GameObject portalPrefab;
    public Vector2 portalPos;

    public async void SpawnPortal()
    {
        if (isStopped)
            return;

        //SaveSystem.Instance.SaveData(false, false);
        _ = HUD.Instance.ShowHugeMessageAsync("All", "cleared");

        GameObject clone = Instantiate(portalPrefab, portalPos, Quaternion.identity, GameManager.Instance.MapTransform);
        clone.transform.localScale = Vector2.zero;
        await clone.transform.DOScale(Vector2.one, 1f).SetEase(Ease.InQuart).AsyncWaitForCompletion();
        _ = Notification.Instance.ShowNotification("Portal is opened!");

        DirectionArrowController.Instance.AddDirection(DirectionType.ReturnPortal, clone.transform);
    }

    public async override Task CheckCompletion()
    {
        while (GameManager.Instance.MobCount > 0)
            await Task.Yield();

        if (portalPrefab)
            SpawnPortal();
    }
}
