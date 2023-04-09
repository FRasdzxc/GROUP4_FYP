using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace PathOfHero.UI
{
    public class StartPanel : Panel
    {
        private IDisposable m_EventListener;
        private Coroutine m_SwitchCoroutine;

        private void OnEnable()
            => m_EventListener = InputSystem.onAnyButtonPress.Call(OnAnyKeyPressed);

        private void OnDisable()
            => m_EventListener.Dispose();

        private void OnAnyKeyPressed(InputControl button)
        {
            if (m_SwitchCoroutine != null)
                return;

            m_SwitchCoroutine = StartCoroutine(m_PanelManager.SwitchPanel(PanelType.Instruction));
        }
    }
}
