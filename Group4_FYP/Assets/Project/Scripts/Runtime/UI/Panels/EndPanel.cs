using UnityEngine;
using TMPro;
using PathOfHero.Telemetry;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace PathOfHero.UI
{
    public class EndPanel : Panel
    {
        [SerializeField]
        private TMP_Text m_Score;

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

        [SerializeField]
        private TMP_Text m_DungeonsCleared;

        public override void OnActivate()
        {
            var dataCollector = DataCollector.Instance;
            if (dataCollector != null && dataCollector.CurrentStats != null)
            {
                m_Score.text = dataCollector.CurrentStats.CalculateScore().ToString();
                m_StepsTaken.text = dataCollector.CurrentStats.stepsTaken.ToString();
                m_WeaponAttacks.text = dataCollector.CurrentStats.WeaponUsage.ToString();
                m_AbilityAttacks.text = dataCollector.CurrentStats.AbilityUsage.ToString();
                m_DamageGiven.text = dataCollector.CurrentStats.damageGiven.ToString("N0");
                m_DamageTaken.text = dataCollector.CurrentStats.damageTaken.ToString("N0");
                m_MobsKilled.text = dataCollector.CurrentStats.MobsKilled.ToString();
                m_DungeonsCleared.text = dataCollector.CurrentStats.DungeonsCleared.ToString();
            }
        }

        public async void OnExitGame()
        {
            var loadingSceen = LoadingScreen.Instance;
            if (loadingSceen != null)
                await loadingSceen.FadeInAsync();

#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
