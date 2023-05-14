using UnityEngine;
using PathOfHero.Others;

[CreateAssetMenu(fileName = "New Map Change Event Request Data", menuName = "Game/Event Requests/Map Change")]
public class MapChangeEventRequestData : EventRequestData
{
    [Tooltip("Search for GameMaps (ScriptableObject) in \"Project\" tab, then find any MapData in the list; The MapData should have a mapId")]
    public string mapId;
    public bool requiresConfirmation = true;
    public bool saveOnMapLoaded = false;

    public override void Invoke()
    {
        MapData mapData = GameManager.Instance.FindMap(mapId);

        if (!mapData)
        {
            return;
        }
        
        if (requiresConfirmation)
        {
            if (mapData.mapType == MapType.Dungeon)
            {
                DungeonMapData dungeonMapData = mapData as DungeonMapData;

                ConfirmationPanel.Instance.ShowConfirmationPanel
                (
                    $"Enter <color={CustomColorStrings.green}>{mapData.mapName}</color>",
                    $"<color={CustomColorStrings.red}>!!</color> You cannot save/quit in a dungeon battle! You must play through the whole dungeon.\n<color={CustomColorStrings.red}>!!</color> Upon death, you will lose all your progress in this dungeon!\n\n<color={CustomColorStrings.yellow}>Type:</color> {dungeonMapData.dungeonType} {dungeonMapData.mapType}\n<color={CustomColorStrings.yellow}>Difficulty:</color> {dungeonMapData.mapDifficulty}",
                    () => { GameManager.Instance.LoadMap(mapId, saveOnMapLoaded); },
                    true
                );
            }
            else
            {
                ConfirmationPanel.Instance.ShowConfirmationPanel
                (
                    $"Enter <color={CustomColorStrings.green}>{mapData.mapName}</color>",
                    $"<color={CustomColorStrings.yellow}>Type:</color> {mapData.mapType}\n<color={CustomColorStrings.yellow}>Difficulty:</color> {mapData.mapDifficulty}",
                    () => { GameManager.Instance.LoadMap(mapId, saveOnMapLoaded); }
                );
            }
        }
        else
        {
            GameManager.Instance.LoadMap(mapId, saveOnMapLoaded);
        }
    }
}
