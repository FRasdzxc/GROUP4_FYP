using System;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private bool isTestScene; // temp only

    // [SerializeField] private GameObject[] maps;

    [Serializable]
    public struct MapEntry
    {
        public GameObject map;
        [Tooltip("Players cannot attack / take damage when Map Type is Peaceful")]
        public MapType mapType;
        [TextArea(2, 5)]
        public string objective;
    }

    [SerializeField] private MapEntry[] maps;

    [SerializeField] private string[] tutorialDialogues; // can be written better?

    private GameObject currentMap; // used as a clone
    private int currentMapIndex;

    private GameState gameState;

    private Hero hero;
    private MaskingCanvas maskingCanvas;
    private HUD hud;

    private static GameController instance;
    public static GameController Instance
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

        if (!isTestScene)
        {
            LoadMap(currentMapIndex);
        }
    }

    public void SetMap(int mapIndex)
    {
        currentMapIndex = mapIndex;
    }

    public int GetMap()
    {
        return currentMapIndex;
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
        return maps[currentMapIndex].mapType;
    }

    public bool IsPlayingHostile()
    {
        return (gameState == GameState.Playing && maps[currentMapIndex].mapType == MapType.Hostile);
    }

    private void LoadMap(int mapIndex)
    {
        // check if maps[currentMapIndex] exists or not
        if (!(mapIndex >= 0 && mapIndex < maps.Length))
        {
            Debug.LogError("Map " + mapIndex + " not found.");
            return;
        }

        // clone map
        currentMap = Instantiate(maps[mapIndex].map);
        currentMapIndex = mapIndex;

        // spawn hero
        hero.Spawn();
        _ = hud.ShowHugeMessage(maps[currentMapIndex].map.name, Color.white);

        // set objective
        if (maps[mapIndex].objective.Length > 0) // if objective is not empty
        {
            hud.ShowObjective(maps[mapIndex].objective);
        }
    }
}
