using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Item : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    [SerializeField] private ScrollRect inventoryScrollRect;
    [SerializeField] private Sprite itemImage;

    private Sprite currentItem;
    private bool isVacant;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (currentItem)
        {
            //currentItem.transform.position = Input.mousePosition;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Pointer is down");
        inventoryScrollRect.enabled = false;

        currentItem = Instantiate(itemImage, Input.mousePosition, Quaternion.identity);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("Pointer is up");
        inventoryScrollRect.enabled = true;

        currentItem = null;
    }

    public void OnPointerClick(PointerEventData eventData)
    {

    }
}
