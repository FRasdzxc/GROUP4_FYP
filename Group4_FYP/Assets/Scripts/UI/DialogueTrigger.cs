using UnityEngine;
using DG.Tweening;

public class DialogueTrigger : Interaction
{
    [SerializeField] private Sprite sprite;
    [SerializeField] private string header;
    [SerializeField] [TextArea(2, 5)] private string[] dialogues;
    [SerializeField] private bool canBeSkipped = true;

    [SerializeField] private bool triggerOnStart;
    [SerializeField] private EventRequestType eventRequest;

    private DialogueEvents dialogueEndEvents = new DialogueEvents();
    // [SerializeField] private float triggerDistance = 2.5f;
    // [SerializeField] private GameObject dialogueBubble;
    // [SerializeField] private GameObject hintPanel;

    // private CanvasGroup dialogueBubbleCanvasGroup;
    // private CanvasGroup hintPanelCanvasGroup;

    // private GameObject player;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        if (triggerOnStart)
        {
            Interact();
        }

        switch (eventRequest)
        {
            case EventRequestType.ShowBuyPanel:
                dialogueEndEvents.AddListener(BuySellPanel.Instance.ShowBuyPanel);
                break;
            case EventRequestType.ShowSellPanel:
                dialogueEndEvents.AddListener(BuySellPanel.Instance.ShowSellPanel);
                break;
            case EventRequestType.ShowWeaponUpgradePanel:
                dialogueEndEvents.AddListener(WeaponUpgradePanel.Instance.ShowWeaponUpgradePanel);
                break;
            case EventRequestType.ShowHeroPanel:
                dialogueEndEvents.AddListener(HeroPanel.Instance.ShowHeroPanel);
                break;
        }
    }

    protected async override void Interact()
    {
        await DialogueController.Instance.ShowDialogue(header, dialogues, dialogueEndEvents, sprite, canBeSkipped);
    }
}
