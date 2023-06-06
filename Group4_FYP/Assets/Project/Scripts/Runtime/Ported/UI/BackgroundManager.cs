using System;
using UnityEngine;
using DG.Tweening;

[Serializable]
public class TilemapBackgroundEntry
{
    public GameObject tilemapBackground;

    [Range(0f, 1f)]
    public float uiAlpha;
}

public class BackgroundManager : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup backgroundCanvasGroup;

    [SerializeField]
    private Transform tilemapBackgroundContainer;

    [SerializeField]
    private TilemapBackgroundEntry[] tilemapBackgrounds;

    private float defaultBackgroundCanvasGroupAlphaValue;

    private int currentTilemapBackgroundIndex;

    private int lastTilemapBackgroundIndex = -1;

    void Start()
    {
        defaultBackgroundCanvasGroupAlphaValue = backgroundCanvasGroup.alpha;
        ChangeTilemapBackground(0f);
    }

    void OnEnable()
        => AudioManager.onMusicStop += () => ChangeTilemapBackground();

    void OnDisable()
        => AudioManager.onMusicStop -= () => ChangeTilemapBackground();

    private async void ChangeTilemapBackground(float duration = 1f)
    {
        if (tilemapBackgrounds.Length <= 0 || !tilemapBackgroundContainer)
            return;

        await backgroundCanvasGroup.DOFade(1, duration).SetEase(Ease.OutQuart).AsyncWaitForCompletion();

        do
            currentTilemapBackgroundIndex = UnityEngine.Random.Range(0, tilemapBackgrounds.Length);
        while (tilemapBackgrounds.Length > 1 && currentTilemapBackgroundIndex == lastTilemapBackgroundIndex);

        foreach (Transform child in tilemapBackgroundContainer)
            Destroy(child.gameObject);
        Instantiate(tilemapBackgrounds[currentTilemapBackgroundIndex].tilemapBackground, tilemapBackgroundContainer);
        lastTilemapBackgroundIndex = currentTilemapBackgroundIndex;

        await backgroundCanvasGroup.DOFade(tilemapBackgrounds[currentTilemapBackgroundIndex].uiAlpha, duration).SetEase(Ease.OutQuart).AsyncWaitForCompletion();
    }
}
