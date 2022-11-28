using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HugeScript;

public class MobSpawner : MonoBehaviour
{
    public MobTable Loot;
    public int RandomDropCount = 1;
    public float DropRange = 5f;
    float rangeX;
    float rangeY;

    private void Start()
    {
        rangeX = GetComponent<SpriteRenderer>().bounds.size.x / 2;
        rangeY = GetComponent<SpriteRenderer>().bounds.size.y / 2;
        Loot.SpawnDrop(this.transform, RandomDropCount, rangeX, rangeY);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {

        }
    }
}
