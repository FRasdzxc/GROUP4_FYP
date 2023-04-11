using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using PathOfHero.Utilities;
using System.Collections.Generic;

namespace PathOfHero.Telemetry
{
    public class DataCollector : SingletonPersistent<DataCollector>
    {
        private const string k_TelemetryEndpoint = "127.0.0.1";
        private SessionStats m_CurrentStats;
        public SessionStats CurrentStats => m_CurrentStats;

        public void StartNewSession()
        {
            m_CurrentStats = new SessionStats();
        }

        public void UploadSession()
        {
            if (m_CurrentStats == null)
                return;

            StartCoroutine(UploadSession(m_CurrentStats));
        }

        public void StepsTaken()
            => m_CurrentStats.stepsTaken += 1;

        public void WeaponUsed(string weponName)
        {
            if (string.IsNullOrWhiteSpace(weponName))
                return;

            if (!m_CurrentStats.weaponUsage.ContainsKey(weponName))
                m_CurrentStats.weaponUsage[weponName] = 0;

            m_CurrentStats.weaponUsage[weponName]++;
        }

        public void MobsKilled(string mobName)
        {
            if (string.IsNullOrWhiteSpace(mobName))
                return;

            if (!m_CurrentStats.mobsKilled.ContainsKey(mobName))
                m_CurrentStats.mobsKilled[mobName] = 0;

            m_CurrentStats.mobsKilled[mobName]++;
        }

        private IEnumerator UploadSession(SessionStats session)
        {
            var form = session.ToForm();
            using UnityWebRequest www = UnityWebRequest.Post(k_TelemetryEndpoint, form);
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
                Debug.LogWarning($"[Data Collector] Upload session stats failed\n{www.result}: {www.error}");
            else
                Debug.Log($"[Data Collector] Uploaded session stats successfully.");
        }

        public class SessionStats
        {
            public int stepsTaken;
            public Dictionary<string, int> weaponUsage;
            public Dictionary<string, int> mobsKilled;

            public SessionStats()
            {
                weaponUsage = new();
                mobsKilled = new();
            }

            public WWWForm ToForm()
            {
                var retVal = new WWWForm();
                retVal.AddField("timestamp", DateTime.Now.Ticks.ToString());
                foreach (var entry in weaponUsage)
                    retVal.AddField($"weapon:{entry.Key}", entry.Value);
                foreach (var entry in mobsKilled)
                    retVal.AddField($"mob:{entry.Key}", entry.Value);

                return retVal;
            }
        }
    }
}
