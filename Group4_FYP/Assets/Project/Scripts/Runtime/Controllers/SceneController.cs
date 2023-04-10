using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using PathOfHero.UI;
using PathOfHero.Utilities;

namespace PathOfHero.Controllers
{
    public class SceneController : Singleton<SceneController>
    {
        public const string k_ControllerSceneName = "Controllers";
        public const string k_GameplaySceneName = "Gameplay";

        private bool m_IsLoading;
        private bool m_IsGameplaySceneLoaded;
        private string m_SceneToLoad;

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
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

            var activeScene = SceneManager.GetActiveScene();
            if (activeScene.name == name)
                yield break;

            m_IsLoading = true;
            m_SceneToLoad = name;
            yield return StartCoroutine(LoadingScreen.Instance.FadeIn(!skipFadeIn));
            yield return StartCoroutine(UnloadScene(activeScene.name));

            if (isGameplay && !m_IsGameplaySceneLoaded)
                yield return StartCoroutine(LoadScene(k_GameplaySceneName));
            else if (m_IsGameplaySceneLoaded)
                yield return StartCoroutine(UnloadScene(k_GameplaySceneName));

            yield return StartCoroutine(LoadScene(m_SceneToLoad));

            if (!isGameplay)
                yield return LoadingScreen.Instance.FadeOut();

            m_IsLoading = false;
        }

        private IEnumerator LoadScene(string name)
        {
            var operation = SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
            yield return new WaitUntil(() => operation.isDone);
        }

        private IEnumerator UnloadScene(string name)
        {
            var operation = SceneManager.UnloadSceneAsync(name);
            yield return new WaitUntil(() => operation.isDone);
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (mode != LoadSceneMode.Additive)
            {
                Debug.LogError("[Scene Controller] A scene is loaded in single mode, controller scene might be lost");
                return;
            }

            if (scene.name == k_GameplaySceneName)
                m_IsGameplaySceneLoaded = true;
            else if (scene.name == m_SceneToLoad)
                SceneManager.SetActiveScene(scene);
        }

        private void OnSceneUnloaded(Scene scene)
        {
            m_IsGameplaySceneLoaded = scene.name == k_GameplaySceneName;
        }
    }
}
