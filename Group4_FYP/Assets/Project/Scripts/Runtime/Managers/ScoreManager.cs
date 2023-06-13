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
        private static readonly string k_Endpoint = "http://172.18.39.114:8000";
        //private static readonly string k_Endpoint = "http://localhost:8000";
        private static readonly string k_RecordEndpoint = "/api/v1/poh/record";
        private static readonly Encoding k_Encoding = new UTF8Encoding();

        [SerializeField]
        private HeroProfile m_HeroProfile;

        [SerializeField]
        private ScoreEventChannel m_EventChannel;

        private bool m_InLevel;
        private bool m_Success;
        private SessionStats m_Session;

        public bool InLevel => m_InLevel;
        public bool Success => m_Success;
        public SessionStats Session => m_Session;

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
            // Note: Hack for gradshow - Expose default value
            if (string.IsNullOrWhiteSpace(PlayerPrefs.GetString("recordsEndpoint", null)))
                PlayerPrefs.SetString("recordsEndpoint", k_Endpoint);
        }

        private void FixedUpdate()
        {
            if (!m_InLevel)
                return;

            if (m_Session != null)
                m_Session.timeTaken += Time.fixedDeltaTime;
        }

        private void LevelStarted(string mapId)
        {
            m_Session = new() { 
                mapId = mapId,
                playerName = m_HeroProfile.DisplayName
            };
            m_InLevel = true;
        }

        private void LevelComplete(bool success)
        {
            m_InLevel = false;
            m_Success = success;
        }

        private void StepsTaken()
        {
            if (!m_InLevel)
                return;

            m_Session.stepsTaken++;
        }

        private void DamageGiven(float amount)
        {
            if (!m_InLevel)
                return;

            m_Session.damageGiven += amount;
        }

        private void DamageTaken(float amount)
        {
            if (!m_InLevel)
                return;

            m_Session.damageTaken += amount;
        }

        private void WeaponUsed(string weponName)
        {
            if (!m_InLevel || string.IsNullOrWhiteSpace(weponName))
                return;

            if (!m_Session.weaponUsage.ContainsKey(weponName))
                m_Session.weaponUsage[weponName] = 0;

            m_Session.weaponUsage[weponName]++;
        }

        private void AbilityUsed(string abilityName)
        {
            if (!m_InLevel || string.IsNullOrWhiteSpace(abilityName))
                return;

            if (!m_Session.abilityUsage.ContainsKey(abilityName))
                m_Session.abilityUsage[abilityName] = 0;

            m_Session.abilityUsage[abilityName]++;
        }

        private void MobKilled(string mobName)
        {
            if (string.IsNullOrWhiteSpace(mobName))
                return;

            // Check if the mob is a chest - can be done after beating a level
            if (mobName.EndsWith("Chest"))
            {
                if (!m_Session.chestsFound.ContainsKey(mobName))
                    m_Session.chestsFound[mobName] = 0;

                m_Session.chestsFound[mobName]++;
            }
            else if (m_InLevel)
            {
                if (!m_Session.mobsKilled.ContainsKey(mobName))
                    m_Session.mobsKilled[mobName] = 0;

                m_Session.mobsKilled[mobName]++;
            }
        }

        public void UploadResult(SessionStats session)
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
                Debug.Log($"[Score Manager] Uploaded session stats successfully.");
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
