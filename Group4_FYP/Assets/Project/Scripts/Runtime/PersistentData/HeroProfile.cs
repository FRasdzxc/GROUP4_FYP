using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using PathOfHero.Serialization;
using PathOfHero.Others;

#if UNITY_EDITOR
using System.IO;
using UnityEditor;
#endif

namespace PathOfHero.PersistentData
{
    [CreateAssetMenu(fileName = "NewHeroProfile", menuName = "Path of Hero/Hero Profile")]
    public class HeroProfile : ScriptableObject
    {
        private static readonly string k_SubFolder = "HeroProfiles";
        private static readonly string k_FileExt = ".heroprofile";

        public static bool CanCreate(string name)
        {
            if (StorageManager.FileExists($"{name}{k_FileExt}", k_SubFolder))
            {
                _ = Notification.Instance.ShowNotificationAsync($"'<color={CustomColorStrings.green}>{name}</color>' is not available, please use another name");
                return false;
            }

            return true;
        }

        public static bool Create(string name, HeroClass heroClass, HeroData classDefault, out HeroProfile createdProfile)
        {
            if (!CanCreate(name))
            {
                createdProfile = null;
                return false;
            }

            createdProfile = CreateInstance<HeroProfile>();
            createdProfile.DisplayName = name;

            createdProfile.Class = heroClass;
            createdProfile.Level = 1;
            createdProfile.BadgeObtained = false;

            createdProfile.Health = classDefault.health;
            createdProfile.MaxHealth = classDefault.health;
            createdProfile.HealthRegeneration = classDefault.healthRegeneration;

            createdProfile.Mana = classDefault.mana;
            createdProfile.MaxMana = classDefault.mana;
            createdProfile.ManaRegeneration = classDefault.manaRegeneration;

            createdProfile.Defense = classDefault.defense;

            createdProfile.ExpGainMultiplierUpgrade = 1;

            createdProfile.MapId = "map_town";
            
            createdProfile.Inventory = new();

            switch (heroClass)
            {
                case HeroClass.Mage:
                    createdProfile.WeaponId = "weapon_wand";
                    break;
            }

            createdProfile.SaveToFile();
            _ = Notification.Instance.ShowNotificationAsync($"Successfully created Profile '<color={CustomColorStrings.green}>{createdProfile.DisplayName}</color>'!");
            return true;
        }

        public static bool Update(string name, string newName)
        {
            if (name == newName)
            {
                _ = Notification.Instance.ShowNotificationAsync("No changes detected");
                return true;
            }

            if (StorageManager.FileExists($"{newName}{k_FileExt}", k_SubFolder))
            {
                _ = Notification.Instance.ShowNotificationAsync($"'<color={CustomColorStrings.green}>{newName}</color>' is not available, please use another name");
                return false;
            }

            var profile = CreateInstance<HeroProfile>();
            if (!StorageManager.ReadFile(profile, $"{name}{k_FileExt}", k_SubFolder))
            {
                _ = Notification.Instance.ShowNotificationAsync($"Profile '<color={CustomColorStrings.green}>{name}</color>' does not exist");
                return false;
            }

            StorageManager.RenameFile($"{name}{k_FileExt}", $"{newName}{k_FileExt}.backup", k_SubFolder);
            profile.DisplayName = newName;
            StorageManager.WriteFile(profile, $"{newName}{k_FileExt}", k_SubFolder);
            _ = Notification.Instance.ShowNotificationAsync($"Successfully updated Profile '<color={CustomColorStrings.green}>{name}</color>' to '<color={CustomColorStrings.green}>{newName}</color>'!");
            return true;
        }

        public static bool Delete(string name)
        {
            var result = StorageManager.DeleteFile($"{name}{k_FileExt}", k_SubFolder);
            if (result)
                _ = Notification.Instance.ShowNotificationAsync($"Profile '<color={CustomColorStrings.green}>{name}</color>' deleted");
            else
                _ = Notification.Instance.ShowNotificationAsync($"Profile '<color={CustomColorStrings.green}>{name}</color>' does not exist");
            return result;
        }

        public static HeroProfile[] GetSavedProfiles()
            => StorageManager.ReadFiles<HeroProfile>(k_FileExt, k_SubFolder);

        public UnityAction OnProfileLoaded;

        #region Variables
        [SerializeField]
        private string m_DisplayName;

        // General
        [SerializeField]
        private string m_Class;
        [SerializeField]
        private int m_Level;
        [SerializeField]
        private int m_Experience;
        [SerializeField]
        private int m_Coins;
        [SerializeField]
        private bool m_badgeObtained;

        // Health
        [SerializeField]
        private float m_Health;
        [SerializeField]
        private float m_MaxHealth;
        [SerializeField]
        private float m_HealthRegeneration;

        // Mana
        [SerializeField]
        private float m_Mana;
        [SerializeField]
        private float m_MaxMana;
        [SerializeField]
        private float m_ManaRegeneration;

        // Defense
        [SerializeField]
        private float m_Defense;

        // Orbs
        [SerializeField]
        private int m_Orbs;
        [SerializeField]
        private int m_UsedOrbs;

        // Upgrades
        [SerializeField]
        private float m_ExpGainMultiplierUpgrade;
        [SerializeField]
        private float m_MaxHealthUpgrade;
        [SerializeField]
        private float m_HealthRegenerationUpgrade;
        [SerializeField]
        private float m_MaxManaUpgrade;
        [SerializeField]
        private float m_ManaRegenerationUpgrade;
        [SerializeField]
        private float m_DefenseUpgrade;
        [SerializeField]
        private float m_AbilityDamageUpgrade;

        // Map
        [SerializeField]
        private string m_MapId;

        // Inventroy
        [SerializeField]
        private List<InventoryEntry> m_Inventory;

        // Weapon
        [SerializeField]
        private string m_WeaponId;
        [SerializeField]
        private int m_WeaponTier;
        #endregion

        #region Properties
        public string DisplayName
        {
            get => m_DisplayName;
            set => m_DisplayName = value;
        }
        public HeroClass Class
        {
            get => System.Enum.Parse<HeroClass>(m_Class);
            set => m_Class = value.ToString();
        }
        public int Level
        {
            get => m_Level;
            set => m_Level = value;
        }
        public int Experience
        {
            get => m_Experience;
            set => m_Experience = value;
        }
        public int Coins
        {
            get => m_Coins;
            set => m_Coins = value;
        }
        public bool BadgeObtained
        {
            get => m_badgeObtained;
            set => m_badgeObtained = value;
        }

        public float Health
        {
            get => m_Health;
            set => m_Health = value;
        }
        public float MaxHealth
        {
            get => m_MaxHealth;
            set => m_MaxHealth = value;
        }
        public float HealthRegeneration
        {
            get => m_HealthRegeneration;
            set => m_HealthRegeneration = value;
        }

        public float Mana
        {
            get => m_Mana;
            set => m_Mana = value;
        }
        public float MaxMana
        {
            get => m_MaxMana;
            set => m_MaxMana = value;
        }
        public float ManaRegeneration
        {
            get => m_ManaRegeneration;
            set => m_ManaRegeneration = value;
        }

        public float Defense
        {
            get => m_Defense;
            set => m_Defense = value;
        }

        public int Orbs
        {
            get => m_Orbs;
            set => m_Orbs = value;
        }
        public int UsedOrbs
        {
            get => m_UsedOrbs;
            set => m_UsedOrbs = value;
        }

        public float ExpGainMultiplierUpgrade
        {
            get => m_ExpGainMultiplierUpgrade;
            set => m_ExpGainMultiplierUpgrade = value;
        }
        public float MaxHealthUpgrade
        {
            get => m_MaxHealthUpgrade;
            set => m_MaxHealthUpgrade = value;
        }
        public float HealthRegenerationUpgrade
        {
            get => m_HealthRegenerationUpgrade;
            set => m_HealthRegenerationUpgrade = value;
        }
        public float MaxManaUpgrade
        {
            get => m_MaxManaUpgrade;
            set => m_MaxManaUpgrade = value;
        }
        public float ManaRegenerationUpgrade
        {
            get => m_ManaRegenerationUpgrade;
            set => m_ManaRegenerationUpgrade = value;
        }
        public float DefenseUpgrade
        {
            get => m_DefenseUpgrade;
            set => m_DefenseUpgrade = value;
        }
        public float AbilityDamageUpgrade
        {
            get => m_AbilityDamageUpgrade;
            set => m_AbilityDamageUpgrade = value;
        }

        public string MapId
        {
            get => m_MapId;
            set => m_MapId = value;
        }

        public List<InventoryEntry> Inventory
        {
            get => m_Inventory;
            set => m_Inventory = value;
        }

        public string WeaponId
        {
            get => m_WeaponId;
            set => m_WeaponId = value;
        }
        public int WeaponTier
        {
            get => m_WeaponTier;
            set => m_WeaponTier = value;
        }
        #endregion

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (string.IsNullOrWhiteSpace(m_DisplayName))
                m_DisplayName = Path.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(this));
        }
#endif

        public void LoadFromFile(string profileName)
        {
            if (string.IsNullOrWhiteSpace(profileName))
                return;

            StorageManager.ReadFile(this, $"{profileName}{k_FileExt}", k_SubFolder);
            OnProfileLoaded?.Invoke();
        }

        public void LoadFromAsset(HeroProfile profile)
        {
            m_DisplayName = profile.m_DisplayName;
            m_Class = profile.m_Class;
            m_Level = profile.m_Level;
            m_Experience = profile.m_Experience;
            m_Coins = profile.m_Coins;
            m_badgeObtained = profile.m_badgeObtained;
            m_Health = profile.m_Health;
            m_MaxHealth = profile.m_MaxHealth;
            m_HealthRegeneration = profile.m_HealthRegeneration;
            m_Mana = profile.m_Mana;
            m_MaxMana = profile.m_MaxMana;
            m_ManaRegeneration = profile.m_ManaRegeneration;
            m_Defense = profile.m_Defense;
            m_Orbs = profile.m_Orbs;
            m_UsedOrbs = profile.m_UsedOrbs;
            m_ExpGainMultiplierUpgrade = profile.m_ExpGainMultiplierUpgrade;
            m_MaxHealthUpgrade = profile.m_MaxHealthUpgrade;
            m_HealthRegenerationUpgrade = profile.m_HealthRegenerationUpgrade;
            m_MaxManaUpgrade = profile.m_MaxManaUpgrade;
            m_ManaRegenerationUpgrade = profile.m_ManaRegenerationUpgrade;
            m_DefenseUpgrade = profile.m_DefenseUpgrade;
            m_AbilityDamageUpgrade = profile.m_AbilityDamageUpgrade;
            m_Inventory = profile.m_Inventory;
            m_WeaponId = profile.m_WeaponId;
            m_WeaponTier = profile.m_WeaponTier;
            OnProfileLoaded?.Invoke();
        }

        public void SaveToFile()
        {
            if (string.IsNullOrWhiteSpace(m_DisplayName))
                return;

            StorageManager.WriteFile(this, $"{m_DisplayName}{k_FileExt}", k_SubFolder);
        }
    }
}
