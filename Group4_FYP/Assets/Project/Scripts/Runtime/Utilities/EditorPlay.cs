using UnityEngine;

#if UNITY_EDITOR
using UnityEngine.SceneManagement;
using PathOfHero.Controllers;
using PathOfHero.Save;
#endif

namespace PathOfHero.Utilities
{
    public class EditorPlay : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField]
        private bool m_IsGameplay;

        [Header("Player Profile")]
        [SerializeField]
        private PlayerProfileController m_PlayerProfileController;

        [SerializeField]
        private PlayerProfile m_EditorProfile;
#endif

        private void Awake()
        {
#if UNITY_EDITOR
            m_PlayerProfileController.LoadFromObject(m_EditorProfile);
            LoadSceneAdditively(SceneController.k_ControllerSceneName);
            if (m_IsGameplay)
                LoadSceneAdditively(SceneController.k_GameplaySceneName);
#endif
            Destroy(gameObject);
        }

#if UNITY_EDITOR
        private void LoadSceneAdditively(string name)
        {
            var scene = SceneManager.GetSceneByName(name);
            if (scene.IsValid())
                return;

            SceneManager.LoadScene(name, LoadSceneMode.Additive);
        }
#endif
    }
}
