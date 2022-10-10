using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class UIAnimationController : MonoBehaviour
{
    public async Task TweenPosition(GameObject gobj, float duration, Vector2 targetPosition, EasingType easingType)
    {
        float elaspedTime = 0;
        Vector2 originalPosition = gobj.GetComponent<RectTransform>().anchoredPosition;

        while (elaspedTime < duration)
        {
            elaspedTime += Time.deltaTime;
            //gobj.GetComponent<RectTransform>().anchoredPosition = Vector2.LerpUnclamped(originalPosition, targetPosition, );

            await Task.Yield();
        }
    }

    public async Task TweenScale(GameObject gobj, float duration, Vector2 targetScale, EasingType easingType)
    {
        float elaspedTime = 0;
        Vector2 originalScale = gobj.GetComponent<RectTransform>().localScale;

        while (elaspedTime < duration)
        {
            elaspedTime += Time.deltaTime;
            //gobj.GetComponent<RectTransform>().anchoredPosition = Vector2.LerpUnclamped(originalScale, targetScale, );

            await Task.Yield();
        }
    }

    #region EasingModes
    public float EaseInQuad(float value)
    {
        return value * value;
    }

    public float EaseOutQuad(float value)
    {
        return 1 - (1 - value) * (1 - value);
    }
    #endregion
}
