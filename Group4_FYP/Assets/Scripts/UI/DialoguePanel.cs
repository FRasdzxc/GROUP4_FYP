using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DialoguePanel : MonoBehaviour
{
    [SerializeField] private Text nameText;
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

    // rewrite how name is assigned?
    public async Task ShowDialoguePanel(string name, string dialogue)
    {
        nameText.text = null;
        dialogueText.text = null;

        if (!panelIsShown)
        {
            gameObject.SetActive(true);
            await gameObject.GetComponent<CanvasGroup>().DOFade(1, 0.25f).AsyncWaitForCompletion();
            panelIsShown = true;
        }

        nameText.text = name;
        await DisplayDialogue(dialogue);
    }

    public async void HideDialoguePanel()
    {
        await gameObject.GetComponent<CanvasGroup>().DOFade(0, 0.25f).AsyncWaitForCompletion();
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
