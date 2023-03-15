using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Panel : MonoBehaviour
{
    protected PlayerInput playerInput;
    // protected InputAction hidePanelAction;

    // public HideEvent OnHide = new HideEvent(() => { });

    // public delegate void HideEvent();

    protected virtual void Awake()
    {
        playerInput = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();
        // playerInput.currentActionMap = playerInput.actions.FindActionMap("Gameplay");
    }

    // // Start is called before the first frame update
    // protected virtual void Start()
    // {
    //     hidePanelAction = playerInput.actions["HidePanel"];
    //     hidePanelAction.Enable();
    //     Debug.Log(hidePanelAction.GetBindingDisplayString());
    // }

    // // Update is called once per frame
    // protected virtual void Update()
    // {
    //     if (hidePanelAction.triggered)
    //     {
    //         HidePanel();
    //         Debug.Log("hidepanelactiontriggered");
    //     }

    //     Debug.Log(playerInput.currentActionMap);
    // }

    public virtual void ShowPanel()
    {
        // playerInput.currentActionMap = playerInput.actions.FindActionMap("UI");
        PanelManager.Instance.AddPanel(this);
    }

    public virtual void HidePanel()
    {
        // playerInput.currentActionMap = playerInput.actions.FindActionMap("Gameplay");
        // OnHide?.Invoke();
        PanelManager.Instance.RemovePanel(this);
    }
}
