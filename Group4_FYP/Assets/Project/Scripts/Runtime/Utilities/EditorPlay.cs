using UnityEngine;

#if UNITY_EDITOR
using UnityEngine.SceneManagement;
using PathOfHero.Controllers;
using PathOfHero.PersistentData;
#endif

namespace PathOfHero.Utilities
{
    public class EditorPlay : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField]
        private HeroProfile m_RuntimeProfile;
        [SerializeField]
        private HeroProfile m_EditorProfile;
#endif

        private void Awake()
        {
#if UNITY_EDITOR
            LoadSceneAdditively(SceneController.k_ControllerSceneName);
#else
            Destroy(gameObject);
#endif
        }

#if UNITY_EDITOR
        private void Start()
        {
            if (m_RuntimeProfile != null && m_EditorProfile != null)
                m_RuntimeProfile.LoadFromAsset(m_EditorProfile);

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
