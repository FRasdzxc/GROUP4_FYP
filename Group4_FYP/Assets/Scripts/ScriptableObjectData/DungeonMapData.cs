using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(fileName = "New Dungeon Map Data", menuName = "Game/Dungeon Map Data")]
public class DungeonMapData : MapData
{
    public GameObject portalPrefab;
    public Vector2 portalPos;

    public void SpawnPortal()
    {
        GameObject clone = Instantiate(portalPrefab, portalPos, Quaternion.identity, GameManager.Instance.GetCurrentMap().transform);
        clone.transform.localScale = Vector2.zero;
        clone.transform.DOScale(Vector2.one, 1f).SetEase(Ease.InQuart);
    }
}
