using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public abstract class Interaction : MonoBehaviour
{
    [SerializeField] private Canvas keyHintCanvas;
    [Tooltip("Leave empty if none.")]
    [SerializeField] private GameObject placeholder;
    [SerializeField] private GameObject keyHintPanel;
    [SerializeField] private Text keyText;
    [SerializeField] private Text hintText;
    [SerializeField] private string hint;
    [SerializeField] private KeyCode interactionKey; 
    [SerializeField] private float interactionDistance = 2.5f;

    private CanvasGroup keyHintPanelCanvasGroup;
    private CanvasGroup placeholderCanvasGroup;
    private Transform player;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        // automatically position and scale canvas
        ApplyModifiedPosition();

        keyText.text = interactionKey.ToString();
        if (hint.Length > 0 || hint == null)
            hintText.text = hint;
        else
            hintText.text = "Interact";
        
        keyHintPanel.SetActive(false);
        keyHintPanelCanvasGroup = keyHintPanel.GetComponent<CanvasGroup>();
        keyHintPanelCanvasGroup.alpha = 0;

        player = GameObject.FindGameObjectWithTag("Player").transform;

        if (placeholder)
        {
            placeholder.SetActive(true);
            placeholderCanvasGroup = placeholder.GetComponent<CanvasGroup>();
            placeholderCanvasGroup.alpha = 1;
        }
    }

    // Update is called once per frame
    protected async virtual void Update()
    {
        if (IsInInteractionDistance())
        {
            if (!keyHintPanel.activeSelf)
            {
                keyHintPanel.SetActive(true);
                _ = keyHintPanelCanvasGroup.DOFade(1, 0.25f).SetEase(Ease.OutQuart).AsyncWaitForCompletion();

                if (placeholder)
                {
                    await placeholderCanvasGroup.DOFade(0, 0.25f).SetEase(Ease.OutQuart).AsyncWaitForCompletion();
                    placeholder.SetActive(false);
                }
            }

            if (Input.GetKeyDown(interactionKey))
                StartCoroutine(Interact());
        }
        else
        {
            if (keyHintPanel.activeSelf)
            {
                if (placeholder)
                {
                    placeholder.SetActive(true);
                    _ = placeholderCanvasGroup.DOFade(1, 0.25f).SetEase(Ease.OutQuart).AsyncWaitForCompletion();
                }
                
                await keyHintPanelCanvasGroup.DOFade(0, 0.25f).SetEase(Ease.OutQuart).AsyncWaitForCompletion();
                keyHintPanel.SetActive(false);
            }
        }
    }

    void OnDisable()
        => StopCoroutine(Interact());

    protected bool IsInInteractionDistance() // is this expensive?
        => (Vector2.Distance(transform.position, player.position) <= interactionDistance);

    protected abstract IEnumerator Interact();

    public void ApplyModifiedPosition()
    {
        if (transform.localScale.magnitude > 0)
        {
            float canvasScale = (1 / transform.localScale.x);
            float targetScale = canvasScale * 0.01f;
            keyHintCanvas.transform.localScale = new Vector2(targetScale, targetScale);
            keyHintCanvas.transform.localPosition = new Vector3(0, (1 + canvasScale) / 2);
        }
    }
}
