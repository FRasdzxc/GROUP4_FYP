using System.Collections;
using UnityEngine;
using DG.Tweening;

public class MinimapCamera : MonoBehaviour
{
    [SerializeField]
    private Camera minimapCamera;

    [SerializeField]
    private float defaultProjectionSize = 12.5f;

    [SerializeField]
    private float expandedProjectionSize = 25f;

    private bool togglingProjectionSize;

    void Start()
        => ToggleProjectionSize(!HUD.Instance.IsMinimapExpanded(), HUD.Instance.GetMinimapToggleDuration());

    void OnEnable()
        => HUD.onMinimapToggled += ToggleProjectionSize;

    void OnDisable()
        => HUD.onMinimapToggled -= ToggleProjectionSize;

    private void ToggleProjectionSize(bool minimapExpanded, float minimapToggleDuration)
        => StartCoroutine(AnimatedProjectionSize(minimapExpanded, minimapToggleDuration));

    private IEnumerator AnimatedProjectionSize(bool minimapExpanded, float minimapToggleDuration)
    {
        if (togglingProjectionSize)
            yield break;

        togglingProjectionSize = true;

        if (minimapExpanded)
            yield return minimapCamera.DOOrthoSize(defaultProjectionSize, minimapToggleDuration).SetEase(Ease.OutQuart).WaitForCompletion();
        else
            yield return minimapCamera.DOOrthoSize(expandedProjectionSize, minimapToggleDuration).SetEase(Ease.OutQuart).WaitForCompletion();

        togglingProjectionSize = false;
    }
}
