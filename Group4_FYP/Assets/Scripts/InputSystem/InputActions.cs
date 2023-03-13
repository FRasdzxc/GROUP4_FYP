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
                    ""name"": ""HidePanel"",
                    ""type"": ""Button"",
                    ""id"": ""079673b4-71f3-4c65-9016-76d72ddd2ffa"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""UseItem"",
                    ""type"": ""Button"",
                    ""id"": ""465e1d71-26e5-4855-9ab4-5497f251485d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""UseAll"",
                    ""type"": ""Button"",
                    ""id"": ""7b6e75dc-fd50-4a8f-bcd7-b6e3eea21c31"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""NextDialogue"",
                    ""type"": ""Button"",
                    ""id"": ""fa7ac31b-5a01-41c8-bdc1-6692ef71c613"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SkipDialogue"",
                    ""type"": ""Button"",
                    ""id"": ""9c7cb3b5-c000-4d54-b1c2-41f2c91d13f3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""HideInventory"",
                    ""type"": ""Button"",
                    ""id"": ""8ed35440-1d44-4af8-a63b-8cdebb8883ce"",
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
                    ""action"": ""HidePanel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""84dd8f80-f7f7-4e26-986c-af56b05d91b0"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardAndMouse"",
                    ""action"": ""UseItem"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e114f25b-1e94-4a6e-9e43-3f09125b5d50"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardAndMouse"",
                    ""action"": ""UseAll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6290acc0-ab36-4b3e-b1be-08768a81574f"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardAndMouse"",
                    ""action"": ""NextDialogue"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8627fd10-2b3d-41ea-808e-9aedd67d97eb"",
                    ""path"": ""<Keyboard>/period"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardAndMouse"",
                    ""action"": ""SkipDialogue"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""afe90d89-0b2a-4e72-b986-2b0bae14ca50"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardAndMouse"",
                    ""action"": ""HideInventory"",
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
                },
                {
                    ""name"": ""ShowPause"",
                    ""type"": ""Button"",
                    ""id"": ""42ea2042-a9fe-466d-98d6-0faf0bc3a225"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ShowInventory"",
                    ""type"": ""Button"",
                    ""id"": ""4393868c-99da-4bd7-995a-ede52c584b57"",
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
                },
                {
                    ""name"": """",
                    ""id"": ""b6afce49-8310-46b8-830f-4cb60bb542ac"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardAndMouse"",
                    ""action"": ""ShowPause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a5e36de0-9150-411a-bdb5-666ef104d711"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardAndMouse"",
                    ""action"": ""ShowInventory"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Testing"",
            ""id"": ""70220496-189f-451f-b197-065346bd2eb6"",
            ""actions"": [
                {
                    ""name"": ""TakeDamage"",
                    ""type"": ""Button"",
                    ""id"": ""63c88859-6a6e-4c3e-9107-1f758875780b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Modifier"",
                    ""type"": ""Button"",
                    ""id"": ""374ab660-0dcd-4cd1-8aa0-c69a7c2fded9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""6a4214d0-17e7-446e-8e0e-8acb3eeb1e9a"",
                    ""path"": ""<Keyboard>/backspace"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""KeyboardAndMouse"",
                    ""action"": ""TakeDamage"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""23ca8311-ad8d-4234-8ee4-214377a84cbd"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardAndMouse"",
                    ""action"": ""Modifier"",
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
        m_UI_HidePanel = m_UI.FindAction("HidePanel", throwIfNotFound: true);
        m_UI_UseItem = m_UI.FindAction("UseItem", throwIfNotFound: true);
        m_UI_UseAll = m_UI.FindAction("UseAll", throwIfNotFound: true);
        m_UI_NextDialogue = m_UI.FindAction("NextDialogue", throwIfNotFound: true);
        m_UI_SkipDialogue = m_UI.FindAction("SkipDialogue", throwIfNotFound: true);
        m_UI_HideInventory = m_UI.FindAction("HideInventory", throwIfNotFound: true);
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
        m_Gameplay_ShowPause = m_Gameplay.FindAction("ShowPause", throwIfNotFound: true);
        m_Gameplay_ShowInventory = m_Gameplay.FindAction("ShowInventory", throwIfNotFound: true);
        // Testing
        m_Testing = asset.FindActionMap("Testing", throwIfNotFound: true);
        m_Testing_TakeDamage = m_Testing.FindAction("TakeDamage", throwIfNotFound: true);
        m_Testing_Modifier = m_Testing.FindAction("Modifier", throwIfNotFound: true);
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
    private readonly InputAction m_UI_HidePanel;
    private readonly InputAction m_UI_UseItem;
    private readonly InputAction m_UI_UseAll;
    private readonly InputAction m_UI_NextDialogue;
    private readonly InputAction m_UI_SkipDialogue;
    private readonly InputAction m_UI_HideInventory;
    public struct UIActions
    {
        private @InputActions m_Wrapper;
        public UIActions(@InputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @HidePanel => m_Wrapper.m_UI_HidePanel;
        public InputAction @UseItem => m_Wrapper.m_UI_UseItem;
        public InputAction @UseAll => m_Wrapper.m_UI_UseAll;
        public InputAction @NextDialogue => m_Wrapper.m_UI_NextDialogue;
        public InputAction @SkipDialogue => m_Wrapper.m_UI_SkipDialogue;
        public InputAction @HideInventory => m_Wrapper.m_UI_HideInventory;
        public InputActionMap Get() { return m_Wrapper.m_UI; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(UIActions set) { return set.Get(); }
        public void SetCallbacks(IUIActions instance)
        {
            if (m_Wrapper.m_UIActionsCallbackInterface != null)
            {
                @HidePanel.started -= m_Wrapper.m_UIActionsCallbackInterface.OnHidePanel;
                @HidePanel.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnHidePanel;
                @HidePanel.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnHidePanel;
                @UseItem.started -= m_Wrapper.m_UIActionsCallbackInterface.OnUseItem;
                @UseItem.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnUseItem;
                @UseItem.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnUseItem;
                @UseAll.started -= m_Wrapper.m_UIActionsCallbackInterface.OnUseAll;
                @UseAll.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnUseAll;
                @UseAll.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnUseAll;
                @NextDialogue.started -= m_Wrapper.m_UIActionsCallbackInterface.OnNextDialogue;
                @NextDialogue.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnNextDialogue;
                @NextDialogue.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnNextDialogue;
                @SkipDialogue.started -= m_Wrapper.m_UIActionsCallbackInterface.OnSkipDialogue;
                @SkipDialogue.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnSkipDialogue;
                @SkipDialogue.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnSkipDialogue;
                @HideInventory.started -= m_Wrapper.m_UIActionsCallbackInterface.OnHideInventory;
                @HideInventory.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnHideInventory;
                @HideInventory.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnHideInventory;
            }
            m_Wrapper.m_UIActionsCallbackInterface = instance;
            if (instance != null)
            {
                @HidePanel.started += instance.OnHidePanel;
                @HidePanel.performed += instance.OnHidePanel;
                @HidePanel.canceled += instance.OnHidePanel;
                @UseItem.started += instance.OnUseItem;
                @UseItem.performed += instance.OnUseItem;
                @UseItem.canceled += instance.OnUseItem;
                @UseAll.started += instance.OnUseAll;
                @UseAll.performed += instance.OnUseAll;
                @UseAll.canceled += instance.OnUseAll;
                @NextDialogue.started += instance.OnNextDialogue;
                @NextDialogue.performed += instance.OnNextDialogue;
                @NextDialogue.canceled += instance.OnNextDialogue;
                @SkipDialogue.started += instance.OnSkipDialogue;
                @SkipDialogue.performed += instance.OnSkipDialogue;
                @SkipDialogue.canceled += instance.OnSkipDialogue;
                @HideInventory.started += instance.OnHideInventory;
                @HideInventory.performed += instance.OnHideInventory;
                @HideInventory.canceled += instance.OnHideInventory;
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
    private readonly InputAction m_Gameplay_ShowPause;
    private readonly InputAction m_Gameplay_ShowInventory;
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
        public InputAction @ShowPause => m_Wrapper.m_Gameplay_ShowPause;
        public InputAction @ShowInventory => m_Wrapper.m_Gameplay_ShowInventory;
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
                @ShowPause.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnShowPause;
                @ShowPause.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnShowPause;
                @ShowPause.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnShowPause;
                @ShowInventory.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnShowInventory;
                @ShowInventory.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnShowInventory;
                @ShowInventory.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnShowInventory;
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
                @ShowPause.started += instance.OnShowPause;
                @ShowPause.performed += instance.OnShowPause;
                @ShowPause.canceled += instance.OnShowPause;
                @ShowInventory.started += instance.OnShowInventory;
                @ShowInventory.performed += instance.OnShowInventory;
                @ShowInventory.canceled += instance.OnShowInventory;
            }
        }
    }
    public GameplayActions @Gameplay => new GameplayActions(this);

    // Testing
    private readonly InputActionMap m_Testing;
    private ITestingActions m_TestingActionsCallbackInterface;
    private readonly InputAction m_Testing_TakeDamage;
    private readonly InputAction m_Testing_Modifier;
    public struct TestingActions
    {
        private @InputActions m_Wrapper;
        public TestingActions(@InputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @TakeDamage => m_Wrapper.m_Testing_TakeDamage;
        public InputAction @Modifier => m_Wrapper.m_Testing_Modifier;
        public InputActionMap Get() { return m_Wrapper.m_Testing; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(TestingActions set) { return set.Get(); }
        public void SetCallbacks(ITestingActions instance)
        {
            if (m_Wrapper.m_TestingActionsCallbackInterface != null)
            {
                @TakeDamage.started -= m_Wrapper.m_TestingActionsCallbackInterface.OnTakeDamage;
                @TakeDamage.performed -= m_Wrapper.m_TestingActionsCallbackInterface.OnTakeDamage;
                @TakeDamage.canceled -= m_Wrapper.m_TestingActionsCallbackInterface.OnTakeDamage;
                @Modifier.started -= m_Wrapper.m_TestingActionsCallbackInterface.OnModifier;
                @Modifier.performed -= m_Wrapper.m_TestingActionsCallbackInterface.OnModifier;
                @Modifier.canceled -= m_Wrapper.m_TestingActionsCallbackInterface.OnModifier;
            }
            m_Wrapper.m_TestingActionsCallbackInterface = instance;
            if (instance != null)
            {
                @TakeDamage.started += instance.OnTakeDamage;
                @TakeDamage.performed += instance.OnTakeDamage;
                @TakeDamage.canceled += instance.OnTakeDamage;
                @Modifier.started += instance.OnModifier;
                @Modifier.performed += instance.OnModifier;
                @Modifier.canceled += instance.OnModifier;
            }
        }
    }
    public TestingActions @Testing => new TestingActions(this);
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
        void OnHidePanel(InputAction.CallbackContext context);
        void OnUseItem(InputAction.CallbackContext context);
        void OnUseAll(InputAction.CallbackContext context);
        void OnNextDialogue(InputAction.CallbackContext context);
        void OnSkipDialogue(InputAction.CallbackContext context);
        void OnHideInventory(InputAction.CallbackContext context);
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
        void OnShowPause(InputAction.CallbackContext context);
        void OnShowInventory(InputAction.CallbackContext context);
    }
    public interface ITestingActions
    {
        void OnTakeDamage(InputAction.CallbackContext context);
        void OnModifier(InputAction.CallbackContext context);
    }
}
