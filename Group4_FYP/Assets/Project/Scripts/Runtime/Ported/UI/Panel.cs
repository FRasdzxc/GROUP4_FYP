using UnityEngine;
using UnityEngine.InputSystem;
using PathOfHero.Gameplay;

public abstract class Panel : MonoBehaviour
{
    protected PlayerInput playerInput;
    protected bool allowHiding = true;
    protected bool isOpened;

    protected virtual void Awake() { }

    public virtual void ShowPanel()
    {
        if (!isOpened)
            PanelManager.Instance.AddPanel(this);
    }

    public virtual void HidePanel()
    {
        if (isOpened && allowHiding)
            PanelManager.Instance.RemovePanel(this);
    }

    public bool GetAllowHiding()
        => allowHiding;

    public bool GetIsOpened()
        => isOpened;

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
