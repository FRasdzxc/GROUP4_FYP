using UnityEngine;

public class DialogueTrigger : Interaction
{
    [SerializeField] private Sprite sprite;
    [SerializeField] private string header;
    [SerializeField] [TextArea(2, 5)] private string[] dialogues;
    [SerializeField] private bool canBeSkipped = true;

    [SerializeField] private bool triggerOnStart;
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

        if (eventRequestData)
        {
            dialogueEndEvents.AddListener(eventRequestData.Invoke);
        }
    }

    protected async override void Interact()
    {
        await DialogueController.Instance.ShowDialogue(header, dialogues, dialogueEndEvents, sprite, canBeSkipped);
    }
}
