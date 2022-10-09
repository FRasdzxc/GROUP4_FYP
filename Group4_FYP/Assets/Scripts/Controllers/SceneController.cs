using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField] private GameObject canvasAcrossScenes;
    [SerializeField] private GameObject canvasAcrossScenesMask;

    public void ChangeScene(string sceneName)
    {
        //canvasAcrossScenesMask.GetComponent<RectTransform>().localScale
        SceneManager.LoadScene(sceneName);
    }

    private async void AnimateMask(float duration, Vector2 scale)
    {

    }
}
