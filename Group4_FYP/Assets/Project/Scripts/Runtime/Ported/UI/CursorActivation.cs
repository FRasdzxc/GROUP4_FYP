using UnityEngine;
using UnityEngine.EventSystems;
using PathOfHero.Controllers;

public class CursorActivation : MonoBehaviour, IPointerEnterHandler/*, IPointerExitHandler*/
{
    [SerializeField] private CursorController.CursorType cursorType;
    [SerializeField] private CursorController cursorController;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerEnter == gameObject)
        {
            cursorController.ChangeCursor(cursorType);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerEnter == gameObject)
            SetDefaultCursor();
    }

    private void OnDisable()
        => SetDefaultCursor();

    private void SetDefaultCursor()
        => cursorController.ChangeCursor(CursorManager.Instance.GetDefaultCursorType());
}
