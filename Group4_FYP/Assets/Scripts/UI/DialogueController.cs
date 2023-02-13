using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class DialogueEvents : UnityEvent {}

// [Serializable]
// public class DialogueEntry
// {
//     [Tooltip("Leave blank to use previous name")] public string name;
//     [TextArea(2, 5)] public string[] dialogues;
//     // public DialogueEvents dialogueEvents; // this takes up to much space
// }

public class DialogueController : MonoBehaviour
{
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private GameObject dialoguePanelPrefab;
    [SerializeField] private GameObject dialoguePanelContainer;
    private DialoguePanel dialoguePanel;
    private GameObject clone;

    private string header;
    private string[] dialogues;
    private DialogueEvents dialogueEvents;
    private Sprite sprite;
    private string hint;

    private int currentDialogueIndex;
    private bool isInConversation;
    private bool canShowNextDialogue;
    private bool canEndManually;

    private static DialogueController _instance;
    public static DialogueController Instance
    {
        get => _instance;
    }

    void Awake()
    {
        if (!_instance)
        {
            _instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    async void Update()
    {
        if (isInConversation)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                await NextDialogue();
            }

            if (canEndManually && Input.GetKeyDown(KeyCode.Period))
            {
                await dialoguePanel.HideDialoguePanel();
                isInConversation = false;
                _ = Notification.Instance.ShowNotification("You ended the conversation");
            }
        }
    }

    public async Task ShowDialogue(string header, string[] dialogues, DialogueEvents dialogueEvents, Sprite sprite = null, bool canEndManually = true)
    {
        if (!isInConversation)
        {
            canShowNextDialogue = false;

            if (dialogues.Length <= 0)
            {
                Debug.LogError("dialogues is empty");
                return;
            }

            if (clone)
            {
                Destroy(clone);
            }
            clone = Instantiate(dialoguePanelPrefab, dialoguePanelContainer.transform);
            dialoguePanel = clone.GetComponent<DialoguePanel>();

            this.header = header;
            this.dialogues = dialogues;
            this.dialogueEvents = dialogueEvents;
            this.canEndManually = canEndManually;

            if (sprite)
            {
                this.sprite = sprite;
            }
            else
            {
                sprite = defaultSprite;
            }

            // can be written better?
            hint = "[SPACE] Continue";
            if (canEndManually)
            {
                hint += "; [.] End";
            }

            currentDialogueIndex = 0;
            isInConversation = true;

            await dialoguePanel.ShowDialoguePanel(header, dialogues[currentDialogueIndex], sprite, hint);
            canShowNextDialogue = true;
        }
    }

    public async Task NextDialogue()
    {
        if (canShowNextDialogue)
        {
            canShowNextDialogue = false;

            currentDialogueIndex++;

            if (currentDialogueIndex < dialogues.Length)
            {
                await dialoguePanel.ShowDialoguePanel(header, dialogues[currentDialogueIndex], sprite, hint);
            }
            else
            {
                await dialoguePanel.HideDialoguePanel();
                dialogueEvents.Invoke();

                currentDialogueIndex = 0;
                isInConversation = false;
            }

            canShowNextDialogue = true;
        }
    }
}
