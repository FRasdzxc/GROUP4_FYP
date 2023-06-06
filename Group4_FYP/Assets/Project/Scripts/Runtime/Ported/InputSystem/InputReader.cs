using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputReader", menuName = "Path of Hero/Input Reader")]
public class InputReader : ScriptableObject, InputActions.IGameplayActions, InputActions.IUIActions
{
    // Gameplay
    public event UnityAction<Vector2> Move = delegate { };
    public event UnityAction Sprint = delegate { };
    public event UnityAction SprintCanceled = delegate { };
    public event UnityAction Ability1 = delegate { };
    public event UnityAction Ability2 = delegate { };
    public event UnityAction Ability3 = delegate { };
    public event UnityAction AbilityU = delegate { };
    public event UnityAction Interact1 = delegate { };
    public event UnityAction Interact2 = delegate { };
    public event UnityAction ShowPause = delegate { };
    public event UnityAction ShowInventory = delegate { };
    public event UnityAction EnterGame = delegate { };
    public event UnityAction<float> MoveSide = delegate { };
    public event UnityAction JumpSide = delegate { };
    public event UnityAction SwitchGravity = delegate { };
    public event UnityAction ToggleMinimap = delegate { };

    // UI
    public event UnityAction HidePanel = delegate { };
    public event UnityAction UseItem = delegate { };
    public event UnityAction UseAll = delegate { };
    public event UnityAction HideInventory = delegate { };
    public event UnityAction SaveGame = delegate { };
    public event UnityAction ExitToMenu = delegate { };
    public event UnityAction NextDialogue = delegate { };
    public event UnityAction SkipDialogue = delegate { };

    private InputActions m_InputActions;

    private void OnEnable()
    {
        if (m_InputActions == null)
        {
            m_InputActions = new InputActions();
            m_InputActions.Gameplay.SetCallbacks(this);
            m_InputActions.UI.SetCallbacks(this);
        }
    }

    private void OnDisable() => DisableAllInput();

    public void EnableInput(ActionMapType type)
    {
        DisableAllInput();
        switch (type)
        {
            case ActionMapType.Gameplay:
                m_InputActions.Gameplay.Enable();
                break;
            case ActionMapType.UI:
                m_InputActions.UI.Enable();
                break;
        }
    }

    public void DisableAllInput()
    {
        if (m_InputActions == null)
            return;

        m_InputActions.Gameplay.Disable();
        m_InputActions.UI.Disable();
    }

    private void GenericAction(InputAction.CallbackContext context, UnityAction action)
    {
        if (context.performed)
            action.Invoke();
    }

    #region Gameplay
    public void OnMove(InputAction.CallbackContext context)
        => Move?.Invoke(context.ReadValue<Vector2>());

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            Sprint?.Invoke();
        else if (context.phase == InputActionPhase.Canceled)
            SprintCanceled?.Invoke();
    }

    public void OnAbility1(InputAction.CallbackContext context)
        => GenericAction(context, Ability1);

    public void OnAbility2(InputAction.CallbackContext context)
        => GenericAction(context, Ability2);

    public void OnAbility3(InputAction.CallbackContext context)
        => GenericAction(context, Ability3);

    public void OnAbilityU(InputAction.CallbackContext context)
        => GenericAction(context, AbilityU);

    public void OnInteract1(InputAction.CallbackContext context)
        => GenericAction(context, Interact1);

    public void OnInteract2(InputAction.CallbackContext context)
        => GenericAction(context, Interact2);

    public void OnShowPause(InputAction.CallbackContext context)
        => GenericAction(context, ShowPause);

    public void OnShowInventory(InputAction.CallbackContext context)
        => GenericAction(context, ShowInventory);

    public void OnEnterGame(InputAction.CallbackContext context)
        => GenericAction(context, EnterGame);

    public void OnMoveSide(InputAction.CallbackContext context)
        => MoveSide?.Invoke(context.ReadValue<float>());

    public void OnJumpSide(InputAction.CallbackContext context)
        => GenericAction(context, JumpSide);

    public void OnSwitchGravity(InputAction.CallbackContext context)
        => GenericAction(context, SwitchGravity);

    public void OnToggleMinimap(InputAction.CallbackContext context)
        => GenericAction(context, ToggleMinimap);
    #endregion

    #region UI
    public void OnHidePanel(InputAction.CallbackContext context)
        => GenericAction(context, HidePanel);

    public void OnUseItem(InputAction.CallbackContext context)
        => GenericAction(context, UseItem);

    public void OnUseAll(InputAction.CallbackContext context)
        => GenericAction(context, UseAll);

    public void OnHideInventory(InputAction.CallbackContext context)
        => GenericAction(context, HideInventory);

    public void OnSaveGame(InputAction.CallbackContext context)
        => GenericAction(context, SaveGame);

    public void OnExitToMenu(InputAction.CallbackContext context)
        => GenericAction(context, ExitToMenu);

    public void OnNextDialogue(InputAction.CallbackContext context)
        => GenericAction(context, NextDialogue);

    public void OnSkipDialogue(InputAction.CallbackContext context)
        => GenericAction(context, SkipDialogue);
    #endregion

    public enum ActionMapType
    {
        Gameplay,
        UI
    }
}
