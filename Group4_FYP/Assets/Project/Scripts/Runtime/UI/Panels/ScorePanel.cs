using System.Linq;
using UnityEngine;
using TMPro;
using PathOfHero.Telemetry;

namespace PathOfHero.UI
{
    public class ScorePanel : Panel
    {
        [SerializeField]
        private TMP_Text m_ScoreText;

        [SerializeField]
        private CharacterEntry[] m_CharacterEntries;

        private Coroutine m_SwitchCoroutine;

        public override void OnActivate()
        {
            var dataCollector = DataCollector.Instance;
            if (dataCollector != null)
                m_ScoreText.text = dataCollector.CurrentStats.CalculateScore().ToString();

            var lastPlayerName = PlayerPrefs.GetString("lastPlayerName");
            if (!string.IsNullOrWhiteSpace(lastPlayerName))
            {
                for (int i = 0; i < m_CharacterEntries.Length; i++)
                    m_CharacterEntries[i].Character = char.ToUpper(lastPlayerName[i]);
            }
        }

        public void OnSubmit()
        {
            if (m_SwitchCoroutine != null)
                return;

            var playerName = new string(m_CharacterEntries.Select(e => e.Character).ToArray());
            PlayerPrefs.SetString("lastPlayerName", playerName);

            var dataCollector = DataCollector.Instance;
            if (dataCollector != null)
                dataCollector.UploadSession(playerName);

            m_SwitchCoroutine = StartCoroutine(m_PanelManager.SwitchPanel(PanelType.End));
        }
    }
}
