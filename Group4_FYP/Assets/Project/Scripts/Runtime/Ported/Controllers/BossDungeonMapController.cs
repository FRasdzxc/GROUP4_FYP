using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using PathOfHero.UI;
using PathOfHero.Others;

public class BossDungeonMapController : DungeonMapController
{
    [SerializeField]
    protected Tilemap bossRoomTilemap;

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

        StopTimer();
        yield return new WaitForSeconds(1f);

        if (Inventory.Instance.FindItem(bossDungeonMapData.gem))
        {
            ConfirmationPanel.Instance.ShowConfirmationPanel
            (
                $"<color=magenta>Boss Battle</color>",
                $"You have a Gem in your Inventory! Do you want to participate in the Boss Battle?\n\nIf you participate, your data will be saved before you enter the Boss Room.",
                () => StartCoroutine(FightBoss()),
                () => SpawnPortal(bossDungeonMapData.secondaryPortalPos),
                false,
                false,
                $"<color={CustomColorStrings.yellow}>Cost:</color> 1 Gem"
            );
        }
        else
        {
            yield return Notification.Instance.ShowNotification($"You do not have a Gem, so you will not be participating in the Boss Battle", 5f);
            SpawnPortal(bossDungeonMapData.secondaryPortalPos);
        }
    }

    protected IEnumerator FightBoss()
    {
        Inventory.Instance.RemoveItem(bossDungeonMapData.gem);
        SaveSystem.Instance.SaveData(true, false);

        // intermission
        yield return HUD.Instance.ShowHugeMessage("Intermission", $"Teleporting in {bossDungeonMapData.intermission} seconds", 2.5f);
        yield return new WaitForSeconds(bossDungeonMapData.intermission);

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
            GameObject boss = Instantiate(bossDungeonMapData.bossPrefab, bossDungeonMapData.bossPos, Quaternion.identity);

            // wait for boss to spawn
            while (GameManager.Instance.MobCount <= 0)
                yield return null;
        }
        else
        {
            Debug.LogError("[Boss Dungeon Map Data] no boss found");
            if (bossDungeonMapData.portalPrefab)
                SpawnPortal();
            yield break;
        }

        // wait until all mobs are killed
        while (GameManager.Instance.MobCount > 0)
            yield return null;

        // spawn portal if all mobs are killed
        if (bossDungeonMapData.portalPrefab)
            SpawnPortal();
    }
}
