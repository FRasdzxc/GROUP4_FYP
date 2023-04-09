using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using PathOfHero.Controllers;

namespace PathOfHero.UI
{
    public enum PanelType
    {
        Start,
        Instruction
    }

    public class PanelManager : MonoBehaviour
    {
        [Header("Panels")]
        [SerializeField]
        private PanelEntry[] m_Panels;
        [SerializeField]
        private float m_FadeDuration;

        [Header("Cursor")]
        [SerializeField]
        private CursorController m_CursorController;

        private PanelEntry m_Current;

        private IEnumerator Start()
        {
            foreach (var entry in m_Panels)
            {
                entry.panel.CanvasAlpha = 0f;
                entry.panel.gameObject.SetActive(false);
            }

            yield return SwitchPanel(PanelType.Start, false);
            m_CursorController.ChangeCursor(CursorController.CursorType.Default);
        }

        public IEnumerator SwitchPanel(PanelType type, bool animated = true)
        {
            if (m_Current != null && m_Current.panelType == type)
                yield break;

            var entry = m_Panels.First(p => p.panelType == type);
            if (entry == null)
            {
                Debug.LogWarning($"[Panel Manager] Missing panel entry for type '{type}'");
                yield break;
            }

            // Fade out current panel
            var previous = m_Current;
            if (previous != null)
            {
                if (animated)
                {
                    previous.panel.CanvasAlpha = 1f;
                    yield return DOTween.To(() => previous.panel.CanvasAlpha, v => previous.panel.CanvasAlpha = v, 0f, m_FadeDuration).WaitForCompletion();
                }
                else
                    previous.panel.CanvasAlpha = 0f;

                previous.panel.gameObject.SetActive(false);
            }

            // Fade in new panel
            entry.panel.gameObject.SetActive(true);
            if (animated)
            {
                entry.panel.CanvasAlpha = 0f;
                yield return DOTween.To(() => entry.panel.CanvasAlpha, v => entry.panel.CanvasAlpha = v, 1f, m_FadeDuration).WaitForCompletion();
            }
            else
                entry.panel.CanvasAlpha = 1f;

            m_Current = entry;
        }

        [Serializable]
        public class PanelEntry
        {
            public PanelType panelType;
            public Panel panel;
        }
    }
}
