using UnityEngine;
using DG.Tweening;

public class DialogueTrigger : Interaction
{
    [SerializeField] private Sprite sprite;
    [SerializeField] private string header;
    [SerializeField] [TextArea(2, 5)] private string[] dialogues;
    [SerializeField] private DialogueEvents dialogueEndEvents;
    [SerializeField] private bool canBeSkipped = true;

    [SerializeField] private bool triggerOnStart;
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

        // dialogueBubble.SetActive(true);
        // dialogueBubbleCanvasGroup = dialogueBubble.GetComponent<CanvasGroup>();
        // dialogueBubbleCanvasGroup.alpha = 1;
        // hintPanel.SetActive(false);
        // hintPanelCanvasGroup = hintPanel.GetComponent<CanvasGroup>();
        // hintPanelCanvasGroup.alpha = 0;
        // player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    // protected override void Update()
    // {
    //     base.Update();

    //     // if (Vector2.Distance(transform.position, player.transform.position) <= triggerDistance)
    //     // {
    //     //     if (!hintPanel.activeSelf)
    //     //     {
    //     //         hintPanel.SetActive(true);
    //     //         _ = hintPanelCanvasGroup.DOFade(1, 0.25f).SetEase(Ease.OutQuart).AsyncWaitForCompletion();
    //     //         await dialogueBubbleCanvasGroup.DOFade(0, 0.25f).SetEase(Ease.OutQuart).AsyncWaitForCompletion();
    //     //         dialogueBubble.SetActive(false);
    //     //     }

    //     //     if (Input.GetKeyDown(KeyCode.E))
    //     //     {
    //     //         Trigger();
    //     //     }
    //     // }
    //     // else
    //     // {
    //     //     if (hintPanel.activeSelf)
    //     //     {
    //     //         dialogueBubble.SetActive(true);
    //     //         _ = dialogueBubbleCanvasGroup.DOFade(1, 0.25f).SetEase(Ease.OutQuart).AsyncWaitForCompletion();
    //     //         await hintPanelCanvasGroup.DOFade(0, 0.25f).SetEase(Ease.OutQuart).AsyncWaitForCompletion();
    //     //         hintPanel.SetActive(false);
    //     //     }
    //     // }
    // }

    protected async override void Interact()
    {
        await DialogueController.Instance.ShowDialogue(header, dialogues, dialogueEndEvents, sprite, canBeSkipped);
    }
}
