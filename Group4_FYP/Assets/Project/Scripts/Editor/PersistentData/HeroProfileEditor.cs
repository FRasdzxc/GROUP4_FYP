using UnityEditor;

namespace PathOfHero.PersistentData
{
    [CustomEditor(typeof(HeroProfile))]
    public class HeroProfileEditor : Editor
    {
        private SerializedProperty m_DisplayName;

        private SerializedProperty m_Class;
        private SerializedProperty m_Level;
        private SerializedProperty m_Experience;
        private SerializedProperty m_Coins;

        private SerializedProperty m_Health;
        private SerializedProperty m_MaxHealth;
        private SerializedProperty m_HealthRegeneration;

        private SerializedProperty m_Mana;
        private SerializedProperty m_MaxMana;
        private SerializedProperty m_ManaRegeneration;

        private SerializedProperty m_Defense;

        private SerializedProperty m_Orbs;
        private SerializedProperty m_UsedOrbs;

        private SerializedProperty m_ExpGainMultiplierUpgrade;
        private SerializedProperty m_MaxHealthUpgrade;
        private SerializedProperty m_HealthRegenerationUpgrade;
        private SerializedProperty m_MaxManaUpgrade;
        private SerializedProperty m_ManaRegenerationUpgrade;
        private SerializedProperty m_DefenseUpgrade;
        private SerializedProperty m_AbilityDamageUpgrade;

        private SerializedProperty m_Inventory;

        private SerializedProperty m_WeaponId;
        private SerializedProperty m_WeaponTier;

        private void OnEnable()
        {
            m_DisplayName = serializedObject.FindProperty("m_DisplayName");

            m_Class = serializedObject.FindProperty("m_Class");
            m_Level = serializedObject.FindProperty("m_Level");
            m_Experience = serializedObject.FindProperty("m_Experience");
            m_Coins = serializedObject.FindProperty("m_Coins");

            m_Health = serializedObject.FindProperty("m_Health");
            m_MaxHealth = serializedObject.FindProperty("m_MaxHealth");
            m_HealthRegeneration = serializedObject.FindProperty("m_HealthRegeneration");

            m_Mana = serializedObject.FindProperty("m_Mana");
            m_MaxMana = serializedObject.FindProperty("m_MaxMana");
            m_ManaRegeneration = serializedObject.FindProperty("m_ManaRegeneration");

            m_Defense = serializedObject.FindProperty("m_Defense");

            m_Orbs = serializedObject.FindProperty("m_Orbs");
            m_UsedOrbs = serializedObject.FindProperty("m_UsedOrbs");

            m_ExpGainMultiplierUpgrade = serializedObject.FindProperty("m_ExpGainMultiplierUpgrade");
            m_MaxHealthUpgrade = serializedObject.FindProperty("m_MaxHealthUpgrade");
            m_HealthRegenerationUpgrade = serializedObject.FindProperty("m_HealthRegenerationUpgrade");
            m_MaxManaUpgrade = serializedObject.FindProperty("m_MaxManaUpgrade");
            m_ManaRegenerationUpgrade = serializedObject.FindProperty("m_ManaRegenerationUpgrade");
            m_DefenseUpgrade = serializedObject.FindProperty("m_DefenseUpgrade");
            m_AbilityDamageUpgrade = serializedObject.FindProperty("m_AbilityDamageUpgrade");

            m_Inventory = serializedObject.FindProperty("m_Inventory");

            m_WeaponId = serializedObject.FindProperty("m_WeaponId");
            m_WeaponTier = serializedObject.FindProperty("m_WeaponTier");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.UpdateIfRequiredOrScript();
            using (new EditorGUI.DisabledScope(true))
            {
                EditorGUILayout.PropertyField(m_DisplayName);
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Hero", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(m_Class);
                EditorGUILayout.PropertyField(m_Level);
                EditorGUILayout.PropertyField(m_Experience);
                EditorGUILayout.PropertyField(m_Coins);
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Health", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(m_Health);
                EditorGUILayout.PropertyField(m_MaxHealth);
                EditorGUILayout.PropertyField(m_HealthRegeneration);
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Mana", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(m_Mana);
                EditorGUILayout.PropertyField(m_MaxMana);
                EditorGUILayout.PropertyField(m_ManaRegeneration);
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Defense", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(m_Defense);
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Orbs", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(m_Orbs);
                EditorGUILayout.PropertyField(m_UsedOrbs);
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Upgrades", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(m_ExpGainMultiplierUpgrade);
                EditorGUILayout.PropertyField(m_MaxHealthUpgrade);
                EditorGUILayout.PropertyField(m_HealthRegenerationUpgrade);
                EditorGUILayout.PropertyField(m_MaxManaUpgrade);
                EditorGUILayout.PropertyField(m_ManaRegenerationUpgrade);
                EditorGUILayout.PropertyField(m_DefenseUpgrade);
                EditorGUILayout.PropertyField(m_AbilityDamageUpgrade);
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Weapon", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(m_WeaponId);
                EditorGUILayout.PropertyField(m_WeaponTier);
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Inventory", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(m_Inventory);
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}
