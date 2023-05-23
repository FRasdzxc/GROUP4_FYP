using System.Collections;
using UnityEngine;
using DG.Tweening;

public class HostileMapController : MapController
{
    protected HostileMapData hostileMapData;

    protected override void Start()
    {
        hostileMapData = mapData as HostileMapData;
        base.Start();
    }

    protected async void SpawnPortal()
    {
        //SaveSystem.Instance.SaveData(false, false);
        _ = HUD.Instance.ShowHugeMessageAsync("All", "cleared");

        GameObject clone = Instantiate(hostileMapData.portalPrefab, hostileMapData.portalPos, Quaternion.identity, GameManager.Instance.MapTransform);
        clone.transform.localScale = Vector2.zero;
        await clone.transform.DOScale(Vector2.one, 1f).SetEase(Ease.InQuart).AsyncWaitForCompletion();
        _ = Notification.Instance.ShowNotification("Portal is opened!");

        DirectionArrowController.Instance.AddDirection(DirectionType.ReturnPortal, clone.transform);
    }

    protected override IEnumerator CheckCompletion()
    {
        while (GameManager.Instance.MobCount > 0)
            yield return null;

        if (hostileMapData.portalPrefab)
            SpawnPortal();
    }
}
