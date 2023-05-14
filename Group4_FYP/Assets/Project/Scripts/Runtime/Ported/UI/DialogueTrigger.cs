using System;
using UnityEngine;

[Serializable]
public class DialogueEntry
{
    [TextArea(2, 5)]
    public string dialogue;

    [Tooltip("Invoke this once this dialogue is shown\nLeave blank if none")]
    public EventRequestData eventRequestData;
}

public class DialogueTrigger : Interaction
{
    

    [SerializeField] private Sprite sprite;
    [SerializeField] private string header;
    [SerializeField] private DialogueEntry[] dialogueEntries;
    [SerializeField] private bool canBeSkipped = true;

    [SerializeField] private bool triggerOnStart;
    [SerializeField] private EventRequestData eventRequestData;
    [SerializeField] private AudioClip[] dialogueSounds;

    private DialogueEvents dialogueEndEvents = new DialogueEvents();

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        if (triggerOnStart)
            Interact();

        if (eventRequestData)
            dialogueEndEvents.AddListener(eventRequestData.Invoke);
    }

    protected async override void Interact()
    {
        if (dialogueSounds.Length > 0)
            AudioManager.Instance.PlaySound(dialogueSounds[UnityEngine.Random.Range(0, dialogueSounds.Length)]);

        await DialogueController.Instance.ShowDialogue(header, dialogueEntries, dialogueEndEvents, sprite, canBeSkipped);
    }
}
