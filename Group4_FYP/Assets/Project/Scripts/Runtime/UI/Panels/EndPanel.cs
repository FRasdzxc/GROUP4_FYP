using UnityEngine;
using TMPro;
using PathOfHero.Controllers;
using PathOfHero.Telemetry;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace PathOfHero.UI
{
    // This is a standalone panel in EndScene, thus not inherting Panel class
    public class EndPanel : MonoBehaviour
    {
        [SerializeField]
        private CursorController m_CursorController;

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

        private void Start()
        {
            m_CursorController.ChangeCursor(CursorController.CursorType.Default);

            var dataCollector = DataCollector.Instance;
            if (dataCollector != null && dataCollector.CurrentStats != null)
            {
                dataCollector.UploadSession();

                // Configure end screen
                m_StepsTaken.text = dataCollector.CurrentStats.stepsTaken.ToString();
                m_WeaponAttacks.text = dataCollector.CurrentStats.WeaponUsage.ToString();
                m_AbilityAttacks.text = dataCollector.CurrentStats.AbilityUsage.ToString();
                m_DamageGiven.text = dataCollector.CurrentStats.damageGiven.ToString("N0");
                m_DamageTaken.text = dataCollector.CurrentStats.damageTaken.ToString("N0");
                m_MobsKilled.text = dataCollector.CurrentStats.MobsKilled.ToString();
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
