using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;

[Serializable]
public class DialogueEvents : UnityEvent {}

[Serializable]
public class DialogueEntry : MonoBehaviour
{
    [TextArea(2, 5)] public string dialogue;
    public bool hasEvents;
    public DialogueEvents dialogueEvents;
}

#if UNITY_EDITOR
[CustomEditor(typeof(DialogueEntry))]
public class DialogueEntryEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        DialogueEntry script = (DialogueEntry)target;

        script.dialogue = EditorGUILayout.TextArea("Dialogue", script.dialogue);
        script.hasEvents = EditorGUILayout.Toggle("Has Events", script.hasEvents);
        if (script.hasEvents)
        {
            // script.dialogueEvents = EditorGUILayout.PropertyField(serializedObject.FindProperty("dialogueEvents"), new GUIContent("Dialogue Events"));
            // script.dialogueEvents = EditorGUILayout.ObjectField("Dialogue Events", script.dialogueEvents, typeof(DialogueEvents), true) as DialogueEvents;
        }
    }
}
#endif

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
