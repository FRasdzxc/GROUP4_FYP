using System.Threading.Tasks;
using UnityEngine;
using HugeScript;
using PathOfHero.Others;

[CreateAssetMenu(fileName = "New Wave Dungeon Map Data", menuName = "Game/Wave Dungeon Map Data")]
public class WaveDungeonMapData : DungeonMapData
{
    public MobTable[] waves;
    private int currentWave = -1;

    public async void NextWave()
    {
        Debug.Log($"currentWave {currentWave}");
        
        if (waves == null || currentWave + 1 == waves.Length)
        {
            SpawnPortal();
            return;
        }

        currentWave++;
        if (currentWave != 0)
        {
            await HUD.Instance.ShowHugeMessageAsync("Wave", "cleared");
            await Task.Delay(5000);
        }

        await HUD.Instance.ShowHugeMessageAsync($"Wave {currentWave + 1}", new Color32(255, 125, 0, 255), $"of {waves.Length}", Color.white);
        MobSpawner ground = Common.RecursiveFindTag(mapPrefab.transform, "MobGround").GetComponent<MobSpawner>();
        ground.SetMobTable(waves[currentWave]);
        ground.Spawn();
    }
}
