using System;
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

    private CanvasGroup canvasGroup;
    private UnityAction confirmAction; // this variable prevents AddListener() from piling up?

    public async void ShowConfirmationPanel(string title, string message, UnityAction confirmAction, bool isImportant)
    {
        titleText.text = title;
        messageText.text = message;
        this.confirmAction = confirmAction;

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
        await gameObject.GetComponent<CanvasGroup>().DOFade(1, 0.25f).AsyncWaitForCompletion();
    }

    public async void HideConfirmationPanel()
    {
        await gameObject.GetComponent<CanvasGroup>().DOFade(0, 0.25f).AsyncWaitForCompletion();
        gameObject.SetActive(false);
    }

    public void Confirm()
    {
        confirmAction.Invoke();
        HideConfirmationPanel();
    }
}
