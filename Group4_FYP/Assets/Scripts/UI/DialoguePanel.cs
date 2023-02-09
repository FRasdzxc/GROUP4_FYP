using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DialoguePanel : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Text headerText;
    [SerializeField] private Text dialogueText;
    [SerializeField] private float textDisplayDelay;

    private bool panelIsShown;
    private bool isDisplayingDialogue;

    private static DialoguePanel instance;
    public static DialoguePanel Instance
    {
        get
        {
            return instance;
        }
    }

    void Awake()
    {
        if (!instance)
        {
            instance = this;
        }

        gameObject.SetActive(false);
        gameObject.GetComponent<CanvasGroup>().alpha = 0;
        panelIsShown = false; // preventive
        isDisplayingDialogue = false; // preventive
    }

    public async Task ShowDialoguePanel(string header, string dialogue, Sprite sprite)
    {
        headerText.text = header;
        dialogueText.text = null;
        image.sprite = sprite;

        if (!panelIsShown)
        {
            gameObject.SetActive(true);
            await gameObject.GetComponent<CanvasGroup>().DOFade(1, 0.25f).SetEase(Ease.OutQuart).AsyncWaitForCompletion();
            panelIsShown = true;
        }

        await DisplayDialogue(dialogue);
    }

    public async Task HideDialoguePanel()
    {
        await gameObject.GetComponent<CanvasGroup>().DOFade(0, 0.25f).SetEase(Ease.OutQuart).AsyncWaitForCompletion();
        gameObject.SetActive(false);
        panelIsShown = false;
    }    

    private async Task DisplayDialogue(string dialogue)
    {
        if (!isDisplayingDialogue)
        {
            isDisplayingDialogue = true;

            for (int i = 0; i < dialogue.Length; i++)
            {
                dialogueText.text = dialogue.Substring(0, i + 1);

                await Task.Delay((int)(textDisplayDelay * 1000));
            }
            dialogueText.text = dialogue;

            isDisplayingDialogue = false;
        }
    }
}
