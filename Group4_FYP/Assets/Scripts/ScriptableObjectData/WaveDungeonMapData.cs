using System.Threading.Tasks;
using UnityEngine;
using HugeScript;
using PathOfHero.Others;

[CreateAssetMenu(fileName = "New Wave Dungeon Map Data", menuName = "Game/Wave Dungeon Map Data")]
public class WaveDungeonMapData : DungeonMapData
{
    public MobTable[] waves;
    [Tooltip("Unit: ms")] public int intermission = 1000;
    private int currentWave;

    public async Task NextWave()
    {
        currentWave++;

        await HUD.Instance.ShowHugeMessageAsync($"Wave {currentWave + 1}", new Color32(255, 125, 0, 255), $"of {waves.Length}", Color.white);
        MobSpawner ground = Common.RecursiveFindTag(mapPrefab.transform, "MobGround").GetComponent<MobSpawner>();
        ground.MobTable = waves[currentWave];
        ground.Spawn();

        while (GameManager.Instance.MobCount <= 0) // wait for mobs to finish spawning
            await Task.Yield();
    }

    public override void SetUp()
    {
        base.SetUp();
        currentWave = -1;
        Debug.Log($"current wave {currentWave}");
    }

    public override async Task CheckCompletion()
    {
        if (waves == null)
        {
            Debug.LogWarning("[WaveDungeonMapData] waves array is empty");
            SpawnPortal();
            return;
        }

        while (currentWave < waves.Length - 1)
        {
            await NextWave();

            while (GameManager.Instance.MobCount > 0)
                await Task.Yield();

            await HUD.Instance.ShowHugeMessageAsync("Wave", "cleared");
            await Task.Delay(intermission);
        }

        SpawnPortal();
    }
}
