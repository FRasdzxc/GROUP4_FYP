using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class TabEntry
{
    public Button tabButton;
    public GameObject targetTab;
}

public class TabPanel : MonoBehaviour
{
    [SerializeField]
    private TabEntry[] tabEntries;

    [SerializeField]
    private TabEntry defaultTab;

    [SerializeField]
    private bool showDefaultTabOnEnable = true;

    // Start is called before the first frame update
    void Start()
    {
        foreach (TabEntry te in tabEntries)
            te.tabButton.onClick.AddListener(() => ShowTab(te));
    }

    void OnEnable()
    {
        if (showDefaultTabOnEnable)
            ShowTab(defaultTab);
    }

    public void ShowTab(TabEntry tabEntry) 
    {
        foreach (TabEntry te in tabEntries)
        {
            te.tabButton.interactable = true;
            te.targetTab.SetActive(false);
        }
        
        tabEntry.tabButton.interactable = false;
        tabEntry.targetTab.SetActive(true);
    }
}
