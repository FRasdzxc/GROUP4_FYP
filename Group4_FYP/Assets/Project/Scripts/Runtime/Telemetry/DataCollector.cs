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
        private const string k_Endpoint =
#if UNITY_EDITOR
            "http://127.0.0.1";
#else
            "http://172.18.38.18";
#endif
        private const string k_ActivationEndpoint = k_Endpoint + "/activation";
        private const string k_TelemetryEndpoint = k_Endpoint + "/stats";

        private Activation m_Activation;
        private SessionStats m_CurrentStats;
        private int? m_UploadedSessionId;

        public SessionStats CurrentStats => m_CurrentStats;

#if UNITY_EDITOR
        private void Start()
        {
            //m_CurrentStats = new SessionStats()
            //{
            //    heroLevel = 2,
            //    expGained = 255,
            //    mapsVisited = new() { "map_tutorial" }
            //};
            //m_Activation = new Activation()
            //{
            //    sessionToken = "deadbeef",
            //    sessionDesc = "Testing"
            //};
            //UploadSession("EDT");
        }
#endif

        private void OnApplicationQuit()
        {
            if (m_UploadedSessionId.HasValue)
            {
#if UNITY_EDITOR
                Debug.Log($"[Data Collector] OpenURL: {k_Endpoint}/stats/{m_UploadedSessionId.Value}");
#else
                Application.OpenURL($"{k_Endpoint}/stats/{m_UploadedSessionId.Value}");
#endif
            }
        }

        public void StartNewSession()
            => m_CurrentStats = new SessionStats();

        public void UploadSession(string name)
        {
            if (m_Activation == null || m_CurrentStats == null)
                return;

            StartCoroutine(UploadSession(m_CurrentStats, name));
        }

        public int HeroLevel(int value)
            => m_CurrentStats.heroLevel = value;

        public void ExpGained(int amount)
            => m_CurrentStats.expGained += amount;

        public void CoinsEarned(int amount)
            => m_CurrentStats.coinsEarned += amount;

        public void CoinsSpent(int amount)
            => m_CurrentStats.coinsSpent += amount;

        public void OrbUpgradeUsed(string type)
        {
            if (string.IsNullOrWhiteSpace(type))
                return;

            if (!m_CurrentStats.orbUpgrades.ContainsKey(type))
                m_CurrentStats.orbUpgrades[type] = 0;

            m_CurrentStats.orbUpgrades[type]++;
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

        public IEnumerator Activate()
        {   
            var userName =
#if UNITY_EDITOR
            $"Editor";
#else
            Environment.UserName;
#endif

            var form = new WWWForm();
            form.AddField("user", userName);
            using UnityWebRequest www = UnityWebRequest.Post(k_ActivationEndpoint, form);
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
#if UNITY_EDITOR
                Debug.LogError($"[Data Collector] Activation failed\n{www.result}: {www.error}");
                yield break;
#else
                Application.OpenURL("https://it.vtc.edu.hk/en/programmes/detail/it114107/");
                Application.Quit();
#endif
            }
            m_Activation = JsonUtility.FromJson<Activation>(www.downloadHandler.text);
        }

        private IEnumerator UploadSession(SessionStats session, string name)
        {
            var form = session.ToForm(m_Activation.sessionToken, name);
            using UnityWebRequest www = UnityWebRequest.Post(k_TelemetryEndpoint, form);
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
                Debug.LogWarning($"[Data Collector] Upload session stats failed\n{www.result}: {www.error}");
            else
            {
                var response = JsonConvert.DeserializeObject<Dictionary<string, int>>(www.downloadHandler.text);
                if (response != null && response.TryGetValue("session_id", out var sessionId))
                {
                    m_UploadedSessionId = sessionId;
                    Debug.Log($"[Data Collector] Uploaded session stats successfully. (ID: {m_UploadedSessionId.Value})");
                }
            }
        }

        [Serializable]
        class Activation
        {
            public string sessionToken;
            public string sessionDesc;
        }

        public class SessionStats
        {
            public int heroLevel;
            public int expGained;

            public int coinsEarned;
            public int coinsSpent;

            public Dictionary<string, int> orbUpgrades;

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
                orbUpgrades = new();
                weaponUsage = new();
                abilityUsage = new();
                mobsKilled = new();
                mapsVisited = new();
                dungeonsCleared = new();
            }

            public int CalculateScore()
            {
                int score = 0;
                score += stepsTaken / 95;
                score += Mathf.CeilToInt(damageGiven / 22.5f);
                foreach (var entry in mobsKilled)
                    score += entry.Value * 6;
                foreach (var entry in dungeonsCleared)
                    score += entry.Value * 23;
                return score;
            }

            public WWWForm ToForm(string sessionToken, string playerName)
            {
                var retVal = new WWWForm();
                retVal.AddField("sessionToken", sessionToken);
                retVal.AddField("playerName", playerName);
                retVal.AddField("finalScore", CalculateScore());
                retVal.AddField("heroLevel", heroLevel);
                retVal.AddField("expGained", expGained);
                retVal.AddField("coinsEarned", coinsEarned);
                retVal.AddField("coinsSpent", coinsSpent);
                if (orbUpgrades.Count > 0)
                {
                    var json = JsonConvert.SerializeObject(orbUpgrades);
                    retVal.AddField("orbUpgrades", json);
                }
                retVal.AddField("stepsTaken", stepsTaken);
                retVal.AddField("damageGiven", damageGiven.ToString());
                retVal.AddField("damageTaken", damageTaken.ToString());
                retVal.AddField("deaths", deaths);
                if (weaponUsage.Count > 0)
                {
                    var json = JsonConvert.SerializeObject(weaponUsage);
                    retVal.AddField("weaponUsage", json);
                }
                if (abilityUsage.Count > 0)
                {
                    var json = JsonConvert.SerializeObject(abilityUsage);
                    retVal.AddField("abilityUsage", json);
                }
                if (mobsKilled.Count > 0)
                {
                    var json = JsonConvert.SerializeObject(mobsKilled);
                    retVal.AddField("mobKilled", json);
                }
                if (mapsVisited.Count > 0)
                {
                    var json = JsonConvert.SerializeObject(mapsVisited);
                    retVal.AddField("mapsVisited", json);
                }
                if (dungeonsCleared.Count > 0)
                {
                    var json = JsonConvert.SerializeObject(dungeonsCleared);
                    retVal.AddField("dungeonsCleared", json);
                }
                return retVal;
            }
        }
    }
}
