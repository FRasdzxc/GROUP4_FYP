using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(fileName = "New Dungeon Map Data", menuName = "Game/Map Data/Dungeon Map Data")]
public class DungeonMapData : HostileMapData
{
    public DungeonType dungeonType;
}
