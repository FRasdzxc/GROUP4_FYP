using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using PathOfHero.Managers.Data;
using PathOfHero.PersistentData;
using PathOfHero.Utilities;

namespace PathOfHero.Managers
{
    public class ScoreManager : Singleton<ScoreManager>
    {
        private static readonly string k_Endpoint = "http://127.0.0.1:8000";
        private static readonly string k_RecordEndpoint = "/api/v1/poh/record";
        private static readonly Encoding k_Encoding = new UTF8Encoding();

        [SerializeField]
        private HeroProfile m_HeroProfile;

        [SerializeField]
        private ScoreEventChannel m_EventChannel;

        private bool m_InLevel;
        private SessionStats m_PrevStats;
        private SessionStats m_CurrentStats;

        public bool InLevel => m_InLevel;
        public SessionStats CurrentStats => m_CurrentStats;
        public SessionStats PrevStats => m_PrevStats;

        private void OnEnable()
        {
            m_EventChannel.OnLevelStart += LevelStarted;
            m_EventChannel.OnLevelComplete += LevelComplete;

            m_EventChannel.OnStepTaken += StepsTaken;
            m_EventChannel.OnWeaponUsed += WeaponUsed;
            m_EventChannel.OnAbilityUsed += AbilityUsed;
            m_EventChannel.OnDamageTaken += DamageTaken;
            m_EventChannel.OnDamageGiven += DamageGiven;
            m_EventChannel.OnMobKilled += MobKilled;
        }

        private void OnDisable()
        {
            m_EventChannel.OnLevelStart -= LevelStarted;
            m_EventChannel.OnLevelComplete -= LevelComplete;

            m_EventChannel.OnStepTaken -= StepsTaken;
            m_EventChannel.OnWeaponUsed -= WeaponUsed;
            m_EventChannel.OnAbilityUsed -= AbilityUsed;
            m_EventChannel.OnDamageTaken -= DamageTaken;
            m_EventChannel.OnDamageGiven -= DamageGiven;
            m_EventChannel.OnMobKilled -= MobKilled;
        }

        private void Start()
        {
            // Expose default value
            if (string.IsNullOrWhiteSpace(PlayerPrefs.GetString("recordsEndpoint", null)))
                PlayerPrefs.SetString("recordsEndpoint", k_Endpoint);

#if UNITY_EDITOR
            // Debug use only
            m_CurrentStats = new SessionStats()
            {
                playerName = "Editor",
                mapId = "map_dungeoni",

                timeTaken = 42.0f
            };
            StartCoroutine(UploadSession(m_CurrentStats));
#endif
        }

        private void Update()
        {
            if (!m_InLevel)
                return;

            m_CurrentStats.timeTaken += Time.deltaTime;
        }

        private void LevelStarted(string mapId)
        {
            if (m_InLevel)
                return;

            m_CurrentStats = new() { 
                mapId = mapId,
                playerName = m_HeroProfile.DisplayName
            };
            m_InLevel = true;
        }

        private void LevelComplete(bool success)
        {
            if (!m_InLevel)
                return;

            m_InLevel = false;
            if (!success)
                m_CurrentStats = null;
        }

        private void StepsTaken()
        {
            if (!m_InLevel)
                return;

            m_CurrentStats.stepsTaken++;
        }

        private void DamageGiven(float amount)
        {
            if (!m_InLevel)
                return;

            m_CurrentStats.damageGiven += amount;
        }

        private void DamageTaken(float amount)
        {
            if (!m_InLevel)
                return;

            m_CurrentStats.damageTaken += amount;
        }

        private void WeaponUsed(string weponName)
        {
            if (!m_InLevel || string.IsNullOrWhiteSpace(weponName))
                return;

            if (!m_CurrentStats.weaponUsage.ContainsKey(weponName))
                m_CurrentStats.weaponUsage[weponName] = 0;

            m_CurrentStats.weaponUsage[weponName]++;
        }

        private void AbilityUsed(string abilityName)
        {
            if (!m_InLevel || string.IsNullOrWhiteSpace(abilityName))
                return;

            if (!m_CurrentStats.abilityUsage.ContainsKey(abilityName))
                m_CurrentStats.abilityUsage[abilityName] = 0;

            m_CurrentStats.abilityUsage[abilityName]++;
        }

        private void MobKilled(string mobName)
        {
            if (string.IsNullOrWhiteSpace(mobName))
                return;

            // Check if the mob is a chest - can be done after beating a level
            if (mobName.EndsWith("Chest"))
            {
                if (!m_CurrentStats.chestsFound.ContainsKey(mobName))
                    m_CurrentStats.chestsFound[mobName] = 0;

                m_CurrentStats.chestsFound[mobName]++;
            }
            else if (m_InLevel)
            {
                if (!m_CurrentStats.mobsKilled.ContainsKey(mobName))
                    m_CurrentStats.mobsKilled[mobName] = 0;

                m_CurrentStats.mobsKilled[mobName]++;
            }
        }

        public void UploadRecord(SessionStats session)
            => StartCoroutine(UploadSession(session));

        private IEnumerator UploadSession(SessionStats session)
        {
            // NOTE: Hack for gradshow - allow changing of endpoint address without rebuilding the game 
            var endpoint = PlayerPrefs.GetString("recordsEndpoint", k_Endpoint);
            var rawJson = JsonConvert.SerializeObject(session);

            // Create a custom web request that send JSON body
            using UnityWebRequest www = new UnityWebRequest($"{endpoint}{k_RecordEndpoint}", "POST");
            www.uploadHandler = new UploadHandlerRaw(k_Encoding.GetBytes(rawJson));
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
                Debug.LogWarning($"[Score Manager] Upload session stats to {endpoint} failed\n{www.result}: {www.error}");
            else
            {
                Debug.Log($"[Score Manager] Uploaded session stats successfully.");
                m_PrevStats = session;
                m_CurrentStats = null;
            }
        }

        [System.Serializable]
        public class SessionStats
        {
            public string mapId;
            public string playerName;
            public float timeTaken;
            public int stepsTaken;
            public float damageGiven;
            public float damageTaken;
            public Dictionary<string, int> weaponUsage;
            public Dictionary<string, int> abilityUsage;
            public Dictionary<string, int> mobsKilled;
            public Dictionary<string, int> chestsFound;

            [JsonIgnore]
            public int WeaponUsage => weaponUsage.Sum(w => w.Value);
            [JsonIgnore]
            public int AbilityUsage => abilityUsage.Sum(a => a.Value);
            [JsonIgnore]
            public int MobsKilled => mobsKilled.Sum(m => m.Value);
            [JsonIgnore]
            public int ChestsFound => chestsFound.Sum(d => d.Value);

            public SessionStats()
            {
                weaponUsage = new();
                abilityUsage = new();
                mobsKilled = new();
                chestsFound = new();
            }
        }
    }
}
