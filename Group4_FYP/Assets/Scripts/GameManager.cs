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

    private GameObject currentMap; // used as a clone
    // private int currentMapIndex;
    private string currentMapId;

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

        gameState = GameState.Playing;
        hero = GameObject.FindGameObjectWithTag("Player").GetComponent<Hero>();
        maskingCanvas = GameObject.FindGameObjectWithTag("MaskingCanvas").GetComponent<MaskingCanvas>();
        hud = GameObject.FindGameObjectWithTag("Canvas").GetComponent<HUD>();
        pauseMenu = GameObject.FindGameObjectWithTag("Canvas").GetComponent<PauseMenu>();

        if (!isTestScene)
        {
            // LoadMap(currentMapIndex);
            LoadMap(currentMapId);
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

    public MapType GetCurrentMapType()
    {
        // return maps.maps[currentMapIndex].mapType;
        return FindMap(currentMapId).mapType;
    }
    #endregion

    public bool IsPlayingHostile()
    {
        // return (gameState == GameState.Playing && maps.maps[currentMapIndex].mapType == MapType.Hostile);
        return (gameState == GameState.Playing && FindMap(currentMapId).mapType == MapType.Hostile);
    }

    // private void LoadMap(int mapIndex)
    // {
    //     // check if maps[currentMapIndex] exists or not
    //     if (!(mapIndex >= 0 && mapIndex < maps.Length))
    //     {
    //         Debug.LogError("Map " + mapIndex + " not found.");
    //         return;
    //     }

    //     // clone map
    //     currentMap = Instantiate(maps[mapIndex].map);
    //     currentMapIndex = mapIndex;

    //     var canvas = SceneController.Canvas;
    //     if (canvas != null)
    //     {
    //         HUD hud = canvas.GetComponent<HUD>();
    //     }

    //     // if map is a dungeon, disable saving buttons
    //     if (maps[currentMapIndex].mapType == MapType.Dungeon)
    //     {
    //         pauseMenu.SetDungeonMode(true);
    //     }
    //     else
    //     {
    //         pauseMenu.SetDungeonMode(false);
    //     }

    //     // spawn hero
    //     hero.Spawn();
    //     if (maps[currentMapIndex].mapType == MapType.Peaceful)
    //     {
    //         _ = hud.ShowHugeMessage(maps[currentMapIndex].map.name, maps[currentMapIndex].mapType.ToString());
    //     }
    //     else
    //     {
    //         _ = hud.ShowHugeMessage(maps[currentMapIndex].map.name, String.Format("{0} - {1}", maps[currentMapIndex].mapType.ToString(), maps[currentMapIndex].mapDifficulty.ToString()));
    //     }

    //     // set objective
    //     if (maps[mapIndex].objective.Length > 0) // if objective is not empty
    //     {
    //         hud.ShowObjective(maps[mapIndex].objective);
    //     }
    // }
    public async void LoadMap(string mapId)
    {
        MapData mapData = FindMap(mapId);

        // check if maps[currentMapIndex] exists or not
        if (!mapData)
        {
            return;
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

        await Task.Delay(50); // make the game look smoother
        await maskingCanvas.ShowMaskingCanvas(false);

        if (mapData.mapType == MapType.Peaceful)
        {
            _ = hud.ShowHugeMessage(mapData.mapName, mapData.mapType.ToString());
        }
        else
        {
            _ = hud.ShowHugeMessage(mapData.mapName, String.Format("{0} | {1}", mapData.mapType.ToString(), mapData.mapDifficulty.ToString()));
        }
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
