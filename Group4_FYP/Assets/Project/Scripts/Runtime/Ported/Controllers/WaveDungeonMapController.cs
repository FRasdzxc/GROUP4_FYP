using System.Collections;
using UnityEngine;
using PathOfHero.Others;

public class WaveDungeonMapController : DungeonMapController
{
    protected WaveDungeonMapData waveDungeonMapData;
    protected int currentWave;
    protected MobSpawner mobSpawner;
    protected bool isStopped;

    protected override void Start()
    {
        waveDungeonMapData = mapData as WaveDungeonMapData;
        currentWave = -1;
        mobSpawner = Common.RecursiveFindTag(transform, "MobGround").GetComponent<MobSpawner>();
        base.Start();
    }

    protected override void Update()
    {
        if (!isStopped && Hero.Instance.IsDead)
        {
            isStopped = true;
            StopCoroutine(CheckCompletion());
            mobSpawner.MobTable = null;
        }

        base.Update();
    }

    protected override IEnumerator CheckCompletion() // TODO: fix; reason: mobs spawn after return to town after dying right after wave is cleared
    {
        if (waveDungeonMapData.waves == null)
        {
            Debug.LogWarning("[WaveDungeonMapData] waves array is empty");
            SpawnPortal();
            yield break;
        }

        while (currentWave < waveDungeonMapData.waves.Length - 1)
        {
            yield return StartCoroutine(NextWave());
            while (GameManager.Instance.MobCount > 0)
                yield return null;
            yield return HUD.Instance.ShowHugeMessage("Wave", "cleared");
        }

        mobSpawner.MobTable = null; // important!! or else the mobtable will be stuck at the last wave's mobtable forever
        SpawnPortal();
    }

    protected IEnumerator NextWave()
    {
        if (!mobSpawner)
            yield break;

        yield return new WaitForSeconds(waveDungeonMapData.intermission);
        currentWave++;

        yield return HUD.Instance.ShowHugeMessage($"Wave {currentWave + 1}", new Color32(255, 135, 0, 255), $"of {waveDungeonMapData.waves.Length}", Color.white);
        mobSpawner.MobTable = waveDungeonMapData.waves[currentWave];
        mobSpawner.Spawn();

        while (GameManager.Instance.MobCount <= 0) // wait for mobs to finish spawning
            yield return null;
    }
}
