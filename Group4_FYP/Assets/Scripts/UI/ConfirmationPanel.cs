using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;

public class ConfirmationPanel : MonoBehaviour
{
    [SerializeField] private Text titleText;
    [SerializeField] private Text messageText;
    [SerializeField] private GameObject buttonsPanelNormal;
    [SerializeField] private GameObject buttonsPanelImportant;
    [SerializeField] private Button confirmButtonNormal;
    [SerializeField] private Button confirmButtonImportant;

    // these variables prevent AddListener() from piling up?
    private UnityAction confirmAction;
    private UnityAction cancelAction;

    private static ConfirmationPanel instance;
    public static ConfirmationPanel Instance
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
    }

    public void ShowConfirmationPanel(string title, string message, UnityAction confirmAction, bool isImportant)
    {
        ShowConfirmationPanel(title, message, confirmAction, () => { _ = HideConfirmationPanel(); }, isImportant);
    }

    public async void ShowConfirmationPanel(string title, string message, UnityAction confirmAction, UnityAction cancelAction, bool isImportant)
    {
        titleText.text = title;
        messageText.text = message;
        this.confirmAction = confirmAction;
        this.cancelAction = cancelAction;

        if (!isImportant)
        {
            buttonsPanelNormal.SetActive(true);
            buttonsPanelImportant.SetActive(false);
        }
        else
        {
            buttonsPanelNormal.SetActive(false);
            buttonsPanelImportant.SetActive(true);
        }

        gameObject.SetActive(true);
        await gameObject.GetComponent<CanvasGroup>().DOFade(1, 0.25f).SetEase(Ease.OutQuart).AsyncWaitForCompletion();
    }

    public async Task HideConfirmationPanel()
    {
        await gameObject.GetComponent<CanvasGroup>().DOFade(0, 0.25f).SetEase(Ease.OutQuart).AsyncWaitForCompletion();
        gameObject.SetActive(false);
    }

    public async void Confirm()
    {
        await HideConfirmationPanel();
        confirmAction.Invoke();
    }

    public async void Cancel()
    {
        await HideConfirmationPanel();
        cancelAction.Invoke();
    }
}
