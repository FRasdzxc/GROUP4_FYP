using System.Collections.Generic;
using UnityEngine;
using PathOfHero.Utilities;

namespace PathOfHero.Gameplay
{
    public class PanelManager : Singleton<PanelManager>
    {
        [SerializeField]
        private InputReader m_InputReader;

        private readonly List<Panel> m_ActivePanels = new();

        private void OnEnable()
        {
            m_InputReader.HidePanel += OnHidePanel;
        }

        private void OnDisable()
        {
            m_InputReader.HidePanel -= OnHidePanel;
        }

        private void OnHidePanel()
        {
            if (m_ActivePanels.Count == 0)
                return;

            var panel = m_ActivePanels[m_ActivePanels.Count - 1];
            if (panel.HidingAllowed && panel.PanelState == PanelState.Shown)
                panel.HidePanel();
        }

        public void AddPanel(Panel panel)
        {
            if (!m_ActivePanels.Contains(panel))
                m_ActivePanels.Add(panel);

            if (HUD.Instance)
                HUD.Instance.HideHUDMain();

            m_InputReader.EnableInput(InputReader.ActionMapType.UI);
        }

        public void RemovePanel(Panel panel)
        {
            m_ActivePanels.Remove(panel);
            if (m_ActivePanels.Count <= 0)
            {
                HUD.Instance?.ShowHUDMain();
                m_InputReader.EnableInput(InputReader.ActionMapType.Gameplay);
            }
        }
    }
}
