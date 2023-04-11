using UnityEngine;
using TMPro;
using PathOfHero.Utilities;
using PathOfHero.Telemetry;
using System.Threading.Tasks;

namespace PathOfHero.Controllers
{
    public class DemoController : SingletonPersistent<DemoController>
    {
        [SerializeField]
        private GameObject m_Countdown;

        [SerializeField]
        private TMP_Text m_CountdownText;

        private bool m_Started;
        private float m_TimeRemaining;

        public bool Paused { get; set; }
        public int SecondsRemaining => (int)m_TimeRemaining;

        private void Start()
        {
            m_Countdown.SetActive(false);
        }

        private void Update()
        {
            if (!m_Started || Paused)
                return;

            m_TimeRemaining -= Time.deltaTime;

            var secondsRemaining = SecondsRemaining;
            if (secondsRemaining.ToString() != m_CountdownText.text)
            {
                if (secondsRemaining <= 30)
                    m_CountdownText.color = Color.yellow;

                m_CountdownText.text = secondsRemaining.ToString();
            }

            if (m_TimeRemaining <= 0)
            {
                SceneController.Instance?.ChangeScene(SceneController.k_EndSceneName, false);
                m_Countdown.SetActive(false);
                m_Started = false;
            }
        }

        public async void StartDemo(float seconds)
        {
            DataCollector.Instance?.StartNewSession();

            m_TimeRemaining = seconds;
            m_CountdownText.text = SecondsRemaining.ToString();
            m_CountdownText.color = Color.white;
            m_Started = true;
            Paused = true;

            var sceneController = SceneController.Instance;
            if (sceneController == null)
            {
                Debug.LogError("[Demo Controller] Scene controller missing! Unable to start.");
                return;
            }
            sceneController.ChangeScene(SceneController.k_InGameSceneName, false);
            do
                await Task.Yield();
            while (sceneController.IsLoading);

            m_Countdown.SetActive(true);
            Paused = false;
        }
    }
}
