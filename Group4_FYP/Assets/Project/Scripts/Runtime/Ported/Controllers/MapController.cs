using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using PathOfHero.UI;
using PathOfHero.Managers.Data;

public class MapController : MonoBehaviour
{
    [SerializeField]
    protected MapData mapData;

    [SerializeField]
    protected ScoreEventChannel m_ScoreEventChannel;

    // Start is called before the first frame update
    protected async virtual void Start()
    {
        // fade out loading screen
        await Task.Delay(50); // make the game look smoother
        await LoadingScreen.Instance.FadeOutAsync();

        // setting up pausemenu mode
        if (mapData.mapType == MapType.Dungeon)
        {
            m_ScoreEventChannel.OnLevelStart();
            PauseMenu.Instance.SetDungeonMode(true);
            SaveSystem.Instance.SaveData(false, false);
        }
        else
            PauseMenu.Instance.SetDungeonMode(false);

        // set objective
        if (mapData.objective.Length > 0) // if objective is not empty
            HUD.Instance.ShowObjective(mapData.objective);
        else
            HUD.Instance.HideObjective();

        // set music
        AudioManager.Instance.SetMusics(mapData.musics);
        AudioManager.Instance.PlayMusic();

        // show huge message
        await ShowMapMessage();
        StartCoroutine(CheckCompletion());
    }

    // Update is called once per frame
    protected virtual void Update() { }

    protected async virtual Task ShowMapMessage()
    {
        if (mapData.mapType == MapType.Peaceful)
            await HUD.Instance.ShowHugeMessageAsync(mapData.mapName, mapData.mapType.ToString());
        else
            await HUD.Instance.ShowHugeMessageAsync(mapData.mapName, $"{mapData.mapType} / {mapData.mapDifficulty}");
    }

    protected virtual IEnumerator CheckCompletion()
    {
        yield return null;
    }

    protected virtual void OnDisable()
        => StopCoroutine(CheckCompletion());
}
