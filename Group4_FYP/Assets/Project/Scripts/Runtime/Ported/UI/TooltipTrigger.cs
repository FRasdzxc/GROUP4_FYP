using UnityEngine;
using UnityEngine.EventSystems;

// for basic gui elements only
public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string header;
    [SerializeField] private string type;
    [SerializeField] [TextArea(5, 5)] private string description;
    [SerializeField] private string[] attributes;
    [SerializeField] private TooltipHintType[] hints;

    public void OnPointerEnter(PointerEventData eventData)
        => Tooltip.Instance.ShowTooltip(header, type, description, attributes, hints);

    public void OnPointerExit(PointerEventData eventData)
        => Tooltip.Instance.HideTooltip();

    public void SetupTooltip(string header, string type = "", string description = "", string[] attributes = null, TooltipHintType[] hints = null)
    {
        this.header = header;
        this.type = type;
        this.description = description;
        this.attributes = attributes;
        this.hints = hints;
    }
}
