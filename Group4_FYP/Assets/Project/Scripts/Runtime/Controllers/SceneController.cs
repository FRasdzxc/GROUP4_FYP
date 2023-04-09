using System.Collections;
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
            yield return StartCoroutine(LoadingScreen.Instance.FadeIn(!skipFadeIn));
            yield return StartCoroutine(UnloadScene(activeScene.name));

            var gameplaySceneLoaded = IsSceneLoaded(k_GameplaySceneName);
            if (isGameplay && !gameplaySceneLoaded)
                yield return StartCoroutine(LoadScene(k_GameplaySceneName));
            else if (gameplaySceneLoaded)
                yield return StartCoroutine(UnloadScene(k_GameplaySceneName));

            yield return StartCoroutine(LoadScene(name));
            var scene = SceneManager.GetSceneByName(name);
            if (scene.IsValid())
                SceneManager.SetActiveScene(scene);

            yield return LoadingScreen.Instance.FadeOut();
            m_IsLoading = false;
        }

        private IEnumerator LoadScene(string name)
        {
            var load = SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
            while (!load.isDone)
                yield return null;
        }

        private IEnumerator UnloadScene(string name)
        {
            var unload = SceneManager.UnloadSceneAsync(name);
            while (!unload.isDone)
                yield return null;
        }

        private bool IsSceneLoaded(string name)
        {
            var scene = SceneManager.GetSceneByName(name);
            return scene.IsValid();
        }
    }
}
