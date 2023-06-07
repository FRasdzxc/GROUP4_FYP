using System.Collections;
using UnityEngine;
using TMPro;
using DG.Tweening;
using PathOfHero.Managers;

[RequireComponent(typeof(CanvasGroup))]
public class DungeonResultPanel : Panel
{
    [SerializeField]
    private TMP_Text m_TimeTaken;

    [SerializeField]
    private TMP_Text m_StepsTaken;

    [SerializeField]
    private TMP_Text m_WeaponAttacks;

    [SerializeField]
    private TMP_Text m_AbilityAttacks;

    [SerializeField]
    private TMP_Text m_DamageGiven;

    [SerializeField]
    private TMP_Text m_DamageTaken;

    [SerializeField]
    private TMP_Text m_MobsKilled;

    private CanvasGroup m_CanvasGroup;

    private static DungeonResultPanel s_Instance;
    public static DungeonResultPanel Instance => s_Instance;

    protected override void Awake()
    {
        base.Awake();
        m_CanvasGroup = GetComponent<CanvasGroup>();
        s_Instance = this;
    }

    private void Start()
    {
        m_CanvasGroup.alpha = 0.0f;
        m_CanvasGroup.blocksRaycasts = false;
        m_CanvasGroup.interactable = false;
    }

    public override void HidePanel()
    {
        base.HidePanel();
        StartCoroutine(HideDungeonResultPanel());
    }

    public void ShowDungeonResultPanel(ScoreManager.SessionStats session)
        => StartCoroutine(DisplayDungeonResultPanel(session));

    private IEnumerator DisplayDungeonResultPanel(ScoreManager.SessionStats session)
    {
        ShowPanel();

        m_TimeTaken.text = session.timeTaken.ToString("N0");
        m_StepsTaken.text = session.stepsTaken.ToString();
        m_WeaponAttacks.text = session.WeaponUsage.ToString();
        m_AbilityAttacks.text = session.AbilityUsage.ToString();
        m_DamageGiven.text = session.damageGiven.ToString("N0");
        m_DamageTaken.text = session.damageTaken.ToString("N0");
        m_MobsKilled.text = session.MobsKilled.ToString();

        m_CanvasGroup.blocksRaycasts = true;
        yield return m_CanvasGroup.DOFade(1, 0.25f).SetEase(Ease.OutQuart).WaitForCompletion();
        m_CanvasGroup.interactable = true;
        panelState = PanelState.Shown;
    }

    private IEnumerator HideDungeonResultPanel()
    {
        yield return m_CanvasGroup.DOFade(0, 0.25f).SetEase(Ease.OutQuart).WaitForCompletion();
        m_CanvasGroup.blocksRaycasts = false;
        m_CanvasGroup.interactable = false;
        panelState = PanelState.Hidden;
    }
}
