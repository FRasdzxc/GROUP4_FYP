using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using PathOfHero.Utilities;

[Serializable]
public class DialogueEvents : UnityEvent {}

// [Serializable]
// public class DialogueEntry
// {
//     [Tooltip("Leave blank to use previous name")] public string name;
//     [TextArea(2, 5)] public string[] dialogues;
//     // public DialogueEvents dialogueEvents; // this takes up to much space
// }

public class DialogueController : Singleton<DialogueController>
{
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private GameObject dialoguePanelPrefab;
    [SerializeField] private GameObject dialoguePanelContainer;
    private DialoguePanel dialoguePanel;
    private GameObject clone;

    private string header;
    private DialogueEntry[] dialogueEntries;
    private DialogueEvents dialogueStartEvents;
    private DialogueEvents dialogueEndEvents;
    private Sprite sprite;
    private string hint;

    private int currentDialogueIndex;
    private bool isInConversation;
    private bool canShowNextDialogue;
    private bool canBeSkipped;

    private PlayerInput playerInput;
    private InputAction nextDialogueAction;
    private InputAction skipDialogueAction;

    // Update is called once per frame
    async void Update()
    {
        if (isInConversation)
        {
            if (nextDialogueAction.triggered)
                await NextDialogue();

            // if (canBeSkipped && Input.GetKeyDown(KeyCode.Period))
            if (canBeSkipped && skipDialogueAction.triggered && dialoguePanel.GetIsOpened())
            {
                dialoguePanel.HidePanel();
                dialogueEndEvents.Invoke();

                currentDialogueIndex = 0;
                isInConversation = false;
                _ = Notification.Instance.ShowNotification("You skipped the conversation");
            }
        }
    }

    public async Task ShowDialogue(string header, DialogueEntry[] dialogueEntries, DialogueEvents dialogueEndEvents, Sprite sprite = null, bool canBeSkipped = true)
    {
        if (!isInConversation)
        {
            // canShowNextDialogue = false;

            if (dialogueEntries.Length <= 0)
            {
                Debug.LogWarning("dialogues is empty");
                dialogueEndEvents.Invoke();
                return;
            }

            if (clone)
                Destroy(clone);
            clone = Instantiate(dialoguePanelPrefab, dialoguePanelContainer.transform);
            dialoguePanel = clone.GetComponent<DialoguePanel>();
            dialoguePanel.SetAllowHiding(canBeSkipped);

            this.header = header;
            this.dialogueEntries = dialogueEntries;
            this.dialogueEndEvents = dialogueEndEvents;
            this.canBeSkipped = canBeSkipped;

            if (sprite)
                this.sprite = sprite;
            else
                sprite = defaultSprite;

            // can be written better?
            hint = "[SPACE] Continue";
            if (canBeSkipped)
                hint += "; [.] Skip";

            currentDialogueIndex = -1;
            isInConversation = true;
            canShowNextDialogue = true;
            await NextDialogue();

            // dialogueEntries[currentDialogueIndex].eventRequestData?.Invoke();
            // await dialoguePanel.ShowDialoguePanel(header, dialogueEntries[currentDialogueIndex].dialogue, sprite, hint);
            // canShowNextDialogue = true;
        }
    }

    public async Task NextDialogue()
    {
        if (canShowNextDialogue)
        {
            canShowNextDialogue = false;

            currentDialogueIndex++;

            if (currentDialogueIndex < dialogueEntries.Length)
            {
                dialogueEntries[currentDialogueIndex].eventRequestData?.Invoke();
                await dialoguePanel.ShowDialoguePanel(header, dialogueEntries[currentDialogueIndex].dialogue, sprite, hint);
            }
            else
            {
                dialoguePanel.SetAllowHiding(true);
                //await dialoguePanel.HideDialoguePanel();
                dialoguePanel.HidePanel();
                dialogueEndEvents.Invoke();

                currentDialogueIndex = 0;
                isInConversation = false;
            }

            canShowNextDialogue = true;
        }
    }

    public void SetIsInConversation(bool value)
        => isInConversation = value;

    public bool GetIsInConversation()
        => isInConversation;

    void OnEnable()
        => GameManager.onPlayerSetUp += SetUp;
    
    void OnDisable()
        => GameManager.onPlayerSetUp -= SetUp;

    public void SetUp()
    {
        playerInput = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();

        nextDialogueAction = playerInput.actions["NextDialogue"];
        skipDialogueAction = playerInput.actions["SkipDialogue"];

        nextDialogueAction.Enable();
        skipDialogueAction.Enable();
    }
    
}
