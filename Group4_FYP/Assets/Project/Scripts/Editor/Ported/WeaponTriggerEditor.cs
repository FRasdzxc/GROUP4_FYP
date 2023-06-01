using UnityEditor;

[CustomEditor(typeof(WeaponTrigger))]
public class WeaponTriggerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.HelpBox($"Please assign either:\n- 'HeroWeaponTrigger'\n- 'HeroWeaponTriggerStronger'\n- 'MobWeaponTrigger'\n- 'MobWeaponTriggerStronger'\n- 'MobWeaponTriggerDeadly'\ntag to the GameObject this script is attached to.", MessageType.Info);
    }
}
