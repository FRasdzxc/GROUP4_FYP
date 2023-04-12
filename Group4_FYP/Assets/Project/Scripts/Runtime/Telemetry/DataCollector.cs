using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using PathOfHero.Utilities;

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

        public void DamageGiven(float amount)
            => m_CurrentStats.damageGiven += amount;

        public void DamageTaken(float amount)
            => m_CurrentStats.damageTaken += amount;

        public void WeaponUsed(string weponName)
        {
            if (string.IsNullOrWhiteSpace(weponName))
                return;

            if (!m_CurrentStats.weaponUsage.ContainsKey(weponName))
                m_CurrentStats.weaponUsage[weponName] = 0;

            m_CurrentStats.weaponUsage[weponName]++;
        }

        public void AbilityUsed(string abilityName)
        {
            if (string.IsNullOrWhiteSpace(abilityName))
                return;

            if (!m_CurrentStats.abilityUsage.ContainsKey(abilityName))
                m_CurrentStats.abilityUsage[abilityName] = 0;

            m_CurrentStats.abilityUsage[abilityName]++;
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
            public float damageGiven;
            public float damageTaken;
            public Dictionary<string, int> weaponUsage;
            public Dictionary<string, int> abilityUsage;
            public Dictionary<string, int> mobsKilled;

            public int WeaponUsage => weaponUsage.Sum(w => w.Value);
            public int AbilityUsage => abilityUsage.Sum(a => a.Value);
            public int MobsKilled => mobsKilled.Sum(m => m.Value);

            public SessionStats()
            {
                weaponUsage = new();
                abilityUsage = new();
                mobsKilled = new();
            }

            public WWWForm ToForm()
            {
                var retVal = new WWWForm();
                retVal.AddField("stepsTaken", stepsTaken);
                retVal.AddField("damageGiven", damageGiven.ToString());
                retVal.AddField("damageTaken", damageTaken.ToString());
                retVal.AddField("weaponUsage", WeaponUsage);
                retVal.AddField("abilityUsage", AbilityUsage);
                retVal.AddField("mobKilled", MobsKilled);
                var json = JsonConvert.SerializeObject(weaponUsage);
                retVal.AddField("weaponUsageDetail", json);
                json = JsonConvert.SerializeObject(abilityUsage);
                retVal.AddField("abilityUsageDetail", json);
                json = JsonConvert.SerializeObject(mobsKilled);
                retVal.AddField("mobKilledDetail", json);
                return retVal;
            }
        }
    }
}
