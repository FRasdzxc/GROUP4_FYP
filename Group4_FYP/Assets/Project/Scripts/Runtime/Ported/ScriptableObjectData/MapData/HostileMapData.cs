using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(fileName = "New Hostile Map Data", menuName = "Game/Map Data/Hostile Map Data")]
public class HostileMapData : MapData
{
    public GameObject portalPrefab;
    public Vector2 portalPos;
}
