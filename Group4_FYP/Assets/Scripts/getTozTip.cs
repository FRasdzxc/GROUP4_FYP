using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class getTozTip : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public RectTransform canvas;
    Vector2 canvasPos;
    private RectTransform rectTransform;
    private bool mouseDown;
    private GameObject target;
    private string n_target;
    private Image t_Image;
    private Image s_Image;

    public void OnPointerDown(PointerEventData eventData)
    {
        mouseDown = (eventData.button == PointerEventData.InputButton.Left);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (mouseDown && eventData.button == PointerEventData.InputButton.Left)
            mouseDown = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, eventData.position, null, out canvasPos);
        rectTransform.anchoredPosition = canvasPos;
    }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (mouseDown || target == null)
            return;

        var pushToy = target.gameObject.GetComponent<pushToy>();
        if (pushToy != null)
        {
            t_Image.sprite = s_Image.sprite;
            pushToy.OnPush(gameObject);

            // DEMO ONLY PLEASE REMOVE
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        target = collider.gameObject;
        n_target = collider.name;
        t_Image = GameObject.Find(n_target + "/Image").GetComponent<Image>();
        s_Image = GetComponent<Image>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (target != null)
        {
            target = null;
            n_target = null;
            t_Image = null;
            s_Image = null;
        }
    }
}
