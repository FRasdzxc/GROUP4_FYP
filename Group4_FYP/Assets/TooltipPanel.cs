using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TooltipPanel : MonoBehaviour
{
    [SerializeField] private Text headerText;
    [SerializeField] private Text descriptionText;
    [SerializeField] private Text attributeText;

    private static TooltipPanel _instance;
    public static TooltipPanel instance
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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowTooltipPanel(string header, string description = "", string attribute = "")
    {
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
    }
}
