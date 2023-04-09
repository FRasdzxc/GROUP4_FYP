using UnityEngine;
using UnityEngine.SceneManagement;
using PathOfHero.Controllers;
using DG.Tweening;

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
            DOTween.Init().SetCapacity(500, 50);
            var load = SceneManager.LoadSceneAsync(SceneController.k_ControllerSceneName, LoadSceneMode.Additive);
            load.completed += OnLoadComplete;
        }

        private void OnLoadComplete(AsyncOperation handle)
        {
            var sceneController = SceneController.Instance;
            if (sceneController == null)
            {
                Debug.LogError("[Bootstrap] Scene controller missing! Unable to continue.");
                return;
            }

            sceneController.ChangeScene(m_SceneNameToLoad, m_IsGameplayScene, true);
        }
    }
}
