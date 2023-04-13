using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using HugeScript;

public class MobSpawner : MonoBehaviour
{
    [SerializeField] private MobTable Loot;
    [SerializeField] private int RandomDropCount = 1;
    [SerializeField] private float DropRange = 5f;
    float rangeX;
    float rangeY;

    private void Start()
    {
        //rangeX = GetComponent<SpriteRenderer>().bounds.size.x / 2;
        //rangeY = GetComponent<SpriteRenderer>().bounds.size.y / 2;
        //Loot.SpawnDrop(GetComponent<Tilemap>(), RandomDropCount, 0.1f, 0.1f);
        Spawn();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {

        }
    }
    public void Spawn()
    {
        Loot.SpawnDrop(GetComponent<Tilemap>(), RandomDropCount, 0.1f, 0.1f);
    }
    public void SetMobTable(MobTable mobTable)
    {
        Loot = mobTable;
    }
}
