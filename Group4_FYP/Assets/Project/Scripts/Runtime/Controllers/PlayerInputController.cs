using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using PathOfHero.Input;
using PathOfHero.Telemetry;

#if UNITY_EDITOR
using System.IO;
#endif

namespace PathOfHero.Controllers
{
    [CreateAssetMenu(fileName = "PlayerInputController", menuName = "Path of Hero/Controller/Player Input")]
    public class PlayerInputController : ScriptableObject, GameInput.IUIActions, GameInput.IGameplayActions, GameInput.IDeveloperActions
    {
        public enum ActionType
        {
            UI,
            Gameplay
        }

        // Gameplay
        public event UnityAction<Vector2> MoveEvent = delegate { };
        public event UnityAction SprintEvent = delegate { };
        public event UnityAction SprintCanceledEvent = delegate { };

        private GameInput m_PlayerInput;

        private void OnEnable()
        {
            if (m_PlayerInput == null)
            {
                m_PlayerInput = new GameInput();
                m_PlayerInput.UI.SetCallbacks(this);
                m_PlayerInput.Gameplay.SetCallbacks(this);

#if UNITY_EDITOR
                // Developer cheats
                m_PlayerInput.Developer.SetCallbacks(this);
                m_PlayerInput.Developer.Enable();
#endif
            }
        }

        private void OnDisable()
            => DisableAllInput();

        public void EnableInput(ActionType type)
        {
            DisableAllInput();
            switch (type)
            {
                case ActionType.UI:
                    m_PlayerInput.UI.Enable();
                    break;
                case ActionType.Gameplay:
                    m_PlayerInput.Gameplay.Enable();
                    break;
            }
        }

        public void DisableAllInput()
        {
            if (m_PlayerInput == null)
                return;

            m_PlayerInput.UI.Disable();
            m_PlayerInput.Gameplay.Disable();
        }

        // Gameplay
        public void OnMove(InputAction.CallbackContext context)
            => MoveEvent?.Invoke(context.ReadValue<Vector2>());

        public void OnSprint(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                SprintEvent?.Invoke();
            else if (context.phase == InputActionPhase.Canceled)
                SprintCanceledEvent?.Invoke();
        }

        // Developer
        public void OnCaptureScreenshot(InputAction.CallbackContext context)
        {
#if UNITY_EDITOR
            if (context.performed)
            {
                var dir = Path.Combine(Environment.CurrentDirectory, "Screenshots");
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                int i = 0;
                string path;
                do
                {
                    i++;
                    path = Path.Combine(dir, $"Screenshot{i}.png");
                }
                while (File.Exists(path));

                ScreenCapture.CaptureScreenshot(path, 2);
                Debug.Log($"[Screenshot] Saved to {path}");
            }
#endif
        }

        public void OnSlowMotion(InputAction.CallbackContext context)
        {
#if UNITY_EDITOR
            if (context.performed)
                Time.timeScale = Time.timeScale != 0.5f ? 0.5f : 1f;
#endif
        }

        public void OnFastMotion(InputAction.CallbackContext context)
        {
#if UNITY_EDITOR
            if (context.performed)
                Time.timeScale = Time.timeScale != 2f ? 2f : 1f;
#endif
        }

        public void OnKillMobs(InputAction.CallbackContext context)
        {
#if UNITY_EDITOR
            if (context.performed)
            {
                var mobs = GameObject.FindGameObjectsWithTag("Mob");
                foreach (var mob in mobs)
                    Destroy(mob);
            }
#endif
        }

        public void OnEndDemo(InputAction.CallbackContext context)
        {
#if UNITY_EDITOR
            if (context.performed)
                DemoController.Instance?.EndDemo();
#endif
        }
    }
}
