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
        private const string k_TelemetryEndpoint =
#if UNITY_EDITOR
            "http://127.0.0.1:5000/upload_stats";
#else
            "http://192.168.19.188:8080/upload_stats";
#endif

        private SessionStats m_CurrentStats;
        public SessionStats CurrentStats => m_CurrentStats;

        public void StartNewSession()
        {
            m_CurrentStats = new SessionStats();
        }

        public void UploadSession(string name)
        {
            if (m_CurrentStats == null)
                return;

            m_CurrentStats.playerName = name;
            StartCoroutine(UploadSession(m_CurrentStats));
        }

        public void StepsTaken()
            => m_CurrentStats.stepsTaken += 1;

        public void DamageGiven(float amount)
            => m_CurrentStats.damageGiven += amount;

        public void DamageTaken(float amount)
            => m_CurrentStats.damageTaken += amount;

        public void PlayerDied()
            => m_CurrentStats.deaths += 1;

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

        public void MapVisited(string mapId)
        {
            if (string.IsNullOrWhiteSpace(mapId))
                return;

            if (m_CurrentStats.mapsVisited.Contains(mapId))
                return;

            m_CurrentStats.mapsVisited.Add(mapId);
        }

        public void DungeonCleared(string mapId)
        {
            if (string.IsNullOrWhiteSpace(mapId))
                return;

            if (!m_CurrentStats.dungeonsCleared.ContainsKey(mapId))
                m_CurrentStats.dungeonsCleared[mapId] = 0;

            m_CurrentStats.dungeonsCleared[mapId]++;
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
            public string playerName;
            public int stepsTaken;
            public float damageGiven;
            public float damageTaken;
            public int deaths;
            public Dictionary<string, int> weaponUsage;
            public Dictionary<string, int> abilityUsage;
            public Dictionary<string, int> mobsKilled;

            public List<string> mapsVisited;
            public Dictionary<string, int> dungeonsCleared;

            public int WeaponUsage => weaponUsage.Sum(w => w.Value);
            public int AbilityUsage => abilityUsage.Sum(a => a.Value);
            public int MobsKilled => mobsKilled.Sum(m => m.Value);
            public int DungeonsCleared => dungeonsCleared.Sum(d => d.Value);

            public SessionStats()
            {
                weaponUsage = new();
                abilityUsage = new();
                mobsKilled = new();
                mapsVisited = new();
                dungeonsCleared = new();
            }

            public int CalculateScore()
            {
                int score = 0;
                score += stepsTaken / 100;
                score += Mathf.CeilToInt(damageGiven / 25f);
                foreach (var entry in mobsKilled)
                    score += entry.Value * 5;
                foreach (var entry in dungeonsCleared)
                    score += entry.Value * 25;
                return score;
            }

            public WWWForm ToForm()
            {
                var retVal = new WWWForm();
                retVal.AddField("playerName", playerName);
                retVal.AddField("finalScore", CalculateScore());
                retVal.AddField("stepsTaken", stepsTaken);
                retVal.AddField("damageGiven", damageGiven.ToString());
                retVal.AddField("damageTaken", damageTaken.ToString());
                retVal.AddField("weaponUsage", WeaponUsage);
                retVal.AddField("abilityUsage", AbilityUsage);
                retVal.AddField("mobKilled", MobsKilled);
                retVal.AddField("dungeonsCleared", DungeonsCleared);
                var json = JsonConvert.SerializeObject(weaponUsage);
                retVal.AddField("weaponUsageDetail", json);
                json = JsonConvert.SerializeObject(abilityUsage);
                retVal.AddField("abilityUsageDetail", json);
                json = JsonConvert.SerializeObject(mobsKilled);
                retVal.AddField("mobKilledDetail", json);
                json = JsonConvert.SerializeObject(mapsVisited);
                retVal.AddField("mapsVisited", json);
                json = JsonConvert.SerializeObject(dungeonsCleared);
                retVal.AddField("dungeonsClearedDetail", json);
                return retVal;
            }
        }
    }
}
