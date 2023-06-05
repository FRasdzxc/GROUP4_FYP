using UnityEngine;
using PathOfHero.Gameplay;

public abstract class Panel : MonoBehaviour
{
    [SerializeField]
    protected AudioClip[] panelSounds;

    protected bool allowsHiding = true;
    protected PanelState panelState = PanelState.Hidden;

    public bool HidingAllowed => allowsHiding;
    public PanelState PanelState => panelState;

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
}
