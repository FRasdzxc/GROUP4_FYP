using System.Threading.Tasks;
using UnityEngine;
using HugeScript;
using FYP;

[CreateAssetMenu(fileName = "New Wave Dungeon Map Data", menuName = "Game/Wave Dungeon Map Data")]
public class WaveDungeonMapData : DungeonMapData
{
    public MobTable[] waves;
    private int currentWave = -1;

    public async void NextWave() {
        Debug.Log($"currentWave {currentWave}");
        
        if (waves == null) {
            SpawnPortal();
            return;
        }

        await HUD.Instance.ShowHugeMessage("Wave", "cleared");
        currentWave++;
        if (currentWave < waves.Length - 1) {
            await Task.Delay(5000);
            await HUD.Instance.ShowHugeMessage($"Wave {currentWave + 1}", new Color32(255, 125, 0, 255), $"of {waves.Length}", Color.white);

            MobSpawner ground = Common.RecursiveFindTag(mapPrefab.transform, "MobGround").GetComponent<MobSpawner>();
            ground.SetMobTable(waves[currentWave]);
            ground.Spawn();
        }
        else
        {
            SpawnPortal();
        }
    }
}
