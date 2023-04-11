using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using PathOfHero.UI;
using PathOfHero.Utilities;

namespace PathOfHero.Controllers
{
    public class SceneController : SingletonPersistent<SceneController>
    {
        public const string k_ControllerSceneName = "Controllers";
        public const string k_InGameSceneName = "InGameScene";
        public const string k_EndSceneName = "EndScene";

        private bool m_IsLoading;
        private string m_SceneToLoad;
        private string m_CurrentScene;

        public bool IsLoading => m_IsLoading;

        private void Start()
        {
            m_CurrentScene = SceneManager.GetActiveScene().name;
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        public void ChangeScene(string name, bool isGameplay, bool skipFadeIn = false)
        {
            LoadingScreen.Instance.DisplayLogo = true;
            StartCoroutine(SwitchScene(name, isGameplay, skipFadeIn));
        }

        private IEnumerator SwitchScene(string name, bool isGameplay, bool skipFadeIn = false)
        {
            if (m_IsLoading)
                yield break;

            if (m_CurrentScene == name)
                yield break;

            m_IsLoading = true;
            m_SceneToLoad = name;
            yield return StartCoroutine(LoadingScreen.Instance.FadeIn(!skipFadeIn));

            // Wait until engine actually switched active scene
            var load = SceneManager.LoadSceneAsync(name);
            do
                yield return null;
            while (load.isDone);
            m_IsLoading = false;

            if (!isGameplay)
                yield return LoadingScreen.Instance.FadeOut();
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == m_SceneToLoad)
                SceneManager.SetActiveScene(scene);
        }
    }
}
