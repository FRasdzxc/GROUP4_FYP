using UnityEngine;
using DG.Tweening;

public class DialogueTrigger : Interaction
{
    [SerializeField] private Sprite sprite;
    [SerializeField] private string header;
    [SerializeField] [TextArea(2, 5)] private string[] dialogues;
    [SerializeField] private bool canBeSkipped = true;

    [SerializeField] private bool triggerOnStart;
    // [SerializeField] private EventRequestType eventRequest;
    [SerializeField] private EventRequestData eventRequestData;

    private DialogueEvents dialogueEndEvents = new DialogueEvents();

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        if (triggerOnStart)
        {
            Interact();
        }

        // is there a better way of doing this?
        // switch (eventRequest)
        // {
        //     case EventRequestType.ShowBuyPanel:
        //         dialogueEndEvents.AddListener(BuySellPanel.Instance.ShowBuyPanel);
        //         break;
        //     case EventRequestType.ShowSellPanel:
        //         dialogueEndEvents.AddListener(BuySellPanel.Instance.ShowSellPanel);
        //         break;
        //     case EventRequestType.ShowWeaponUpgradePanel:
        //         dialogueEndEvents.AddListener(WeaponUpgradePanel.Instance.ShowWeaponUpgradePanel);
        //         break;
        //     case EventRequestType.ShowHeroPanel:
        //         dialogueEndEvents.AddListener(HeroPanel.Instance.ShowHeroPanel);
        //         break;
        // }

        dialogueEndEvents.AddListener(eventRequestData.Invoke);
    }

    protected async override void Interact()
    {
        await DialogueController.Instance.ShowDialogue(header, dialogues, dialogueEndEvents, sprite, canBeSkipped);
    }
}
