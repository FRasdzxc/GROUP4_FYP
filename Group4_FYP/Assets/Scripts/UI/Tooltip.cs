using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Tooltip : MonoBehaviour
{
    [SerializeField] private Text headerText;
    [SerializeField] private Text descriptionText;
    [SerializeField] private Text attributeText;
    [SerializeField] private Text hintText;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private static Tooltip _instance;
    public static Tooltip Instance
    {
        get => _instance;
    }

    void Awake()
    {
        _instance = this;
    }

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
        transform.position = Input.mousePosition;

        float pivotX = transform.position.x / Screen.width;
        float pivotY = transform.position.y / Screen.height;

        rectTransform.pivot = new Vector2(pivotX, pivotY);
    }

    public async void ShowTooltip(string header, string description = "", string attribute = "", string hint = "")
    {
        KillTween();

        headerText.text = header;

        if (description != "")
        {
            descriptionText.gameObject.SetActive(true);
            descriptionText.text = description;
        }
        else
        {
            descriptionText.gameObject.SetActive(false);
        }

        if (attribute != "")
        {
            attributeText.gameObject.SetActive(true);
            attributeText.text = attribute;
        }
        else
        {
            attributeText.gameObject.SetActive(false);
        }

        if (hint != "")
        {
            hintText.gameObject.SetActive(true);
            hintText.text = hint;
        }
        else
        {
            hintText.gameObject.SetActive(false);
        }

        gameObject.SetActive(true);
        //Cursor.visible = false;
        await canvasGroup.DOFade(1, 0.25f).SetDelay(0.5f).SetEase(Ease.OutQuart).AsyncWaitForCompletion();
    }

    // public void ShowTooltip(string header, string description = "", string[] attributes = null, TooltipHintType[] tooltipHintType = null)
    // {
    //     this.header = header;
    //     this.description = description;

    //     if (attributes == null)
    //     {
    //         this.attributes = "";
    //     }
    //     else
    //     {
    //         StringBuilder sb = new StringBuilder();
    //         foreach (var v in attributes)
    //         {
    //             sb.AppendLine(v);
    //         }
    //         this.attributes = sb.ToString().Substring(0, sb.Length - 1);
    //     }

    //     if (tooltipHintType == null)
    //     {
    //         this.hints = "";
    //     }
    //     else
    //     {
    //         StringBuilder sb = new StringBuilder();
    //         foreach (var h in tooltipHintType)
    //         {
    //             sb.Append(h.ToString() + '\t');
    //         }
    //         this.hints = sb.ToString().Substring(0, hints.Length - 1);
    //     }
    // }

    public async void HideTooltip()
    {
        KillTween();

        await canvasGroup.DOFade(0, 0.25f).SetEase(Ease.OutQuart).AsyncWaitForCompletion();
        //Cursor.visible = true;
        gameObject.SetActive(false);
    }

    private void KillTween()
    {
        if (DOTween.IsTweening(canvasGroup, true))
        {
            DOTween.Kill(canvasGroup);
        }
    }
}
