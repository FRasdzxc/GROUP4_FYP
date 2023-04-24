using UnityEngine;
using UnityEngine.Tilemaps;
using HugeScript;

public class MobSpawner : MonoBehaviour
{
    [SerializeField] private MobTable Loot;
    [SerializeField] private int RandomDropCount = 1;
    [SerializeField] private float DropRange = 5f;

    public MobTable MobTable
    {
        get => Loot;
        set => Loot = value;
    }

    private void Start()
        => Spawn();

    public void Spawn()
        => Loot.SpawnDrop(GetComponent<Tilemap>(), RandomDropCount, 0.1f, 0.1f);
}
