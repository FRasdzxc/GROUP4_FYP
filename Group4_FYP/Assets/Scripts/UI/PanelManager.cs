using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PanelManager : MonoBehaviour
{
    protected PlayerInput playerInput;
    protected InputAction hidePanelAction;

    public HideEvent OnHide = new HideEvent(() => { });

    public delegate void HideEvent();

    private List<Panel> shownPanels;

    private static PanelManager instance;
    public static PanelManager Instance {
        get => instance;
    }

    void Awake()
    {
        if (!instance)
        {
            instance = this;
        }

        shownPanels = new List<Panel>();

        playerInput = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();
        playerInput.currentActionMap = playerInput.actions.FindActionMap("Gameplay");
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        hidePanelAction = playerInput.actions["HidePanel"];
        hidePanelAction.Enable();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (hidePanelAction.triggered)
        {
            if (shownPanels.Count > 0)
            {
                shownPanels[shownPanels.Count - 1].HidePanel();
                Debug.Log("hidepanelactiontriggered");
            }
        }
    }

    public void AddPanel(Panel panel)
    {
        if (FindPanel(panel))
        {
            Debug.Log("panel is shown already");
        }
        else
        {
            shownPanels.Add(panel);
            playerInput.currentActionMap = playerInput.actions.FindActionMap("UI");
        }
    }

    public void RemovePanel(Panel panel)
    {
        if (FindPanel(panel))
        {
            shownPanels.Remove(panel);

            if (shownPanels.Count <= 0)
            {
                playerInput.currentActionMap = playerInput.actions.FindActionMap("Gameplay");
            }
        }
        else
        {
            Debug.LogError("could not find panel to remove");
        }
    }

    private bool FindPanel(Panel panel)
    {
        foreach (Panel p in shownPanels)
        {
            if (panel.name.Equals(p.name))
            {
                return true;
            }
        }

        return false;
    }
}
