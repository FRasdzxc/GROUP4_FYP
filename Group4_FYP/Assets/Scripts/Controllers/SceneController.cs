using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using PathOfHero.Utilities;

public class SceneController : Singleton<MonoBehaviour>
{
    private static GameObject m_Instance;

    private string selectedSceneName;

    public async void ChangeScene(string sceneName) // not used for now?; instead use EnterScene() in the future
    {
        await MaskingCanvas.Instance.ShowMaskingCanvas(true);
        await WaitForSceneToLoad(sceneName);
        await MaskingCanvas.Instance.ShowMaskingCanvas(false);
    }

    public async void EnterScene() // temporary; change to WaitForSceneToLoad("PlayScene") in the future; remove SceneSelected()? idk
    {
        if (SceneSelected())
        {
            await MaskingCanvas.Instance.ShowMaskingCanvas(true);
            await WaitForSceneToLoad(selectedSceneName);
            await MaskingCanvas.Instance.ShowMaskingCanvas(false);
        }
    }

    public bool SceneSelected() // temporary
    {
        return (selectedSceneName != null && selectedSceneName != "");
    }

    public async void EnterPlayScene() // newest, EnterScene() and SceneSelected() unused?
    {
        await MaskingCanvas.Instance.ShowMaskingCanvas(true);
        await WaitForSceneToLoad("PlayScene");
        // await maskingCanvas.ShowMaskingCanvas(false); // GameManager will do this operation
    }

    private async Task WaitForSceneToLoad(string sceneName)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncOperation.isDone)
        {
            await Task.Yield();
        }
    }
}
