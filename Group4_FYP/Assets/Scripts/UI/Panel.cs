using UnityEngine;
using UnityEngine.InputSystem;
using PathOfHero.Gameplay;

public abstract class Panel : MonoBehaviour
{
    protected PlayerInput playerInput;
    protected bool allowHiding = true;

    protected virtual void Awake()
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

    public virtual void ShowPanel()
        => PanelManager.Instance.AddPanel(this);

    public virtual void HidePanel()
    {
        if (allowHiding)
            PanelManager.Instance.RemovePanel(this);
    }
}
