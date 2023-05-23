using System.Collections;
using UnityEngine;
using PathOfHero.UI;

public class BossDungeonMapController : DungeonMapController
{
    protected BossDungeonMapData bossDungeonMapData;

    protected override void Start()
    {
        bossDungeonMapData = mapData as BossDungeonMapData;
        base.Start();
    }

    protected override IEnumerator CheckCompletion()
    {
        // wait until all regular mobs are killed
        while (GameManager.Instance.MobCount > 0)
            yield return null;

        // await Notification.Instance.ShowNotification($"You will be teleported to the boss room in {intermission} seconds");
        yield return HUD.Instance.ShowHugeMessageAsync("Intermission", $"Teleporting in {bossDungeonMapData.intermission} seconds", 2.5f);
        yield return new WaitForSeconds(bossDungeonMapData.intermission);
        yield return StartCoroutine(FightBoss());

        if (bossDungeonMapData.portalPrefab)
            SpawnPortal();
    }

    protected IEnumerator FightBoss()
    {
        // teleport player
        AudioManager.Instance.StopMusic();
        yield return LoadingScreen.Instance.FadeIn();
        GameObject.FindGameObjectWithTag("Player").transform.position = bossDungeonMapData.bossRoomPos;
        yield return LoadingScreen.Instance.FadeOut();
        AudioManager.Instance.SetMusics(bossDungeonMapData.bossMusics);
        AudioManager.Instance.PlayMusic();

        // spawn boss
        if (bossDungeonMapData.bossPrefab)
        {
            yield return HUD.Instance.ShowHugeMessage("Boss", Color.magenta, "Battle", Color.white);
            Instantiate(bossDungeonMapData.bossPrefab, bossDungeonMapData.bossPos, Quaternion.identity);

            // wait for boss to spawn
            while (GameManager.Instance.MobCount <= 0)
                yield return null;
        }
        else
        {
            Debug.LogError("[BossDungeonMapData]: no boss found");
            yield break;
        }

        // wait until all mobs are killed
        while (GameManager.Instance.MobCount > 0)
            yield return null;
    }
}
