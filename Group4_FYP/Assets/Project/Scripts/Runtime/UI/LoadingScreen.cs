using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using PathOfHero.Utilities;

namespace PathOfHero.UI
{
    [RequireComponent(typeof(GraphicRaycaster))]
    public class LoadingScreen : Singleton<LoadingScreen>
    {
        [SerializeField]
        private Transform m_MaskTransform;

        [SerializeField]
        private Image m_Background;

        [SerializeField]
        private GameObject m_Logo;
        public bool DisplayLogo
        {
            get => m_Logo.activeSelf;
            set => m_Logo.SetActive(value);
        }

        [SerializeField]
        private float m_Duration;

        private GraphicRaycaster m_GraphicRaycaster;
        private bool m_Fading;

        protected override void Awake()
        {
            base.Awake();
            m_GraphicRaycaster = GetComponent<GraphicRaycaster>();
        }

        private void Start()
        {
            // Hide logo by default
            DisplayLogo = false;
        }

        public IEnumerator FadeIn(bool animated = true)
        {
            var task = PerformFadeAsync(false, animated);
            yield return new WaitUntil(() => task.IsCompleted);
        }
        public IEnumerator FadeOut(bool animated = true)
        {
            var task = PerformFadeAsync(true, animated);
            yield return new WaitUntil(() => task.IsCompleted);
        }

        public async Task PerformFadeAsync(bool fadeOut, bool animated = true)
        {
            if (m_Fading)
            {
                Debug.LogWarning("[Loading Screen] A fade is already in progress, skipping...");
                return;
            }

            m_GraphicRaycaster.enabled = !fadeOut;
            m_MaskTransform.localScale = fadeOut ? Vector3.zero : Vector3.one;
            m_Background.raycastTarget = !fadeOut;

            m_Fading = true;
            var newScale = fadeOut ? Vector3.one : Vector3.zero;
            var ease = fadeOut ? Ease.InQuart : Ease.OutQuart;
            if (animated)
                await m_MaskTransform.DOScale(newScale, m_Duration).SetEase(ease).AsyncWaitForCompletion();
            else
                m_MaskTransform.localScale = newScale;
            m_Fading = false;
        }
    }
}
