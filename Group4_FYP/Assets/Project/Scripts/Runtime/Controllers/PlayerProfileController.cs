using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using PathOfHero.Save;

namespace PathOfHero.Controllers
{
    [CreateAssetMenu(fileName = "PlayerProfileController", menuName = "Path of Hero/Controller/Player Profile")]
    public class PlayerProfileController : ScriptableObject
    {
        [SerializeField]
        private PlayerProfile m_CurrentProfile;
        public PlayerProfile CurrentProfile => m_CurrentProfile;
        public PlayerProfile[] Profiles
        {
            get
            {
                if (!Directory.Exists(k_SaveDataPath))
                    return Array.Empty<PlayerProfile>();

                var info = new DirectoryInfo(k_SaveDataPath);
                var files = info.GetFiles($"*{k_SaveDataExt}");

                var result = new List<PlayerProfile>();
                foreach (var file in files)
                {
                    var json = File.ReadAllText(file.FullName);
                    result.Add(JsonUtility.FromJson<PlayerProfile>(json));
                }
                return result.ToArray();
            }
        }

        private static string k_SaveDataPath;
        private static string k_SaveDataExt;

        private void Awake()
        {
            m_CurrentProfile = new();
            k_SaveDataPath = Path.Combine(Application.persistentDataPath, "Profiles");
            k_SaveDataExt = ".heroprofile";
        }

        public void LoadFromObject(PlayerProfile profile) => m_CurrentProfile.Load(profile);
        public bool LoadFromDisk(string profileName)
        {
            if (string.IsNullOrWhiteSpace(profileName))
            {
                Debug.LogWarning("[PlayerProfile Controller] Invalid profile name.");
                return false;
            }

            var fullpath = BuildPath(profileName);
            if (!File.Exists(fullpath))
            {
                Debug.LogError($"[PlayerProfile Controller] PlayerProfile name '{profileName}' does not exist.");
                return false;
            }

            var json = File.ReadAllText(fullpath);
            m_CurrentProfile.Load(json);
            return true;
        }
        public void SaveToDisk()
        {
            if (string.IsNullOrWhiteSpace(m_CurrentProfile.name))
            {
                Debug.LogWarning("[PlayerProfile Controller] Please name your profile before saving.");
                return;
            }

            var fullpath = BuildPath(m_CurrentProfile.name);
            if (File.Exists(fullpath))
            {
                var backupPath = BuildPath($"{m_CurrentProfile.name}.bk");
                if (File.Exists(backupPath))
                    File.Delete(backupPath);

                File.Move(fullpath, backupPath);
            }

            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fullpath));
                File.WriteAllText(fullpath, m_CurrentProfile.ToJson());
            }
            catch (Exception e)
            {
                Debug.LogError($"[PlayerProfile Controller] Error saving file {fullpath} ({e.GetType()}: {e.Message})");
            }
        }
        public void DeleteFromDisk(string profileName)
        {
            if (string.IsNullOrWhiteSpace(profileName))
            {
                Debug.LogWarning("[PlayerProfile Controller] Invalid profile name.");
                return;
            }

            var fullpath = BuildPath(profileName);
            if (!File.Exists(fullpath))
            {
                Debug.LogError($"[PlayerProfile Controller] PlayerProfile name '{profileName}' does not exist.");
                return;
            }

            File.Delete(fullpath);
        }

        private string BuildPath(string profileName)
        {
            var result = Path.Combine(k_SaveDataPath, profileName);
            result = Path.ChangeExtension(result, k_SaveDataExt);
            return result;
        }
    }
}
