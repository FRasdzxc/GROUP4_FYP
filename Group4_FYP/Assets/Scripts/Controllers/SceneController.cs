using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SceneController : MonoBehaviour
{
    private MaskingCanvas maskingCanvas;

    private string selectedSceneName;

    void Awake()
    {
        maskingCanvas = GameObject.FindGameObjectWithTag("MaskingCanvas").GetComponent<MaskingCanvas>();
    }

    public void SetScene(string sceneName) // temporary
    {
        selectedSceneName = sceneName;
    }

    public async void ChangeScene(string sceneName) // not used for now?; instead use EnterScene() in the future
    {
        await maskingCanvas.ShowMaskingCanvas(true);
        await WaitForSceneToLoad(sceneName);
        await maskingCanvas.ShowMaskingCanvas(false);
    }

    public async void EnterScene() // temporary; change to WaitForSceneToLoad("PlayScene") in the future; remove SceneSelected()? idk
    {
        if (SceneSelected())
        {
            await maskingCanvas.ShowMaskingCanvas(true);
            await WaitForSceneToLoad(selectedSceneName);
            await maskingCanvas.ShowMaskingCanvas(false);
        }
    }

    public bool SceneSelected() // temporary
    {
        return (selectedSceneName != null && selectedSceneName != "");
    }

    public async void EnterPlayScene() // newest, EnterScene() and SceneSelected() unused?
    {
        await maskingCanvas.ShowMaskingCanvas(true);
        await WaitForSceneToLoad("PlayScene");
        await maskingCanvas.ShowMaskingCanvas(false);
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
