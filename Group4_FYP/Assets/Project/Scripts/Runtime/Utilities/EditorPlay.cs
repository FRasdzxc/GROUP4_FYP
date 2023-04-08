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
        private string m_ControllerSceneName;

        [SerializeField]
        private string m_GameplaySceneName;

        [SerializeField]
        private bool m_IsGameplay;

        [Header("Player Profile")]
        [SerializeField]
        private PlayerProfileController m_PlayerProfileController;

        [SerializeField]
        private PlayerProfile m_EditorProfile;

        [Header("Cursor")]
        [SerializeField]
        private CursorController m_CursorController;
#endif

        private void Awake()
        {
#if UNITY_EDITOR
            m_PlayerProfileController.LoadFromObject(m_EditorProfile);
            m_CursorController.ChangeCursor(m_IsGameplay ? CursorController.CursorType.Crosshair : CursorController.CursorType.Default);
            LoadSceneAdditively(m_ControllerSceneName);
            if (m_IsGameplay)
                LoadSceneAdditively(m_GameplaySceneName);
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
