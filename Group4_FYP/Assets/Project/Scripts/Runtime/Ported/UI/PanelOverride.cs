using System;
using System.Threading.Tasks;
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

    protected bool CanShow()
    {
        // check if panel can show up
        foreach (PanelOverride panel in overridingPanels.blockedBy)
        {
            //if (panel.GetPanel().activeSelf)
            // if (panel.GetPanelGobj().activeSelf || panel.GetIsOpened())
            if (panel.GetPanelState().Equals(PanelState.Shown) || panel.GetPanelState().Equals(PanelState.Showing))
                return false;
        }

        OverridePanels();
        return true;
    }

    private async void OverridePanels()
    {
        // force hide other panels
        foreach (PanelOverride panel in overridingPanels.overrides)
        {
            //if (panel.GetPanel().activeSelf)
            // if (panel.GetPanelGobj().activeSelf || panel.GetIsOpened())
            //     panel.HidePanel();
            if (panel.GetPanelState().Equals(PanelState.Shown))
                panel.HidePanel();
            else if (panel.GetPanelState().Equals(PanelState.Showing))
            {
                Debug.Log("panel is still showing");
                while (!panel.GetPanelState().Equals(PanelState.Shown))
                    await Task.Yield();
                panel.HidePanel();
            }
        }
    }

    protected abstract GameObject GetPanelGobj();
}
