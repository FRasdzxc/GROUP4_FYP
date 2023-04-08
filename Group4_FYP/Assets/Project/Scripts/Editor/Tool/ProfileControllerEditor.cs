using UnityEngine;
using UnityEditor;
using PathOfHero.Controllers;

namespace PathOfHero.Tool
{
    [CustomEditor(typeof(PlayerProfileController))]
    public class ProfileControllerEditor : Editor
    {
        private SerializedProperty m_CurrentProfile;

        private void OnEnable()
        {
            m_CurrentProfile = serializedObject.FindProperty("m_CurrentProfile");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            GUI.enabled = false;
            EditorGUILayout.PropertyField(m_CurrentProfile);
            GUI.enabled = true;
            if (GUILayout.Button("Save"))
            {
                var controller = target as PlayerProfileController;
                if (controller)
                    controller.SaveToDisk();
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
