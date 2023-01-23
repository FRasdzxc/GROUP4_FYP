using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject[] maps;

    private GameObject currentMap; // used as a clone
    private int currentMapIndex;

    private Hero hero;
    private MaskingCanvas maskingCanvas;
    private HUD hud;

    // Start is called before the first frame update
    void Start()
    {
        hero = GameObject.FindGameObjectWithTag("Player").GetComponent<Hero>();
        maskingCanvas = GameObject.FindGameObjectWithTag("MaskingCanvas").GetComponent<MaskingCanvas>();
        hud = GameObject.FindGameObjectWithTag("Canvas").GetComponent<HUD>();
        LoadMap(currentMapIndex);
    }

    public void SetMap(int mapIndex)
    {
        currentMapIndex = mapIndex;
    }

    public int GetMap()
    {
        return currentMapIndex;
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
