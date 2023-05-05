using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using PathOfHero.Telemetry;
using PathOfHero.UI;
using PathOfHero.Utilities;

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
    public int MobCount { get; set; }

    protected override void Awake()
    {
        base.Awake();
        GameState = GameState.Paused;
        hero = GameObject.FindGameObjectWithTag("Player").GetComponent<Hero>();
        hud = GameObject.FindGameObjectWithTag("Canvas").GetComponent<HUD>();
        pauseMenu = GameObject.FindGameObjectWithTag("Canvas").GetComponent<PauseMenu>();
    }

    private IEnumerator Start()
    {
        //yield return new WaitUntil(() => DataCollector.Instance != null && DataCollector.Instance.CurrentStats != null);
        LoadMap(MapId, skipFadeIn: true);
        return null;

    }

    void Update()
    {
        if (GameState != GameState.Playing)
            return;

        MobCount = GameObject.FindGameObjectsWithTag("Mob").Length;
        hud.UpdateMobCount(MobCount);
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

        // stop music
        AudioManager.Instance.StopMusic();

        // start load map operation
        if (!skipFadeIn)
            await LoadingScreen.Instance.FadeInAsync();

        // destroy current map and its mobs
        if (currentMap)
            DestroyImmediate(currentMap);
        foreach (var mob in GameObject.FindGameObjectsWithTag("Mob"))
            Destroy(mob);
        foreach (var chest in GameObject.FindGameObjectsWithTag("Chest"))
            Destroy(chest);
        foreach (var drop in GameObject.FindGameObjectsWithTag("Drop"))
            Destroy(drop);

        // clone new map
        currentMap = Instantiate(currentMapData.mapPrefab); // isn't Instantiate() synchronized?
        while (!currentMap) // wait for map to load
            await Task.Yield();
        
        // spawn hero
        hero.Spawn();

        await Task.Delay(50); // make the game look smoother
        await LoadingScreen.Instance.FadeOutAsync();

        currentMapData.SetUp();

        GameState = GameState.Playing;
        DirectionArrowController.Instance.Activated = false; // reset arrows indicating mob directions

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

    public void GiveUp()
    {
        if (MapType != MapType.Dungeon || MapType != MapType.WaveDungeon)
            Debug.LogWarning("[GameManager] Failed trying to give up in a non-dungeon map");

        currentMapData.Stop();
        GameManager.Instance.LoadMap("map_town");   // cannot respawn in dungeon so player will be teleported back to town
        SaveSystem.Instance.LoadData();             // revert all stats earned in dungeon
    }

    public GameObject GetMap()
    {
        return currentMap;
    }
}
