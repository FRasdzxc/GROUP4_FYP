using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SceneController : MonoBehaviour
{
    [SerializeField] private GameObject canvasAcrossScenes;
    [SerializeField] private GameObject canvasAcrossScenesMask;

    public async void ChangeScene(string sceneName)
    {
        canvasAcrossScenes.SetActive(true);
        await canvasAcrossScenesMask.GetComponent<RectTransform>().DOScale(new Vector2(0, 0), 0.5f).SetEase(Ease.OutQuad).AsyncWaitForCompletion();
        DontDestroyOnLoad(canvasAcrossScenes);
        await WaitForSceneToLoad(sceneName);
        await canvasAcrossScenesMask.GetComponent<RectTransform>().DOScale(new Vector2(1, 1), 0.5f).SetEase(Ease.InQuad).AsyncWaitForCompletion();
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
