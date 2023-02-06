using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

// for basic gui elements only? or implement an event member?
public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string header;
    [SerializeField] [TextArea(5, 5)] private string description;
    [SerializeField] [TextArea(5, 5)] private string attributes;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Tooltip.Instance.ShowTooltip(header, description, attributes);
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

    public void SetupTooltip(string header, string description = "", string[] attributes = null)
    {
        //SetHeader(header)
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
            this.attributes = sb.ToString();

            if (attributes.Length > 0)
            {
                this.attributes.Substring(0, attributes.Length - 1);
            }
        }
    }
}
