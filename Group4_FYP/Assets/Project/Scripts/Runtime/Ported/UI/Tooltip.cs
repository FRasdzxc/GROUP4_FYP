using System.Text;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using PathOfHero.Utilities;

public class Tooltip : Singleton<Tooltip>
{
    [SerializeField] private Text headerText;
    [SerializeField] private Text typeText;
    [SerializeField] private Text descriptionText;
    [SerializeField] private Text attributeText;
    [SerializeField] private Text hintText;
    [SerializeField] private float posXOffset;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        gameObject.SetActive(false);
        canvasGroup.alpha = 0;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Input.mousePosition + new Vector3(posXOffset, 0);
        float pivotX = 0;

        // if right border of the TooltipPanel is over right side of the screen
        if ((rectTransform.transform.position.x + rectTransform.rect.width) > Screen.width)
        {
            transform.position = Input.mousePosition - new Vector3(posXOffset, 0);
            pivotX = 1;
        }

        float pivotY = transform.position.y / Screen.height;
        
        rectTransform.pivot = new Vector2(pivotX, pivotY);
    }

    public async void ShowTooltip(string header, string type = "", string description = "", string[] attributes = null, TooltipHintType[] hints = null)
    {
        KillTween();

        // header
        headerText.text = header;

        // type
        if (type != "")
        {
            typeText.gameObject.SetActive(true);
            typeText.text = type;
        }
        else
            typeText.gameObject.SetActive(false);

        // description
        if (description != "")
        {
            descriptionText.gameObject.SetActive(true);
            descriptionText.text = description;
        }
        else
            descriptionText.gameObject.SetActive(false);

        // attributes
        if (attributes == null || attributes.Length <= 0)
            attributeText.gameObject.SetActive(false);
        else
        {
            StringBuilder sb = new StringBuilder();
            foreach (var v in attributes)
                sb.AppendLine(v);
            attributeText.text = sb.ToString().Substring(0, sb.Length - 1); // remove last blank line
            attributeText.gameObject.SetActive(true);
        }

        // hints
        if (hints == null || hints.Length <= 0)
            hintText.gameObject.SetActive(false);
        else
        {
            StringBuilder sb = new StringBuilder();
            foreach (var h in hints)
            {
                switch (h)
                {
                    case TooltipHintType.None:
                        break;
                    case TooltipHintType.Use:
                        sb.AppendLine("[LMB] - Use");
                        break;
                    case TooltipHintType.Drop:
                        sb.AppendLine("[RMB] - Drop");
                        break;
                    case TooltipHintType.UseAll:
                        sb.AppendLine("[LShift + LMB] - Use All");
                        break;
                    case TooltipHintType.DropAll:
                        sb.AppendLine("[LShift + RMB] - Drop All");
                        break;
                    case TooltipHintType.Transfer:
                        sb.AppendLine("[LMB] - Transfer");
                        break;
                    case TooltipHintType.TransferAll:
                        sb.AppendLine("[LShift + LMB] - Transfer All");
                        break;
                }
            }
            hintText.text = sb.ToString().Substring(0, sb.Length - 1); // remove last blank line
            hintText.gameObject.SetActive(true);
        }

        // show tooltip
        gameObject.SetActive(true);
        //Cursor.visible = false;
        await canvasGroup.DOFade(1, 0.25f).SetDelay(0.25f).SetEase(Ease.OutQuart).AsyncWaitForCompletion();
        // await canvasGroup.DOFade(1, 0.25f).SetEase(Ease.OutQuart).AsyncWaitForCompletion();
        // await canvasGroup.DOFade(1, 0f).SetDelay(0.5f).SetEase(Ease.OutQuart).AsyncWaitForCompletion();
    }

    public async void HideTooltip()
    {
        KillTween();

        // await canvasGroup.DOFade(0, 0.25f).SetEase(Ease.OutQuart).AsyncWaitForCompletion();
        await canvasGroup.DOFade(0, 0f).SetEase(Ease.OutQuart).AsyncWaitForCompletion();
        //Cursor.visible = true;
        gameObject.SetActive(false);
    }

    private void KillTween()
    {
        if (DOTween.IsTweening(canvasGroup, true))
            DOTween.Kill(canvasGroup);
    }
}
