using System.Threading.Tasks;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    [SerializeField] private string[] dialogues; // test only
    private int currentDialogueIndex;

    // Start is called before the first frame update
    void Start()
    {
        currentDialogueIndex = 0;
    }

    // Update is called once per frame
    async void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log(dialogues[currentDialogueIndex]);
            await DialoguePanel.Instance.ShowDialoguePanel("test", dialogues[currentDialogueIndex]); // better not hardcode the name
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            await NextDialogue();
        }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.E))
        {
            DialoguePanel.Instance.HideDialoguePanel();
        }
    }

    public async Task NextDialogue()
    {
        currentDialogueIndex++;

        if (currentDialogueIndex < dialogues.Length)
        {
            await DialoguePanel.Instance.ShowDialoguePanel("test", dialogues[currentDialogueIndex]);
        }
        else
        {
            DialoguePanel.Instance.HideDialoguePanel();
            currentDialogueIndex = 0;
        }
    }
}
