using System.Threading.Tasks;
using UnityEngine;
using PathOfHero.UI;
using PathOfHero.Utilities;
using System.Collections;
using SceneControl = PathOfHero.Controllers.SceneController;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private GameMaps maps;
    [SerializeField]
    private string defaultMapId = "map_town";

    public GameState GameState { get; set; }
    public string MapId { get; set; }
    public Transform MapTransform => currentMap.transform;
    public MapType MapType => currentMapData.mapType;

    private Hero hero;
    private HUD hud;
    private PauseMenu pauseMenu;

    private MapData currentMapData;
    private GameObject currentMap;
    private bool mapCleared;

    protected override void Awake()
    {
        base.Awake();
        GameState = GameState.Paused;
        hero = GameObject.FindGameObjectWithTag("Player").GetComponent<Hero>();
        hud = GameObject.FindGameObjectWithTag("Canvas").GetComponent<HUD>();
        pauseMenu = GameObject.FindGameObjectWithTag("Canvas").GetComponent<PauseMenu>();
    }

    private void Start()
    {
        LoadMap(MapId, skipFadeIn: true);
    }

    void Update()
    {
        if (GameState != GameState.Playing)
            return;

        if (!mapCleared)
        {
            int mobCount = GameObject.FindGameObjectsWithTag("Mob").Length;
            hud.UpdateMobCount(mobCount);
            if (mobCount <= 0)
            {
                if (currentMapData is DungeonMapData dungeon)
                    dungeon.SpawnPortal();

                mapCleared = true;
            }
        }
    }

    public bool IsPlayingHostile()
        => GameState == GameState.Playing && MapType != MapType.Peaceful;

    public async void LoadMap(string mapId, bool saveOnLoaded = false, bool skipFadeIn = false)
    {
        if (string.IsNullOrWhiteSpace(mapId))
            mapId = defaultMapId;

        currentMapData = FindMap(mapId);
        if (!currentMapData)
            return;

        // if map is a dungeon, disable saving buttons
        if (currentMapData.mapType == MapType.Dungeon)
        {
            pauseMenu.SetDungeonMode(true);
            SaveSystem.Instance.SaveData(false, false);
        }
        else
        {
            pauseMenu.SetDungeonMode(false);
        }

        // start load map operation
        if (!skipFadeIn)
            await LoadingScreen.Instance.FadeInAsync();

        // destroy current map and its mobs
        if (currentMap)
            DestroyImmediate(currentMap);
        foreach (var mob in GameObject.FindGameObjectsWithTag("Mob"))
            Destroy(mob);
        foreach (var drop in GameObject.FindGameObjectsWithTag("Drop"))
            Destroy(drop);

        // clone new map
        currentMap = Instantiate(currentMapData.mapPrefab); // isn't Instantiate() synchronized?
        while (!currentMap) // wait for map to load
            await Task.Yield();
        
        // spawn hero
        hero.Spawn();

        // set objective
        if (currentMapData.objective.Length > 0) // if objective is not empty
            hud.ShowObjective(currentMapData.objective);
        else
            hud.HideObjective();

        await Task.Delay(50); // make the game look smoother
        await LoadingScreen.Instance.FadeOutAsync();

        if (currentMapData.mapType == MapType.Peaceful)
            StartCoroutine(hud.ShowHugeMessage(currentMapData.mapName, currentMapData.mapType.ToString()));
        else
            StartCoroutine(hud.ShowHugeMessage(currentMapData.mapName, $"{currentMapData.mapType} | {currentMapData.mapDifficulty}"));

        GameState = GameState.Playing;
        mapCleared = false;

        if (saveOnLoaded)
            SaveSystem.Instance.SaveData(true, false);
    }

    public MapData FindMap(string mapId)
    {
        foreach (MapData map in maps.maps)
        {
            if (map.mapId == mapId)
                return map;
        }

        Debug.LogError($"[Game Manager] No map with an id of {mapId} found");
        return null;
    }
}
