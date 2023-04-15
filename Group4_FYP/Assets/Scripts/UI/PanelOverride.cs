using System;
using UnityEngine;
using PathOfHero.Gameplay;

public abstract class PanelOverride : Panel
{
    [Serializable]
    public struct OverridingPanels
    {
        [Tooltip("Force closes these panels when this panel shows up.")]
        public PanelOverride[] overrides;
        [Tooltip("Prevent this panel from showing up when these panels are showing.")]
        public PanelOverride[] blockedBy;
    }

    [SerializeField] protected OverridingPanels overridingPanels;

    protected bool OverridePanel()
    {
        // check if panel can show up
        foreach (PanelOverride panel in overridingPanels.blockedBy)
        {
            if (panel.GetPanel().activeSelf)
                return false;
        }

        // hide panels
        foreach (PanelOverride panel in overridingPanels.overrides)
        {
            if (panel.GetPanel().activeSelf)
                panel.HidePanel();
        }

        return true;
    }

    protected abstract GameObject GetPanel();
}
