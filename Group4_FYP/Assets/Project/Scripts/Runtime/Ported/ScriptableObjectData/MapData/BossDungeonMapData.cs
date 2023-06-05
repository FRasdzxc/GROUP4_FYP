using UnityEngine;

[CreateAssetMenu(fileName = "New Boss Dungeon Map Data", menuName = "Game/Map Data/Boss Dungeon Map Data")]
public class BossDungeonMapData : DungeonMapData
{
    [Tooltip("Unit: seconds")]
    public float intermission = 5f;
    public Vector2 bossRoomPos;
    public GameObject bossPrefab;
    public Vector2 bossPos;
    public MusicEntry[] bossMusics;
    public GemItemData gem;
    [Tooltip("For when the player does not have a gem, the portal will spawn here instead.")]
    public Vector2 secondaryPortalPos;
    public MusicEntry bossDefeatMusic;
    public AudioClip badgeObtainSound;
}
