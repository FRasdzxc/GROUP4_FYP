using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;
using FYP;

public class SelectionPanel : Panel
{
    [SerializeField] private Text titleText;
    [SerializeField] private Text messageText;
    [SerializeField] private Transform contentTransform;
    [SerializeField] private GameObject buttonPrefab;
    //[SerializeField] private GameObject buttonsPanelNormal;
    //[SerializeField] private GameObject buttonsPanelImportant;
    //[SerializeField] private Button confirmButtonNormal;
    //[SerializeField] private Button confirmButtonImportant;
    [SerializeField] private GameMaps gameMaps;

    // these variables prevent AddListener() from piling up?
    private UnityAction confirmAction;
    private UnityAction cancelAction;

    private static SelectionPanel instance;
    public static SelectionPanel Instance
    {
        get
        {
            return instance;
        }
    }

    protected override void Awake()
    {
        base.Awake();

        if (!instance)
        {
            instance = this;
        }

        gameObject.SetActive(false);
        gameObject.GetComponent<CanvasGroup>().alpha = 0;
    }

    // private void Start()
    // {
    //     ShowSelectionPanel(SelectionType.Dungeon); // test
    // }

    //public void ShowConfirmationPanel(string title, string message, UnityAction confirmAction, bool isImportant = false)
    //{
    //    ShowConfirmationPanel(title, message, confirmAction, () => { _ = HideConfirmationPanel(); }, isImportant);
    //}

    //public async void ShowConfirmationPanel(string title, string message, UnityAction confirmAction, UnityAction cancelAction, bool isImportant = false)
    //{
    //    titleText.text = title;
    //    messageText.text = message;
    //    this.confirmAction = confirmAction;
    //    this.cancelAction = cancelAction;

    //    if (!isImportant)
    //    {
    //        buttonsPanelNormal.SetActive(true);
    //        buttonsPanelImportant.SetActive(false);
    //    }
    //    else
    //    {
    //        buttonsPanelNormal.SetActive(false);
    //        buttonsPanelImportant.SetActive(true);
    //    }

    //    gameObject.SetActive(true);
    //    await gameObject.GetComponent<CanvasGroup>().DOFade(1, 0.25f).SetEase(Ease.OutQuart).AsyncWaitForCompletion();

    //    ShowPanel();
    //}

    public void ShowSelectionPanel(SelectionType selectionType)
    {
        RefreshContents();

        switch (selectionType)
        {
            case SelectionType.Map:
                {
                    titleText.text = "Maps";
                    messageText.text = "Select a map from the list below...";

                    foreach (MapData mapData in gameMaps.maps)
                    {
                        GameObject button = Instantiate(buttonPrefab, contentTransform);
                            button.GetComponent<Button>().onClick.AddListener(delegate
                            {
                                ConfirmationPanel.Instance.ShowConfirmationPanel
                                (
                                    $"Enter {mapData.mapName}",
                                    $"Enter {mapData.mapName}?\n\nType: {mapData.mapType}\nDifficulty: {mapData.mapDifficulty}",
                                    () =>
                                    {
                                        GameManager.Instance.LoadMap(mapData.mapId);
                                        HidePanel();
                                    },
                                    true
                                );
                            });

                            Text buttonTitle = Common.RecursiveFindChild(button.transform, "Title").GetComponent<Text>();
                            buttonTitle.text = mapData.mapName;

                            Text buttonDescription = Common.RecursiveFindChild(button.transform, "Description").GetComponent<Text>();
                            buttonDescription.text = $"{mapData.mapType.ToString()} | {mapData.mapDifficulty.ToString()}";
                    }
                }
                break;
            case SelectionType.Dungeon:
                {
                    titleText.text = "Dungeons";
                    messageText.text = "Select a dungeon from the list below...";

                    foreach (MapData mapData in gameMaps.maps)
                    {
                        if (mapData.mapType == MapType.Dungeon)
                        {
                            GameObject button = Instantiate(buttonPrefab, contentTransform);
                            button.GetComponent<Button>().onClick.AddListener(delegate
                            {
                                ConfirmationPanel.Instance.ShowConfirmationPanel
                                (
                                    $"Enter {mapData.mapName}",
                                    $"Enter {mapData.mapName}?\n\n[!] Please note that you cannot save/quit in the middle of a dungeon battle! You will have to play through the whole dungeon. Upon death, you will lose all your progress in this dungeon!\n\nType: {mapData.mapType}\nDifficulty: {mapData.mapDifficulty}",
                                    () =>
                                    {
                                        GameManager.Instance.LoadMap(mapData.mapId);
                                        HidePanel();
                                    },
                                    true
                                );
                            });

                            Text buttonTitle = Common.RecursiveFindChild(button.transform, "Title").GetComponent<Text>();
                            buttonTitle.text = mapData.mapName;

                            Text buttonDescription = Common.RecursiveFindChild(button.transform, "Description").GetComponent<Text>();
                            buttonDescription.text = $"{mapData.mapType.ToString()} | {mapData.mapDifficulty.ToString()}";
                        }
                    }
                }
                break;
        }

        ShowPanel();
    }

    public void DungeonSelected(string name)
    {
        Debug.Log(name);
    }

    public async Task HideSelectionPanel()
    {
        await gameObject.GetComponent<CanvasGroup>().DOFade(0, 0.25f).SetEase(Ease.OutQuart).AsyncWaitForCompletion();
        gameObject.SetActive(false);
    }

    private void RefreshContents()
    {
        foreach (Transform child in contentTransform)
        {
            Destroy(child.gameObject);
        }
    }

    public override async void ShowPanel()
    {
        gameObject.SetActive(true);
        await gameObject.GetComponent<CanvasGroup>().DOFade(1, 0.25f).SetEase(Ease.OutQuart).AsyncWaitForCompletion();
        base.ShowPanel();
    }

    public override void HidePanel()
    {
        base.HidePanel();

        _ = HideSelectionPanel();
    }

    //public void Confirm()
    //{
    //    // await HideConfirmationPanel();
    //    HidePanel();
    //    confirmAction.Invoke();
    //}

    //public void Cancel()
    //{
    //    // await HideConfirmationPanel();
    //    HidePanel();
    //    cancelAction.Invoke();
    //}

    
}
