using System;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private bool isTestScene; // temp only

    // [SerializeField] private GameObject[] maps;

    // [Serializable]
    // public struct MapEntry
    // {
    //     public GameObject map;
    //     [Tooltip("Players cannot attack / take damage when Map Type is Peaceful")]
    //     public MapType mapType;
    //     public MapDifficulty mapDifficulty;
    //     [TextArea(2, 5)]
    //     public string objective;
    // }

    // [SerializeField] private MapEntry[] maps;
    [SerializeField] private GameMaps maps;

    [SerializeField] private string[] tutorialDialogues; // can be written better?
    [SerializeField] private string defaultMapId = "map_town";

    private GameObject currentMap; // used as a clone
    // private int currentMapIndex;
    private string currentMapId;
    private bool mapClearChecked;

    private GameState gameState;

    private Hero hero;
    private MaskingCanvas maskingCanvas;
    private HUD hud;
    private PauseMenu pauseMenu;

    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }

    void Awake()
    {
        if (!instance)
        {
            instance = this;
        }

        gameState = GameState.Paused;
        hero = GameObject.FindGameObjectWithTag("Player").GetComponent<Hero>();
        maskingCanvas = GameObject.FindGameObjectWithTag("MaskingCanvas").GetComponent<MaskingCanvas>();
        hud = GameObject.FindGameObjectWithTag("Canvas").GetComponent<HUD>();
        pauseMenu = GameObject.FindGameObjectWithTag("Canvas").GetComponent<PauseMenu>();

        if (!isTestScene)
        {
            // LoadMap(currentMapIndex);
            LoadMap(currentMapId);
        }
        else
        {
            gameState = GameState.Playing;
        }
    }

    void Update()
    {
        int mobCount = GameObject.FindGameObjectsWithTag("Mob").Length;
        hud.UpdateMobCount(mobCount);

        if (mobCount <= 0 && !mapClearChecked) {
            mapClearChecked = true;
            Debug.Log("map is cleared@!");

            MapData mapData = FindMap(currentMapId);
            if (mapData is DungeonMapData)
            {
                DungeonMapData dungeonMapData = mapData as DungeonMapData;
                dungeonMapData.SpawnPortal();
            }
        }
    }

    #region Setters/Getters
    // public void SetMap(int mapIndex)
    // {
    //     currentMapIndex = mapIndex;
    // }

    public void SetMapId(string mapId)
    {
        currentMapId = mapId;
    }

    // public int GetMap()
    // {
    //     return currentMapIndex;
    // }
    public string GetMapId()
    {
        return currentMapId;
    }

    public void SetGameState(GameState gameState)
    {
        this.gameState = gameState;
    }

    public GameState GetGameState()
    {
        return gameState;
    }

    public GameObject GetCurrentMap()
    {
        return currentMap;
    }

    public MapType GetCurrentMapType()
    {
        // return maps.maps[currentMapIndex].mapType;
        return FindMap(currentMapId).mapType;
    }
    #endregion

    public bool IsPlayingHostile()
    {
        // return (gameState == GameState.Playing && maps.maps[currentMapIndex].mapType == MapType.Hostile);
        return (gameState == GameState.Playing && FindMap(currentMapId).mapType != MapType.Peaceful);
    }

    public async void LoadMap(string mapId)
    {
        MapData mapData = FindMap(mapId);

        // check if maps[currentMapIndex] exists or not
        if (!mapData)
        {
            // return;
            mapData = FindMap(defaultMapId);
        }

        await maskingCanvas.ShowMaskingCanvas(true);

        // clone map
        if (currentMap)
        {
            Destroy(currentMap);
            while (currentMap) // wait for map to be destroyed
            {
                await Task.Yield();
            }
        }
        GameObject[] mobs = GameObject.FindGameObjectsWithTag("Mob");
        foreach (GameObject go in mobs)
        {
            Destroy(go);
        }
        GameObject[] drops = GameObject.FindGameObjectsWithTag("Drop");
        foreach (GameObject go in drops)
        {
            Destroy(go);
        }

        currentMap = Instantiate(mapData.mapPrefab); // isn't Instantiate() synchronized?
        currentMapId = mapData.mapId;
        while (!currentMap) // wait for map to load
        {
            await Task.Yield();
        }

        var canvas = SceneController.Canvas;
        if (canvas != null)
        {
            HUD hud = canvas.GetComponent<HUD>();
        }

        // if map is a dungeon, disable saving buttons
        if (mapData.mapType == MapType.Dungeon)
        {
            pauseMenu.SetDungeonMode(true);
        }
        else
        {
            pauseMenu.SetDungeonMode(false);
        }
        
        // spawn hero
        hero.Spawn();

        // set objective
        if (mapData.objective.Length > 0) // if objective is not empty
        {
            hud.ShowObjective(mapData.objective);
        }
        else
        {
            hud.HideObjective();
        }

        await Task.Delay(50); // make the game look smoother
        await maskingCanvas.ShowMaskingCanvas(false);

        if (mapData.mapType == MapType.Peaceful)
        {
            _ = hud.ShowHugeMessage(mapData.mapName, mapData.mapType.ToString());
        }
        else
        {
            // _ = hud.ShowHugeMessage(mapData.mapName, String.Format("{0} | {1}", mapData.mapType.ToString(), mapData.mapDifficulty.ToString()));
            _ = hud.ShowHugeMessage(mapData.mapName, $"{mapData.mapType.ToString()} | {mapData.mapDifficulty.ToString()}");
        }

        gameState = GameState.Playing;
        mapClearChecked = false;
    }

    public MapData FindMap(string mapId)
    {
        foreach (MapData map in maps.maps)
        {
            if (map.mapId == mapId)
            {
                return map;
            }
        }

        Debug.LogError($"No map with an id of {mapId} found");
        return null;
    }
}
