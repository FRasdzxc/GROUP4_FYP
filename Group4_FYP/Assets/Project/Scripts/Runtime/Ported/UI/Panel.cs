using UnityEngine;
using UnityEngine.InputSystem;
using PathOfHero.Gameplay;

public abstract class Panel : MonoBehaviour
{
    [SerializeField]
    protected AudioClip[] panelSounds;

    protected PlayerInput playerInput;
    protected bool allowsHiding = true;
    // protected bool isOpened;
    protected PanelState panelState = PanelState.Hidden;

    protected virtual void Awake() { }

    public virtual void ShowPanel()
    {
        if (panelState.Equals(PanelState.Hidden))
        {
            panelState = PanelState.Showing;
            PanelManager.Instance.AddPanel(this);

            if (panelSounds.Length > 0)
                AudioManager.Instance.PlaySound(panelSounds[Random.Range(0, panelSounds.Length)]);
        }
    }

    public virtual void HidePanel()
    {
        if (panelState.Equals(PanelState.Shown) && allowsHiding)
        {
            panelState = PanelState.Hiding;
            PanelManager.Instance.RemovePanel(this);
            
            if (panelSounds.Length > 0)
                AudioManager.Instance.PlaySound(panelSounds[Random.Range(0, panelSounds.Length)]);
        }
    }

    public bool GetAllowsHiding()
        => allowsHiding;

    // public bool GetIsOpened()
    //     => isOpened;

    public PanelState GetPanelState()
        => panelState;

    void OnEnable()
        => GameManager.onPlayerSetUp += SetUp;

    void OnDisable()
        => GameManager.onPlayerSetUp -= SetUp;

    protected virtual void SetUp()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogWarning("[Panel] GameObject 'Player' not found.");
            return;
        }

        if (!player.TryGetComponent(out playerInput))
            Debug.LogWarning("[Panel] Component 'Player Input' not found.");
    }
}
