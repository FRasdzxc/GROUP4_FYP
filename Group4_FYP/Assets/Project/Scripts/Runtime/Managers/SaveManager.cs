using UnityEngine;
using UnityEngine.Serialization;
using PathOfHero.PersistentData;
using PathOfHero.Utilities;
using PathOfHero.Others;

namespace PathOfHero.Managers
{
    public class SaveManager : SingletonPersistent<SaveManager>
    {
        [SerializeField]
        private HeroProfile m_RuntimeProfile;

        [SerializeField]
        [FormerlySerializedAs("autosaveDuration")]
        [Tooltip("Interval between auto saves, in minutes.")]
        private float m_AutoSaveInterval;

        private float m_NextAutoSaveUnscaledTime;

        private void Start()
            => m_NextAutoSaveUnscaledTime = Time.unscaledTime + (m_AutoSaveInterval * 60f);

        private void Update()
        {
            var gameManager = GameManager.Instance;
            if (gameManager == null || gameManager.MapType == MapType.Dungeon ||
                Time.unscaledTime < m_NextAutoSaveUnscaledTime)
                return;

            SaveProfile();
            m_NextAutoSaveUnscaledTime = Time.unscaledTime + (m_AutoSaveInterval * 60f);
        }

        public void LoadSelectedProfile()
        {
            m_RuntimeProfile.LoadFromFile(PlayerPrefs.GetString("selectedProfileName"));
        }

        public void SaveProfile(bool showNotification = true, bool accountForMapType = true)
        {
            if (accountForMapType && GameManager.Instance.MapType == MapType.Dungeon)
            {
                _ = Notification.Instance.ShowNotificationAsync("You cannot save game while engaged in a dungeon battle!");
                return;
            }

            m_RuntimeProfile.MapId = GameManager.Instance.MapId;

            m_RuntimeProfile.Health = Hero.Instance.GetHealth();
            m_RuntimeProfile.MaxHealth = Hero.Instance.GetMaxHealth();
            m_RuntimeProfile.HealthRegeneration = Hero.Instance.GetHealthRegeneration();
            m_RuntimeProfile.Defense = Hero.Instance.GetDefense();
            m_RuntimeProfile.Level = Hero.Instance.GetLevel();
            m_RuntimeProfile.Experience = Hero.Instance.GetStoredExp();
            m_RuntimeProfile.Coins = Hero.Instance.GetStoredCoin();
            m_RuntimeProfile.MaxHealthUpgrade = Hero.Instance.GetMaxHealthUpgrade();
            m_RuntimeProfile.HealthRegenerationUpgrade = Hero.Instance.GetHealthRegenerationUpgrade();
            m_RuntimeProfile.DefenseUpgrade = Hero.Instance.GetDefenseUpgrade();
            m_RuntimeProfile.ExpGainMultiplierUpgrade = Hero.Instance.GetExpGainMultiplierUpgrade();

            m_RuntimeProfile.Mana = AbilityManager.Instance.GetMana();
            m_RuntimeProfile.MaxMana = AbilityManager.Instance.GetMaxMana();
            m_RuntimeProfile.ManaRegeneration = AbilityManager.Instance.GetManaRegeneration();
            m_RuntimeProfile.MaxManaUpgrade = AbilityManager.Instance.GetMaxManaUpgrade();
            m_RuntimeProfile.ManaRegenerationUpgrade = AbilityManager.Instance.GetManaRegenerationUpgrade();
            m_RuntimeProfile.AbilityDamageUpgrade = AbilityManager.Instance.GetAbilityDamageUpgrade();

            m_RuntimeProfile.Orbs = Orb.Instance.GetOrbs();
            m_RuntimeProfile.UsedOrbs = Orb.Instance.GetUsedOrbs();

            m_RuntimeProfile.Inventory = Inventory.Instance.GetItems();

            m_RuntimeProfile.WeaponId = WeaponManager.Instance.GetWeaponId();
            m_RuntimeProfile.WeaponTier = WeaponManager.Instance.GetWeaponTier();

            m_RuntimeProfile.SaveToFile();
            m_NextAutoSaveUnscaledTime = Time.unscaledTime + (m_AutoSaveInterval * 60f);
            if (showNotification)
                _ = Notification.Instance.ShowNotificationAsync($"Successfully saved data to Profile '<color={CustomColorStrings.green}>{m_RuntimeProfile.DisplayName}</color>'!");
        }

        public void ApplyProfile()
        {
            GameManager.Instance.MapId = m_RuntimeProfile.MapId;

            Hero.Instance.SetDefense(m_RuntimeProfile.Defense);
            Hero.Instance.SetDefenseUpgrade(m_RuntimeProfile.DefenseUpgrade);
            Hero.Instance.SetExpGainMultiplierUpgrade(m_RuntimeProfile.ExpGainMultiplierUpgrade);
            Hero.Instance.SetHealth(m_RuntimeProfile.Health);
            Hero.Instance.SetHealthRegeneration(m_RuntimeProfile.HealthRegeneration);
            Hero.Instance.SetHealthRegenerationUpgrade(m_RuntimeProfile.HealthRegenerationUpgrade);
            Hero.Instance.SetLevel(m_RuntimeProfile.Level);
            Hero.Instance.SetMaxHealth(m_RuntimeProfile.MaxHealth);
            Hero.Instance.SetMaxHealthUpgrade(m_RuntimeProfile.MaxHealthUpgrade);
            Hero.Instance.SetStoredCoin(m_RuntimeProfile.Coins);
            Hero.Instance.SetStoredExp(m_RuntimeProfile.Experience);

            Orb.Instance.SetOrbs(m_RuntimeProfile.Orbs);
            Orb.Instance.SetUsedOrbs(m_RuntimeProfile.UsedOrbs);
            Orb.Instance.RefreshUpgradeItemContainer();

            Inventory.Instance.SetItems(m_RuntimeProfile.Inventory);

            AbilityManager.Instance.SetAbilityDamageUpgrade(m_RuntimeProfile.AbilityDamageUpgrade);
            AbilityManager.Instance.SetMana(m_RuntimeProfile.Mana);
            AbilityManager.Instance.SetManaRegeneration(m_RuntimeProfile.ManaRegeneration);
            AbilityManager.Instance.SetManaRegenerationUpgrade(m_RuntimeProfile.ManaRegenerationUpgrade);
            AbilityManager.Instance.SetMaxMana(m_RuntimeProfile.MaxMana);
            AbilityManager.Instance.SetMaxManaUpgrade(m_RuntimeProfile.MaxManaUpgrade);

            WeaponManager.Instance.SetWeaponTier(m_RuntimeProfile.Class, m_RuntimeProfile.WeaponId, m_RuntimeProfile.WeaponTier);
        }
    }
}
