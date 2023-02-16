using UnityEngine;
using UnityEngine.EventSystems;

public class getTozTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler, IPointerDownHandler, IPointerUpHandler
{
    public RectTransform canvas;
    private RectTransform rectTransform;
    private bool mouseOver, mouseFollow;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!mouseOver || eventData.button != PointerEventData.InputButton.Left)
            return;

        mouseFollow = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!mouseFollow || eventData.button != PointerEventData.InputButton.Left)
            return;

        mouseFollow = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOver = false;
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, eventData.position, null, out Vector2 canvasPos);
        rectTransform.anchoredPosition = canvasPos;
    }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
}
