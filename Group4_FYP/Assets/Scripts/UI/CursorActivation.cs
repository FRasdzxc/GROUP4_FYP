using UnityEngine;
using UnityEngine.EventSystems;

public class CursorActivation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private CursorType cursorType;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerEnter == gameObject)
        {
            CursorController.Instance.SetCursor(cursorType);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerEnter == gameObject)
        {
            SetDefaultCursor();
        }
    }

    private void OnDisable()
    {
        SetDefaultCursor();
    }

    private void SetDefaultCursor()
    {
        CursorController.Instance.SetDefaultCursor();
    }
}
