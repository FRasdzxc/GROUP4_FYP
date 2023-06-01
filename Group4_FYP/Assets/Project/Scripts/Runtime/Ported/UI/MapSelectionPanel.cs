using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;
using PathOfHero.Others;

public class MapSelectionPanel : Panel // maybe inherit SelectionPanel in the future? (not yet written)
{
    [SerializeField] private Text titleText;
    [SerializeField] private Text messageText;
    [SerializeField] private Transform contentTransform;
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private GameMaps gameMaps;

    // these variables prevent AddListener() from piling up?
    private UnityAction confirmAction;
    private UnityAction cancelAction;

    private static MapSelectionPanel instance;
    public static MapSelectionPanel Instance => instance;

    protected override void Awake()
    {
        base.Awake();

        if (!instance)
            instance = this;
    }

    private void Start()
    {
        gameObject.SetActive(false);
        gameObject.GetComponent<CanvasGroup>().alpha = 0;
    }

    public void ShowMapSelectionPanel(MapSelectionType mapSelectionType)
    {
        RefreshContents();

        switch (mapSelectionType)
        {
            case MapSelectionType.Map:
                {
                    titleText.text = "Maps";
                    messageText.text = "Select a map from the list below...";

                    foreach (MapData mapData in gameMaps.maps)
                    {
                        if (mapData.mapId == GameManager.Instance.MapId)
                            continue;

                        // set up confirmation panel when button is clicked
                        GameObject button = Instantiate(buttonPrefab, contentTransform);
                        if (mapData.mapType == MapType.Dungeon)
                        {
                            DungeonMapData dungeonMapData = mapData as DungeonMapData;

                            button.GetComponent<Button>().onClick.AddListener(delegate
                            {
                                ConfirmationPanel.Instance.ShowConfirmationPanel
                                (
                                    $"Enter <color={CustomColorStrings.green}>{dungeonMapData.mapName}</color>",
                                    $"<color=red>!!</color> You cannot save/quit in a dungeon battle! You must play through the whole dungeon.\n<color=red>!!</color> Upon death, you will lose all your progress in this dungeon!",
                                    () =>
                                    {
                                        GameManager.Instance.LoadMap(dungeonMapData.mapId);
                                        HidePanel();
                                    },
                                    false,
                                    true,
                                    $"<color={CustomColorStrings.yellow}>Type:</color> {dungeonMapData.dungeonType} {dungeonMapData.mapType}",
                                    $"<color={CustomColorStrings.yellow}>Difficulty:</color> {dungeonMapData.mapDifficulty}",
                                    dungeonMapData.isTimed ? $"<color={CustomColorStrings.yellow}>Time Limit:</color> {TimeSpan.FromSeconds(dungeonMapData.timeLimit).ToString("mm':'ss")}" : null
                                );
                            });
                        }
                        else
                        {
                            button.GetComponent<Button>().onClick.AddListener(delegate
                            {
                                ConfirmationPanel.Instance.ShowConfirmationPanel
                                (
                                    $"Enter <color={CustomColorStrings.green}>{mapData.mapName}</color>",
                                    null,
                                    () =>
                                    {
                                        GameManager.Instance.LoadMap(mapData.mapId);
                                        HidePanel();
                                    },
                                    false,
                                    true,
                                    $"<color={CustomColorStrings.yellow}>Type:</color> {mapData.mapType}",
                                    $"<color={CustomColorStrings.yellow}>Difficulty:</color> {mapData.mapDifficulty}"
                                );
                            });
                        }

                        // set up button text
                        Text buttonTitle = Common.RecursiveFindChild(button.transform, "Title").GetComponent<Text>();
                        buttonTitle.text = mapData.mapName;

                        Text buttonDescription = Common.RecursiveFindChild(button.transform, "Description").GetComponent<Text>();

                        if (mapData.mapType == MapType.Peaceful)
                            buttonDescription.text = $"{mapData.mapType.ToString()}";
                        else if (mapData.mapType == MapType.Dungeon)
                        {
                            DungeonMapData dungeonMapData = mapData as DungeonMapData;
                            buttonDescription.text = $"{dungeonMapData.dungeonType} {dungeonMapData.mapType.ToString()} / {dungeonMapData.mapDifficulty.ToString()}";
                        }
                        else
                            buttonDescription.text = $"{mapData.mapType.ToString()} / {mapData.mapDifficulty.ToString()}";
                    }
                }
                break;
            case MapSelectionType.Dungeon: // repetitive; rewrite this section maybe?
                {
                    titleText.text = "Dungeons";
                    messageText.text = "Select a dungeon from the list below...";

                    foreach (MapData mapData in gameMaps.maps)
                    {
                        if (mapData.mapType == MapType.Dungeon)
                        {
                            DungeonMapData dungeonMapData = mapData as DungeonMapData;

                            GameObject button = Instantiate(buttonPrefab, contentTransform);
                            button.GetComponent<Button>().onClick.AddListener(delegate
                            {
                                ConfirmationPanel.Instance.ShowConfirmationPanel
                                (
                                    $"Enter <color={CustomColorStrings.green}>{dungeonMapData.mapName}</color>",
                                    $"<color=red>!!</color> You cannot save/quit in a dungeon battle! You must play through the whole dungeon.\n<color=red>!!</color> Upon death, you will lose all your progress in this dungeon!",
                                    () =>
                                    {
                                        GameManager.Instance.LoadMap(dungeonMapData.mapId);
                                        HidePanel();
                                    },
                                    false,
                                    true,
                                    $"<color={CustomColorStrings.yellow}>Type:</color> {dungeonMapData.dungeonType} {dungeonMapData.mapType}",
                                    $"<color={CustomColorStrings.yellow}>Difficulty:</color> {dungeonMapData.mapDifficulty}",
                                    dungeonMapData.isTimed ? $"<color={CustomColorStrings.yellow}>Time Limit:</color> {TimeSpan.FromSeconds(dungeonMapData.timeLimit).ToString("mm':'ss")}" : null
                                );
                            });

                            Text buttonTitle = Common.RecursiveFindChild(button.transform, "Title").GetComponent<Text>();
                            buttonTitle.text = dungeonMapData.mapName;

                            Text buttonDescription = Common.RecursiveFindChild(button.transform, "Description").GetComponent<Text>();
                            buttonDescription.text = $"{dungeonMapData.mapType.ToString()} ({dungeonMapData.dungeonType}) / {dungeonMapData.mapDifficulty.ToString()}";
                        }
                    }
                }
                break;
        }

        ShowPanel();
    }

    public void DungeonSelected(string name)
        => Debug.Log(name);

    private void RefreshContents()
    {
        foreach (Transform child in contentTransform)
            Destroy(child.gameObject);
    }

    public override async void ShowPanel()
    {
        base.ShowPanel();
        gameObject.SetActive(true);
        await gameObject.GetComponent<CanvasGroup>().DOFade(1, 0.25f).SetEase(Ease.OutQuart).AsyncWaitForCompletion();
        // isOpened = true;
        panelState = PanelState.Shown;
    }

    public async override void HidePanel()
    {
        base.HidePanel();

        await gameObject.GetComponent<CanvasGroup>().DOFade(0, 0.25f).SetEase(Ease.OutQuart).AsyncWaitForCompletion();
        gameObject.SetActive(false);
        // isOpened = false;
        panelState = PanelState.Hidden;
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
