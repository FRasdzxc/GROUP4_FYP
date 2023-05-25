using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform), typeof(ContentSizeFitter))]
public class ContentScroller : UIBehaviour
{
    public enum ScrollType
    {
        ScrollToTop,
        ScrollToBottom
    }

    [SerializeField]
    private ScrollRect scrollRect;

    [SerializeField]
    private ScrollType scrollType;

    private RectTransform rectTransform;

    protected override void Start() => rectTransform = GetComponent<RectTransform>();

    protected override void OnRectTransformDimensionsChange()
    {
        base.OnRectTransformDimensionsChange();
        
        if (scrollType.Equals(ScrollType.ScrollToTop))
            scrollRect.verticalNormalizedPosition = 1f;
        else
            scrollRect.verticalNormalizedPosition = 0f;
    }
}
