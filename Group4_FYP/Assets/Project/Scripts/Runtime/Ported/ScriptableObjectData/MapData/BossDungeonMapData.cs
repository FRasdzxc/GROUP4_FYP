using UnityEngine;
using System.Threading.Tasks;
using PathOfHero.UI;

[CreateAssetMenu(fileName = "New Boss Dungeon Map Data", menuName = "Game/Map Data/Boss Dungeon Map Data")]
public class BossDungeonMapData : DungeonMapData
{
    [Tooltip("Unit: seconds")]
    public float intermission = 5f;
    public Vector2 bossRoomPos;
    public GameObject bossPrefab;
    public Vector2 bossPos;
    public MusicEntry[] bossMusics;

    public async override Task CheckCompletion()
    {
        // wait until all regular mobs are killed
        while (GameManager.Instance.MobCount > 0)
            await Task.Yield();

        // await Notification.Instance.ShowNotification($"You will be teleported to the boss room in {intermission} seconds");
        await HUD.Instance.ShowHugeMessageAsync("Intermission", $"Teleporting in {intermission} seconds", 2.5f);
        await Task.Delay((int)intermission * 1000);
        await FightBoss();

        if (portalPrefab)
            SpawnPortal();
    }

    protected async Task FightBoss()
    {
        // teleport player
        AudioManager.Instance.StopMusic();
        await LoadingScreen.Instance.FadeInAsync();
        GameObject.FindGameObjectWithTag("Player").transform.position = bossRoomPos;
        await LoadingScreen.Instance.FadeOutAsync();
        AudioManager.Instance.SetMusics(bossMusics);
        AudioManager.Instance.PlayMusic();

        // spawn boss
        if (bossPrefab)
        {
            await HUD.Instance.ShowHugeMessageAsync("Boss", Color.magenta, "Battle", Color.white);
            Instantiate(bossPrefab, bossPos, Quaternion.identity);

            // wait for boss to spawn
            while (GameManager.Instance.MobCount <= 0)
                await Task.Yield();
        }
        else
        {
            Debug.LogError("[BossDungeonMapData]: no boss found");
            return;
        }

        // wait until all mobs are killed
        while (GameManager.Instance.MobCount > 0)
            await Task.Yield();
    }
}
