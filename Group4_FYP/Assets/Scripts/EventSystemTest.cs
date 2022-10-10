using UnityEngine;
using UnityEngine.EventSystems;

public class EventSystemTest : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Hello");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Helloooooo");
    }
}
