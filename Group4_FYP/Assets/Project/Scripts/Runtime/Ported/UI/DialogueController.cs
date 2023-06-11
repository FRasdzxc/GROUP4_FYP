using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using PathOfHero.Utilities;
using PathOfHero.Others;

[Serializable]
public class DialogueEvents : UnityEvent {}

public class DialogueController : Singleton<DialogueController>
{
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private GameObject dialoguePanelPrefab;
    [SerializeField] private GameObject dialoguePanelContainer;
    [SerializeField] private InputReader m_InputReader;
    [SerializeField] private InputActionReference nextDialogueAction;
    [SerializeField] private InputActionReference skipDialogueAction;

    private DialoguePanel dialoguePanel;
    private GameObject clone;

    private string header;
    private DialogueEntry[] dialogueEntries;
    private DialogueEvents dialogueStartEvents;
    private DialogueEvents dialogueEndEvents;
    private Sprite sprite;
    private string hint;

    private int currentDialogueIndex;
    private bool canShowNextDialogue;
    private bool canBeSkipped;

    public bool IsInConversation { get; set; }

    public IEnumerator ShowDialogue(string header, DialogueEntry[] dialogueEntries, DialogueEvents dialogueEndEvents, Sprite sprite = null, bool canBeSkipped = true)
    {
        if (!IsInConversation)
        {
            if (dialogueEntries.Length <= 0)
            {
                Debug.LogWarning("dialogues is empty");
                dialogueEndEvents.Invoke();
                yield break;
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
            hint = $"'<color={CustomColorStrings.yellow}>{nextDialogueAction.action.GetBindingDisplayString()}</color>' Continue";
            if (canBeSkipped)
                hint += $"<color={CustomColorStrings.white}>;</color> '<color={CustomColorStrings.yellow}>{skipDialogueAction.action.GetBindingDisplayString()}</color>' Skip";

            currentDialogueIndex = -1;
            IsInConversation = true;
            canShowNextDialogue = true;
            yield return NextDialogue();
        }
    }

    public IEnumerator NextDialogue()
    {
        if (canShowNextDialogue)
        {
            canShowNextDialogue = false;

            currentDialogueIndex++;

            if (currentDialogueIndex < dialogueEntries.Length)
            {
                dialogueEntries[currentDialogueIndex].eventRequestData?.Invoke();
                yield return dialoguePanel.ShowDialoguePanel(header, dialogueEntries[currentDialogueIndex].dialogue, sprite, hint);
            }
            else
            {
                dialoguePanel.SetAllowHiding(true);
                dialoguePanel.HidePanel();
                dialogueEndEvents.Invoke();

                currentDialogueIndex = 0;
                IsInConversation = false;
            }

            canShowNextDialogue = true;
        }
    }

    void OnEnable()
    {
        m_InputReader.NextDialogue += OnNextDialogue;
        m_InputReader.SkipDialogue += OnSkipDialogue;
    }

    void OnDisable()
    {
        m_InputReader.NextDialogue -= OnNextDialogue;
        m_InputReader.SkipDialogue -= OnSkipDialogue;
    }

    private void OnNextDialogue()
    {
        if (!IsInConversation)
            return;

        StartCoroutine(NextDialogue());
    }

    private void OnSkipDialogue()
    {
        if (!IsInConversation || !canBeSkipped)
            return;

        if (dialoguePanel.PanelState == PanelState.Shown)
        {
            dialoguePanel.HidePanel();
            dialogueEndEvents.Invoke();

            currentDialogueIndex = 0;
            IsInConversation = false;
            _ = Notification.Instance.ShowNotificationAsync("You skipped the conversation");
        }
    }
}
