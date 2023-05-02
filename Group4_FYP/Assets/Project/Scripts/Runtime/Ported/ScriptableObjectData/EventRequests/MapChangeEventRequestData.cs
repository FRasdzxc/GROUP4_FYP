using UnityEngine;

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
                ConfirmationPanel.Instance.ShowConfirmationPanel
                (
                    $"Enter {mapData.mapName}",
                    $"Enter {mapData.mapName}?\n\n[!] Please note that you cannot save/quit in the middle of a dungeon battle! You will have to play through the whole dungeon. Upon death, you will lose all your progress in this dungeon!\n\nType: {mapData.mapType}\nDifficulty: {mapData.mapDifficulty}",
                    () => { GameManager.Instance.LoadMap(mapId, saveOnMapLoaded); },
                    true
                );
            }
            else
            {
                ConfirmationPanel.Instance.ShowConfirmationPanel
                (
                    $"Enter {mapData.mapName}",
                    $"Enter {mapData.mapName}?\n\nType: {mapData.mapType}\nDifficulty: {mapData.mapDifficulty}",
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
