using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

public class MaskingCanvas : MonoBehaviour
{
    [SerializeField] private Transform mask;

    private static MaskingCanvas instance;
    public static MaskingCanvas Instance
    {
        get
        {
            return instance;
        }
    }

    void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        gameObject.SetActive(false);
    }

    public async Task ShowMaskingCanvas(bool value)
    {
        if (value)
        {
            gameObject.SetActive(true);
            await mask.DOScale(Vector2.zero, 0.5f).SetEase(Ease.OutQuart).AsyncWaitForCompletion();
        }
        else
        {
            await mask.DOScale(Vector2.one, 0.5f).SetEase(Ease.InQuart).AsyncWaitForCompletion();
            gameObject.SetActive(false);
        }
    }
}
