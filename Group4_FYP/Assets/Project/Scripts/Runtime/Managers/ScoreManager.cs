using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using PathOfHero.Managers.Data;
using PathOfHero.PersistentData;

namespace PathOfHero.Managers
{
    public class ScoreManager : MonoBehaviour
    {
        private static readonly string k_Endpoint = "http://127.0.0.1:8000/api/v1/poh";
        private static readonly string k_RecordEndpoint = k_Endpoint + "/record";
        private static readonly Encoding k_Encoding = new UTF8Encoding();

        [SerializeField]
        private HeroProfile m_HeroProfile;

        [SerializeField]
        private ScoreEventChannel m_EventChannel;

        private bool m_InLevel;
        private SessionStats m_CurrentStats;

        public bool InLevel => m_InLevel;
        public SessionStats CurrentStats => m_CurrentStats;

        private void OnEnable()
        {
            m_EventChannel.OnLevelStart += LevelStarted;
            m_EventChannel.OnLevelEnd += LevelEnded;

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
            m_EventChannel.OnLevelEnd -= LevelEnded;

            m_EventChannel.OnStepTaken -= StepsTaken;
            m_EventChannel.OnWeaponUsed -= WeaponUsed;
            m_EventChannel.OnAbilityUsed -= AbilityUsed;
            m_EventChannel.OnDamageTaken -= DamageTaken;
            m_EventChannel.OnDamageGiven -= DamageGiven;
            m_EventChannel.OnMobKilled -= MobKilled;
        }

#if UNITY_EDITOR
        // Debug use only
        private void Start()
        {
            var session = new SessionStats()
            {
                playerName = "Editor",
                mapId = "map_dungeoni",

                timeTaken = 42.0f
            };
            StartCoroutine(UploadSession(session));
        }
#endif

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

            m_CurrentStats = new() { mapId = mapId };
            m_InLevel = true;
        }

        private void LevelEnded()
        {
            if (!m_InLevel)
                return;

            m_InLevel = false;
            StartCoroutine(UploadSession(m_CurrentStats));
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
            if (!m_InLevel || string.IsNullOrWhiteSpace(mobName))
                return;

            // Check if the mob is a chest
            if (mobName.EndsWith("Chest"))
            {
                if (!m_CurrentStats.chestsFound.ContainsKey(mobName))
                    m_CurrentStats.chestsFound[mobName] = 0;

                m_CurrentStats.chestsFound[mobName]++;
            }
            else
            {
                if (!m_CurrentStats.mobsKilled.ContainsKey(mobName))
                    m_CurrentStats.mobsKilled[mobName] = 0;

                m_CurrentStats.mobsKilled[mobName]++;
            }
        }

        private IEnumerator UploadSession(SessionStats session)
        {
            session.playerName = m_HeroProfile.DisplayName;

            var rawJson = JsonConvert.SerializeObject(session);

            // Create a custom web request that send JSON body
            using UnityWebRequest www = new UnityWebRequest($"{k_RecordEndpoint}", "POST");
            www.uploadHandler = new UploadHandlerRaw(k_Encoding.GetBytes(rawJson));
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
                Debug.LogWarning($"[Score Manager] Upload session stats failed\n{www.result}: {www.error}");
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
