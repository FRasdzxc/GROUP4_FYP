// GENERATED AUTOMATICALLY FROM 'Assets/InputActions.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @InputActions : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputActions"",
    ""maps"": [
        {
            ""name"": ""UI"",
            ""id"": ""bf304a28-8b40-4935-8ed4-2145a71c5910"",
            ""actions"": [
                {
                    ""name"": ""TogglePause"",
                    ""type"": ""Button"",
                    ""id"": ""079673b4-71f3-4c65-9016-76d72ddd2ffa"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ToggleInventory"",
                    ""type"": ""Button"",
                    ""id"": ""a6e93d45-1aa4-4446-af71-49d07dd17778"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""59dcd6ab-f903-44f8-bfd1-c6eec7d8ff7a"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardAndMouse"",
                    ""action"": ""TogglePause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f044adbb-a706-48f6-a698-8dce1058265f"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardAndMouse"",
                    ""action"": ""ToggleInventory"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Gameplay"",
            ""id"": ""0ea1d748-92df-486d-85a8-08211a48fe31"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""0eabcbf6-ea03-4dd7-a178-5dac88d6a257"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Sprint"",
                    ""type"": ""Button"",
                    ""id"": ""49dcf7b1-c014-4f0c-b396-d87b309609b4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Ability1"",
                    ""type"": ""Button"",
                    ""id"": ""a7f1e384-02a3-4aac-b571-94251fd7f5a2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Ability2"",
                    ""type"": ""Button"",
                    ""id"": ""2655efdd-6f0e-4349-9f47-0b82b3f490ad"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Ability3"",
                    ""type"": ""Button"",
                    ""id"": ""b53e4770-3075-4496-a25b-37513352acdf"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""AbilityU"",
                    ""type"": ""Button"",
                    ""id"": ""3cc047c7-1e64-4c97-ad0b-d00b6110ca5d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Interact1"",
                    ""type"": ""Button"",
                    ""id"": ""53b67cd5-f70c-42ad-85c0-8e292059cf0a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Interact2"",
                    ""type"": ""Button"",
                    ""id"": ""96b2d38a-dd0e-4ac6-b076-06973e944311"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Keyboard Arrows"",
                    ""id"": ""9bec042e-5275-4c41-89d1-879ca869ba4a"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""782ce819-e2d0-4153-92b9-2a76075985cc"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardAndMouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""367c84b2-4e21-4d70-bfd4-d8b546610dee"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardAndMouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""0d5c7ddd-aa12-47cc-9f37-e64cc6949816"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardAndMouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""bb7968a6-eec0-4811-8e52-29e42ed63a53"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardAndMouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Keyboard WASD"",
                    ""id"": ""77b8b4ed-6113-403d-b5c0-40c15de9d13a"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""34eff479-f45e-45b1-ba18-85f7ea3802ab"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardAndMouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""78c9140f-bb78-4f30-950a-c25e643b1afa"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardAndMouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""43e8443f-c4a7-4bc0-a470-3306daed7a7e"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardAndMouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""b1b894ea-36e0-45bb-a500-a281aed0470d"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardAndMouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""c9e33fb8-927f-466f-a84b-bbcfab1b33a4"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""caa78328-1f73-4ac2-b150-fe4b46b234a6"",
                    ""path"": ""<Gamepad>/dpad"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""022d1187-48a0-45c8-a00f-96227b1ae9e3"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardAndMouse"",
                    ""action"": ""Sprint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8f6aaff9-c7ad-4b10-9cf2-9428b00b9a4a"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Sprint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""dc16cae2-f4d6-48b2-84a8-9fc84a051680"",
                    ""path"": ""<Keyboard>/z"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardAndMouse"",
                    ""action"": ""Ability1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""64c2d4de-0f56-4910-a3b3-3b87c12e715f"",
                    ""path"": ""<Keyboard>/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardAndMouse"",
                    ""action"": ""Ability2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""eaf1b83d-6e49-4bf3-9d92-d5a09ba73994"",
                    ""path"": ""<Keyboard>/c"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardAndMouse"",
                    ""action"": ""Ability3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4290be86-20dc-4bd9-b87d-a9f4d1880145"",
                    ""path"": ""<Keyboard>/v"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardAndMouse"",
                    ""action"": ""AbilityU"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fd5fefbc-eab8-4061-8969-c6688fe1a817"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardAndMouse"",
                    ""action"": ""Interact1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bbbecca1-ecd2-4af3-b6d1-60b0ea52110b"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardAndMouse"",
                    ""action"": ""Interact2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""KeyboardAndMouse"",
            ""bindingGroup"": ""KeyboardAndMouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // UI
        m_UI = asset.FindActionMap("UI", throwIfNotFound: true);
        m_UI_TogglePause = m_UI.FindAction("TogglePause", throwIfNotFound: true);
        m_UI_ToggleInventory = m_UI.FindAction("ToggleInventory", throwIfNotFound: true);
        // Gameplay
        m_Gameplay = asset.FindActionMap("Gameplay", throwIfNotFound: true);
        m_Gameplay_Move = m_Gameplay.FindAction("Move", throwIfNotFound: true);
        m_Gameplay_Sprint = m_Gameplay.FindAction("Sprint", throwIfNotFound: true);
        m_Gameplay_Ability1 = m_Gameplay.FindAction("Ability1", throwIfNotFound: true);
        m_Gameplay_Ability2 = m_Gameplay.FindAction("Ability2", throwIfNotFound: true);
        m_Gameplay_Ability3 = m_Gameplay.FindAction("Ability3", throwIfNotFound: true);
        m_Gameplay_AbilityU = m_Gameplay.FindAction("AbilityU", throwIfNotFound: true);
        m_Gameplay_Interact1 = m_Gameplay.FindAction("Interact1", throwIfNotFound: true);
        m_Gameplay_Interact2 = m_Gameplay.FindAction("Interact2", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // UI
    private readonly InputActionMap m_UI;
    private IUIActions m_UIActionsCallbackInterface;
    private readonly InputAction m_UI_TogglePause;
    private readonly InputAction m_UI_ToggleInventory;
    public struct UIActions
    {
        private @InputActions m_Wrapper;
        public UIActions(@InputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @TogglePause => m_Wrapper.m_UI_TogglePause;
        public InputAction @ToggleInventory => m_Wrapper.m_UI_ToggleInventory;
        public InputActionMap Get() { return m_Wrapper.m_UI; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(UIActions set) { return set.Get(); }
        public void SetCallbacks(IUIActions instance)
        {
            if (m_Wrapper.m_UIActionsCallbackInterface != null)
            {
                @TogglePause.started -= m_Wrapper.m_UIActionsCallbackInterface.OnTogglePause;
                @TogglePause.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnTogglePause;
                @TogglePause.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnTogglePause;
                @ToggleInventory.started -= m_Wrapper.m_UIActionsCallbackInterface.OnToggleInventory;
                @ToggleInventory.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnToggleInventory;
                @ToggleInventory.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnToggleInventory;
            }
            m_Wrapper.m_UIActionsCallbackInterface = instance;
            if (instance != null)
            {
                @TogglePause.started += instance.OnTogglePause;
                @TogglePause.performed += instance.OnTogglePause;
                @TogglePause.canceled += instance.OnTogglePause;
                @ToggleInventory.started += instance.OnToggleInventory;
                @ToggleInventory.performed += instance.OnToggleInventory;
                @ToggleInventory.canceled += instance.OnToggleInventory;
            }
        }
    }
    public UIActions @UI => new UIActions(this);

    // Gameplay
    private readonly InputActionMap m_Gameplay;
    private IGameplayActions m_GameplayActionsCallbackInterface;
    private readonly InputAction m_Gameplay_Move;
    private readonly InputAction m_Gameplay_Sprint;
    private readonly InputAction m_Gameplay_Ability1;
    private readonly InputAction m_Gameplay_Ability2;
    private readonly InputAction m_Gameplay_Ability3;
    private readonly InputAction m_Gameplay_AbilityU;
    private readonly InputAction m_Gameplay_Interact1;
    private readonly InputAction m_Gameplay_Interact2;
    public struct GameplayActions
    {
        private @InputActions m_Wrapper;
        public GameplayActions(@InputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Gameplay_Move;
        public InputAction @Sprint => m_Wrapper.m_Gameplay_Sprint;
        public InputAction @Ability1 => m_Wrapper.m_Gameplay_Ability1;
        public InputAction @Ability2 => m_Wrapper.m_Gameplay_Ability2;
        public InputAction @Ability3 => m_Wrapper.m_Gameplay_Ability3;
        public InputAction @AbilityU => m_Wrapper.m_Gameplay_AbilityU;
        public InputAction @Interact1 => m_Wrapper.m_Gameplay_Interact1;
        public InputAction @Interact2 => m_Wrapper.m_Gameplay_Interact2;
        public InputActionMap Get() { return m_Wrapper.m_Gameplay; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameplayActions set) { return set.Get(); }
        public void SetCallbacks(IGameplayActions instance)
        {
            if (m_Wrapper.m_GameplayActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMove;
                @Sprint.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSprint;
                @Sprint.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSprint;
                @Sprint.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSprint;
                @Ability1.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnAbility1;
                @Ability1.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnAbility1;
                @Ability1.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnAbility1;
                @Ability2.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnAbility2;
                @Ability2.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnAbility2;
                @Ability2.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnAbility2;
                @Ability3.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnAbility3;
                @Ability3.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnAbility3;
                @Ability3.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnAbility3;
                @AbilityU.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnAbilityU;
                @AbilityU.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnAbilityU;
                @AbilityU.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnAbilityU;
                @Interact1.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnInteract1;
                @Interact1.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnInteract1;
                @Interact1.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnInteract1;
                @Interact2.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnInteract2;
                @Interact2.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnInteract2;
                @Interact2.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnInteract2;
            }
            m_Wrapper.m_GameplayActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Sprint.started += instance.OnSprint;
                @Sprint.performed += instance.OnSprint;
                @Sprint.canceled += instance.OnSprint;
                @Ability1.started += instance.OnAbility1;
                @Ability1.performed += instance.OnAbility1;
                @Ability1.canceled += instance.OnAbility1;
                @Ability2.started += instance.OnAbility2;
                @Ability2.performed += instance.OnAbility2;
                @Ability2.canceled += instance.OnAbility2;
                @Ability3.started += instance.OnAbility3;
                @Ability3.performed += instance.OnAbility3;
                @Ability3.canceled += instance.OnAbility3;
                @AbilityU.started += instance.OnAbilityU;
                @AbilityU.performed += instance.OnAbilityU;
                @AbilityU.canceled += instance.OnAbilityU;
                @Interact1.started += instance.OnInteract1;
                @Interact1.performed += instance.OnInteract1;
                @Interact1.canceled += instance.OnInteract1;
                @Interact2.started += instance.OnInteract2;
                @Interact2.performed += instance.OnInteract2;
                @Interact2.canceled += instance.OnInteract2;
            }
        }
    }
    public GameplayActions @Gameplay => new GameplayActions(this);
    private int m_KeyboardAndMouseSchemeIndex = -1;
    public InputControlScheme KeyboardAndMouseScheme
    {
        get
        {
            if (m_KeyboardAndMouseSchemeIndex == -1) m_KeyboardAndMouseSchemeIndex = asset.FindControlSchemeIndex("KeyboardAndMouse");
            return asset.controlSchemes[m_KeyboardAndMouseSchemeIndex];
        }
    }
    private int m_GamepadSchemeIndex = -1;
    public InputControlScheme GamepadScheme
    {
        get
        {
            if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
            return asset.controlSchemes[m_GamepadSchemeIndex];
        }
    }
    public interface IUIActions
    {
        void OnTogglePause(InputAction.CallbackContext context);
        void OnToggleInventory(InputAction.CallbackContext context);
    }
    public interface IGameplayActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnSprint(InputAction.CallbackContext context);
        void OnAbility1(InputAction.CallbackContext context);
        void OnAbility2(InputAction.CallbackContext context);
        void OnAbility3(InputAction.CallbackContext context);
        void OnAbilityU(InputAction.CallbackContext context);
        void OnInteract1(InputAction.CallbackContext context);
        void OnInteract2(InputAction.CallbackContext context);
    }
}
