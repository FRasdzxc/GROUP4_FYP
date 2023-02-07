using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

// for basic gui elements only? or implement an event member?
public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string header;
    [SerializeField] [TextArea(5, 5)] private string description;
    [SerializeField] [TextArea(5, 5)] private string attributes;
    [SerializeField] private string hints;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Tooltip.Instance.ShowTooltip(header, description, attributes, hints);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Tooltip.Instance.HideTooltip();
    }

    //public void SetHeader(string value)
    //{
    //    header = value;
    //}

    //public void SetDescription(string value)
    //{
    //    description = value;
    //}

    //public void SetAttributes(string[] values)
    //{
    //    StringBuilder sb = new StringBuilder();
    //    foreach (var v in values)
    //    {
    //        sb.AppendLine(v);
    //    }
    //    attributes = sb.ToString();

    //    if (attributes.Length > 0)
    //    {
    //        attributes.Substring(0, attributes.Length - 1);
    //    }
    //}

    public void SetupTooltip(string header, string description = "", string[] attributes = null, TooltipHintType[] tooltipHintType = null)
    {
        this.header = header;
        this.description = description;

        if (attributes == null)
        {
            this.attributes = "";
        }
        else
        {
            StringBuilder sb = new StringBuilder();
            foreach (var v in attributes)
            {
                sb.AppendLine(v);
            }
            this.attributes = sb.ToString().Substring(0, sb.Length - 1);
        }

        if (tooltipHintType == null)
        {
            this.hints = "";
        }
        else
        {
            StringBuilder sb = new StringBuilder();
            foreach (var h in tooltipHintType)
            {
                sb.Append(h.ToString() + '\t');
            }
            this.hints = sb.ToString().Substring(0, hints.Length - 1);
        }
    }
}
