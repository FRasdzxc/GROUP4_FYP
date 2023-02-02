using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private bool isTestScene; // temp only

    [SerializeField] private GameObject[] maps;

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

    private void LoadMap(int mapIndex)
    {
        // check if maps[currentMapIndex] exists or not
        if (!(mapIndex >= 0 && mapIndex < maps.Length))
        {
            Debug.LogError("Map " + mapIndex + " not found.");
            return;
        }

        // clone map
        currentMap = Instantiate(maps[mapIndex]);
        currentMapIndex = mapIndex;

        // spawn hero
        hero.Spawn();
        _ = hud.ShowHugeMessage(maps[currentMapIndex].name, Color.white);
    }
}
