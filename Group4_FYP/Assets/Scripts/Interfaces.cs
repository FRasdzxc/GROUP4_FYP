public interface IPanelConflictable
{
    public bool HideConflictingPanels();
    public bool IsPanelOverridable(); // used by another class
    public bool IsPanelActive(); // used by another class
}
