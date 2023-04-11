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

                int weaponAttacks = 0;
                foreach (var entry in dataCollector.CurrentStats.weaponUsage)
                    weaponAttacks += entry.Value;
                m_WeaponAttacks.text = weaponAttacks.ToString();

                int mobsKilled = 0;
                foreach (var entry in dataCollector.CurrentStats.mobsKilled)
                    mobsKilled += entry.Value;
                m_MobsKilled.text = mobsKilled.ToString();
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
