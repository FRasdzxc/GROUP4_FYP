using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using PathOfHero.Controllers;

namespace PathOfHero.Utilities
{
    public class StandalonePlay : MonoBehaviour
    {
        [SerializeField]
        private string m_SceneNameToLoad;
        [SerializeField]
        private bool m_IsGameplayScene;

        private void Start()
        {
            DOTween.Init();
            var load = SceneManager.LoadSceneAsync(SceneController.k_ControllerSceneName, LoadSceneMode.Additive);
            load.completed += handle => StartCoroutine(OnLoadComplete(handle));
        }

        private IEnumerator OnLoadComplete(AsyncOperation handle)
        {
            var sceneController = SceneController.Instance;
            if (sceneController == null)
            {
                Debug.LogError("[Bootstrap] Scene controller missing! Unable to continue.");
                yield break;
            }

            sceneController.ChangeScene(m_SceneNameToLoad, m_IsGameplayScene, true);
        }
    }
}
