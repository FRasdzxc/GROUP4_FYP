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
    private MobSpawner mobSpawner;

    public async Task NextWave()
    {
        await Task.Delay(intermission);
        currentWave++;

        await HUD.Instance.ShowHugeMessageAsync($"Wave {currentWave + 1}", new Color32(255, 125, 0, 255), $"of {waves.Length}", Color.white);
        mobSpawner.MobTable = waves[currentWave];
        mobSpawner.Spawn();

        while (GameManager.Instance.MobCount <= 0) // wait for mobs to finish spawning
            await Task.Yield();
    }

    public override void SetUp()
    {
        base.SetUp();
        currentWave = -1;
        Common.RecursiveFindTag(mapPrefab.transform, "MobGround").TryGetComponent<MobSpawner>(out mobSpawner);
    }

    public override async Task CheckCompletion() // TODO: fix; reason: mobs spawn after return to town after dying right after wave is cleared
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
            {
                if (Hero.Instance.IsDead)
                {
                    mobSpawner.MobTable = null;
                    return;
                }

                await Task.Yield();
            }

            await HUD.Instance.ShowHugeMessageAsync("Wave", "cleared");
        }

        mobSpawner.MobTable = null; // important!! or else the mobtable will be stuck at the last wave's mobtable forever
        SpawnPortal();
    }
}
