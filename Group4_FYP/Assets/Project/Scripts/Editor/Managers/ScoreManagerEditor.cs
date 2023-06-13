using UnityEditor;

namespace PathOfHero.Managers
{
    [CustomEditor(typeof(ScoreManager))]
    public class ScoreManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.UpdateIfRequiredOrScript();
            DrawDefaultInspector();

            EditorGUILayout.LabelField("State", EditorStyles.boldLabel);
            var instance = target as ScoreManager;
            if (instance.InLevel)
            {
                var stats = instance.Session;
                using (new EditorGUI.DisabledScope(true))
                {
                    EditorGUILayout.TextField("Time Taken", System.TimeSpan.FromSeconds(stats.timeTaken).ToString(@"mm\:ss"));
                    EditorGUILayout.IntField("Steps Taken", stats.stepsTaken);
                    EditorGUILayout.FloatField("Damage Taken", stats.damageTaken);
                    EditorGUILayout.FloatField("Damage Given", stats.damageGiven);
                    EditorGUILayout.IntField("Weapon Usage", stats.WeaponUsage);
                    EditorGUILayout.IntField("Ability Usage", stats.AbilityUsage);
                    EditorGUILayout.IntField("Mobs Killed", stats.MobsKilled);
                    EditorGUILayout.IntField("Chests Found", stats.ChestsFound);
                }
            }
            else
                EditorGUILayout.LabelField("Inactive");

            serializedObject.ApplyModifiedProperties();
        }
    }
}
