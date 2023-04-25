using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;

[CreateAssetMenu(fileName = "New Dungeon Map Data", menuName = "Game/Dungeon Map Data")]
public class DungeonMapData : MapData
{
    public GameObject portalPrefab;
    public Vector2 portalPos;

    public async void SpawnPortal()
    {
        //SaveSystem.Instance.SaveData(false, false);
        _ = HUD.Instance.ShowHugeMessageAsync("All", "cleared");

        GameObject clone = Instantiate(portalPrefab, portalPos, Quaternion.identity, GameManager.Instance.MapTransform);
        clone.transform.localScale = Vector2.zero;
        await clone.transform.DOScale(Vector2.one, 1f).SetEase(Ease.InQuart).AsyncWaitForCompletion();
        _ = Notification.Instance.ShowNotification("Portal is opened!");
    }

    public async override Task CheckCompletion()
    {
        while (GameManager.Instance.MobCount > 0)
            await Task.Yield();

        SpawnPortal();
    }
}
