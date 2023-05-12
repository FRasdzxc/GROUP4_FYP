using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;

[CreateAssetMenu(fileName = "New Dungeon Map Data", menuName = "Game/Dungeon Map Data")]
public class DungeonMapData : MapData
{
    public DungeonType dungeonType;
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

    // public async override Task ShowMapMessage()  // until tutorial uses a custom mapdata or else this function will stay commented
    //     => await HUD.Instance.ShowHugeMessageAsync(mapName, $"{dungeonType} {mapType} / {mapDifficulty}");
}
