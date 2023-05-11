using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "New Map Data", menuName = "Game/Map Data")]
public class MapData : ScriptableObject
{
    public string mapId;
    [Tooltip("Players cannot attack / take damage when Map Type is Peaceful")]
    public string mapName;
    public MapType mapType;
    public MapDifficulty mapDifficulty;
    [Tooltip("Leave blank if none.")] [TextArea(5, 5)]
    public string objective;
    public GameObject mapPrefab;
    public MusicEntry[] musics;

    protected bool isStopped;

    public async virtual void SetUp()
    {
        // setting up pausemenu mode
        if (mapType == MapType.Dungeon)
        {
            PauseMenu.Instance.SetDungeonMode(true);
            SaveSystem.Instance.SaveData(false, false);
        }
        else
        {
            PauseMenu.Instance.SetDungeonMode(false);
        }

        // set objective
        if (objective.Length > 0) // if objective is not empty
            HUD.Instance.ShowObjective(objective);
        else
            HUD.Instance.HideObjective();

        // set music
        AudioManager.Instance.SetMusics(musics);
        AudioManager.Instance.PlayMusic();
        
        // show huge message
        await ShowMapMessage();

        isStopped = false;
        await CheckCompletion();
    }

    public async virtual Task CheckCompletion() {}

    public async virtual Task ShowMapMessage()
    {
        if (mapType == MapType.Peaceful)
            await HUD.Instance.ShowHugeMessageAsync(mapName, mapType.ToString());
        else
            await HUD.Instance.ShowHugeMessageAsync(mapName, $"{mapType} / {mapDifficulty}");
    }

    public void Stop()
        => isStopped = true;
}
