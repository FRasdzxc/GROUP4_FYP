using UnityEngine;

namespace PathOfHero.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class Panel : MonoBehaviour
    {
        public float CanvasAlpha
        {
            get => m_CanvasGroup.alpha;
            set => m_CanvasGroup.alpha = value;
        }

        protected PanelManager m_PanelManager;
        protected CanvasGroup m_CanvasGroup;

        protected virtual void Awake()
        {
            m_PanelManager = GetComponentInParent<PanelManager>();
            if (m_PanelManager == null)
                Debug.LogWarning("[Panel] Panel manager is missing.");

            m_CanvasGroup = GetComponent<CanvasGroup>();
        }
    }
}
