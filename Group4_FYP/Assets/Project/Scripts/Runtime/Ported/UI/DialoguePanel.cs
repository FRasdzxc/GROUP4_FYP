using System.Collections;
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
    [SerializeField] private float textDisplayDelay;

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

    public IEnumerator ShowDialoguePanel(string header, string dialogue, Sprite sprite, string hint)
    {
        headerText.text = header;
        dialogueText.text = null;
        image.sprite = sprite;
        hintText.text = hint;

        if (panelState.Equals(PanelState.Hidden))
        {
            ShowPanel();
            gameObject.SetActive(true);
            yield return gameObject.GetComponent<CanvasGroup>().DOFade(1, 0.25f).SetEase(Ease.OutQuart).WaitForCompletion();
        }

        panelState = PanelState.Shown;
        yield return DisplayDialogue(dialogue);
    }

    public async Task HideDialoguePanel()
    {
        if (panelState.Equals(PanelState.Hidden))
            return;

        await gameObject.GetComponent<CanvasGroup>().DOFade(0, 0.25f).SetEase(Ease.OutQuart).AsyncWaitForCompletion();
        gameObject.SetActive(false);
        DialogueController.Instance.IsInConversation = false;
    }

    public async override void HidePanel()
    {
        base.HidePanel();
        await HideDialoguePanel();
        panelState = PanelState.Hidden;
    }

    public void SetAllowHiding(bool allowHiding)
        => this.allowsHiding = allowHiding;

    private IEnumerator DisplayDialogue(string dialogue)
    {
        if (!isDisplayingDialogue)
        {
            isDisplayingDialogue = true;
            yield return dialogueText.DOText(dialogue, dialogue.Length * textDisplayDelay).SetEase(Ease.Linear).WaitForCompletion();
            dialogueText.text = dialogue; // preventive
            isDisplayingDialogue = false;
        }
    }

    protected override GameObject GetPanelGobj()
        => gameObject;
}
