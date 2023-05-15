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
        }

        private void Update() // remove this after bug has been fixed
            => Debug.Log($"shownPanels.Count = {shownPanels.Count}; last = {shownPanels[shownPanels.Count - 1].GetType()}");

        private void OnEnable()
        {
            GameManager.onPlayerSetUp += SetUp;
            StartMenu.onPlayerSetUp += SetUp;
        }

        private void OnDisable()
        {
            GameManager.onPlayerSetUp -= SetUp;
            StartMenu.onPlayerSetUp -= SetUp;

            hidePanelAction.performed -= OnHidePanel;
        }

        private void OnHidePanel(InputAction.CallbackContext content)
        {
            if (content.performed && shownPanels.Count > 0)
            {
                //var index = shownPanels.Count - 1;
                //var panel = shownPanels[index];

                //if (!panel.GetAllowHiding() || !panel.GetIsOpened())
                //    return;

                //shownPanels.RemoveAt(index);
                //panel.HidePanel();

                if (shownPanels[shownPanels.Count - 1].GetAllowHiding() && shownPanels[shownPanels.Count - 1].GetIsOpened())
                    shownPanels[shownPanels.Count - 1].HidePanel();
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

            if (HUD.Instance)
                HUD.Instance.HideHUDMain();

            SelectCurrentActionMap();
        }

        public void RemovePanel(Panel panel)
        {
            shownPanels.Remove(panel);

            if (shownPanels.Count <= 0 && HUD.Instance)
                HUD.Instance.ShowHUDMain();

            SelectCurrentActionMap();
        }

        private void SetUp()
        {
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
            hidePanelAction.performed += OnHidePanel;

            Debug.Log(uiActionMap);

            SelectCurrentActionMap();
        }
    }
}
