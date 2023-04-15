using UnityEngine;

#if UNITY_EDITOR
using UnityEngine.SceneManagement;
using PathOfHero.Controllers;
using PathOfHero.Save;
using PathOfHero.Telemetry;
#endif

namespace PathOfHero.Utilities
{
    public class EditorPlay : MonoBehaviour
    {
#if UNITY_EDITOR
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
#else
            Destroy(gameObject);
#endif
        }

#if UNITY_EDITOR
        private void Start()
        {
            var dataCollector = DataCollector.Instance;
            if (dataCollector != null && dataCollector.CurrentStats == null)
                dataCollector.StartNewSession();

            Destroy(gameObject);
        }

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
