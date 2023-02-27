using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SceneController : MonoBehaviour
{
    private static GameObject m_Instance;
    public static GameObject Canvas
    {
        get
        {
            if (m_Instance == null)
                Debug.LogWarning("Canvas game object does not exist");

            return m_Instance;
        }
    }

    private MaskingCanvas maskingCanvas;

    private string selectedSceneName;

    void Awake()
    {
        Debug.Assert(m_Instance == null || m_Instance == this, "More than one SceneController exists, please double check");
        if (m_Instance == null)
            m_Instance = gameObject;

        maskingCanvas = GameObject.FindGameObjectWithTag("MaskingCanvas").GetComponent<MaskingCanvas>();
    }

    private void OnDestroy()
    {
        if (m_Instance == gameObject)
            m_Instance = null;
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
