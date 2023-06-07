using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
using PathOfHero.Utilities;

public class MaskingCanvas : SingletonPersistent<MaskingCanvas>
{
    [SerializeField] private Transform mask;

    // Start is called before the first frame update
    void Start()
    {
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
