using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DialoguePanel : PanelOverride /*MonoBehaviour*/
{
    [SerializeField] private Image image;
    [SerializeField] private Text headerText;
    [SerializeField] private Text dialogueText;
    [SerializeField] private Text hintText;
    [SerializeField] [HideInInspector] private float textDisplayDelay;

    private bool panelIsShown;
    private bool isDisplayingDialogue;

    protected override void Awake()
    {
        base.Awake();
        gameObject.SetActive(false);
        gameObject.GetComponent<CanvasGroup>().alpha = 0;
        panelIsShown = false; // preventive
        isDisplayingDialogue = false; // preventive
    }

    public async Task ShowDialoguePanel(string header, string dialogue, Sprite sprite, string hint)
    {
        headerText.text = header;
        dialogueText.text = null;
        image.sprite = sprite;
        hintText.text = hint;

        if (!panelIsShown)
        {
            ShowPanel();
            gameObject.SetActive(true);
            await gameObject.GetComponent<CanvasGroup>().DOFade(1, 0.25f).SetEase(Ease.OutQuart).AsyncWaitForCompletion();
            panelIsShown = true;
        }

        isOpened = true;
        await DisplayDialogue(dialogue);
    }

    public async Task HideDialoguePanel()
    {
        if (!panelIsShown)
            return;

        //HidePanel();

        await gameObject.GetComponent<CanvasGroup>().DOFade(0, 0.25f).SetEase(Ease.OutQuart).AsyncWaitForCompletion();
        gameObject.SetActive(false);
        panelIsShown = false;
        DialogueController.Instance.SetIsInConversation(false);
    }

    public async override void HidePanel()
    {
        base.HidePanel();
        await HideDialoguePanel();
        isOpened = false;
    }

    public void SetAllowHiding(bool allowHiding)
    {
        this.allowHiding = allowHiding;
    }

    private async Task DisplayDialogue(string dialogue)
    {
        if (!isDisplayingDialogue)
        {
            isDisplayingDialogue = true;

            for (int i = 0; i < dialogue.Length; i++)
            {
                dialogueText.text = dialogue.Substring(0, i + 1);
                // await Task.Delay((int)(textDisplayDelay * 1000));
                await Task.Yield();
            }
            dialogueText.text = dialogue;

            isDisplayingDialogue = false;
        }
    }

    protected override GameObject GetPanel()
    {
        return gameObject;
    }
}
