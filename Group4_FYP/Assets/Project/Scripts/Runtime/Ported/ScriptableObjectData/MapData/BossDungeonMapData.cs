using UnityEngine;
using System.Threading.Tasks;
using PathOfHero.UI;

[CreateAssetMenu(fileName = "New Boss Dungeon Map Data", menuName = "Game/Map Data/Boss Dungeon Map Data")]
public class BossDungeonMapData : DungeonMapData
{
    [Tooltip("Unit: seconds")]
    public float intermission = 5f;
    public Vector2 bossRoomPos;
    public GameObject bossPrefab;
    public Vector2 bossPos;
    public MusicEntry[] bossMusics;
}
