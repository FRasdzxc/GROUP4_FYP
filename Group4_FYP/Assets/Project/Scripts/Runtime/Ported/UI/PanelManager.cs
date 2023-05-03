using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using PathOfHero.Utilities;

namespace PathOfHero.Gameplay
{
    public class PanelManager : Singleton<PanelManager>
    {
        private PlayerInput playerInput;
        private InputActionMap gameplayActionMap;
        private InputActionMap uiActionMap;
        private InputAction hidePanelAction;

        private List<Panel> shownPanels;

        protected override void Awake()
        {
            base.Awake();
            shownPanels = new();

            var player = GameObject.FindGameObjectWithTag("Player");
            if (player == null)
            {
                Debug.LogWarning("[Panel Manager] GameObject 'Player' not found.");
                return;
            }

            if (!player.TryGetComponent(out playerInput))
            {
                Debug.LogWarning("[Panel Manager] Component 'Player Input' not found.");
                return;
            }

            gameplayActionMap = playerInput.actions.FindActionMap("Gameplay", throwIfNotFound: true);
            uiActionMap = playerInput.actions.FindActionMap("UI", throwIfNotFound: true);
            hidePanelAction = uiActionMap.FindAction("HidePanel", throwIfNotFound: true);
        }

        private void Start()
            => SelectCurrentActionMap();

        private void OnEnable()
            => hidePanelAction.performed += OnHidePanel;

        private void OnDisable()
            => hidePanelAction.performed -= OnHidePanel;

        private void OnHidePanel(InputAction.CallbackContext content)
        {
            if (content.performed && shownPanels.Count > 0)
            {
                var index = shownPanels.Count - 1;
                var panel = shownPanels[index];

                if (!panel.GetAllowHiding())
                    return;

                shownPanels.RemoveAt(index);
                panel.HidePanel();
            }
        }

        private void SelectCurrentActionMap()
        {
            uiActionMap.Disable();
            gameplayActionMap.Disable();

            playerInput.currentActionMap = (shownPanels.Count > 0) ? uiActionMap : gameplayActionMap;
            playerInput.currentActionMap.Enable();
        }

        public void AddPanel(Panel panel)
        {
            if (!shownPanels.Contains(panel))
                shownPanels.Add(panel);

            SelectCurrentActionMap();

            if (HUD.Instance)
                HUD.Instance.HideHUDMain();
        }

        public void RemovePanel(Panel panel)
        {
            shownPanels.Remove(panel);
            SelectCurrentActionMap();

            if (shownPanels.Count <= 0 && HUD.Instance)
                HUD.Instance.ShowHUDMain();
        }
    }
}
