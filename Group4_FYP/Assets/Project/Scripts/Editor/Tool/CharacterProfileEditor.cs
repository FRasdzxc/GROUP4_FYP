using System;
using UnityEngine;
using UnityEditor;
using PathOfHero.Characters.Data;

namespace PathOfHero.Tool
{
    [CustomEditor(typeof(CharacterProfile))]
    public class CharacterProfileEditor : Editor
    {
        private SerializedProperty m_Class;
        private SerializedProperty m_MovementType;
        private SerializedProperty m_MoveSpeed;
        private SerializedProperty m_SprintFactor;
        private SerializedProperty m_StepStartDelay;
        private SerializedProperty m_StepDistance;
        private SerializedProperty m_StepTime;
        private SerializedProperty m_CanPickupItems;
        private SerializedProperty m_BaseHealth;
        private SerializedProperty m_DamageEffectTime;
        private SerializedProperty m_AIControllable;
        private SerializedProperty m_DetectionRadius;
        private SerializedProperty m_AttackDistance;

        private void OnEnable()
        {
            m_Class = serializedObject.FindProperty("m_Class");
            m_MovementType = serializedObject.FindProperty("m_MovementType");
            m_MoveSpeed = serializedObject.FindProperty("m_MoveSpeed");
            m_SprintFactor = serializedObject.FindProperty("m_SprintFactor");
            m_StepStartDelay = serializedObject.FindProperty("m_StepStartDelay");
            m_StepDistance = serializedObject.FindProperty("m_StepDistance");
            m_StepTime = serializedObject.FindProperty("m_StepTime");
            m_CanPickupItems = serializedObject.FindProperty("m_CanPickupItems");
            m_BaseHealth = serializedObject.FindProperty("m_BaseHealth");
            m_DamageEffectTime = serializedObject.FindProperty("m_DamageEffectTime");
            m_AIControllable = serializedObject.FindProperty("m_AIControllable");
            m_DetectionRadius = serializedObject.FindProperty("m_DetectionRadius");
            m_AttackDistance = serializedObject.FindProperty("m_AttackDistance");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.LabelField("Character", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(m_Class);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Movement", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(m_MovementType);
            switch ((MovementType)m_MovementType.enumValueIndex)
            {
                case MovementType.None:
                    break;
                case MovementType.Linear:
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Linear Movement", EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(m_MoveSpeed);
                    EditorGUILayout.PropertyField(m_SprintFactor);
                    break;
                case MovementType.Step:
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Step Movement", EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(m_StepStartDelay);
                    EditorGUILayout.PropertyField(m_StepDistance);
                    EditorGUILayout.PropertyField(m_StepTime);
                    break;
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Item", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(m_CanPickupItems);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Stats", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(m_BaseHealth);
            EditorGUILayout.PropertyField(m_DamageEffectTime);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("AI", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(m_AIControllable);
            if (m_AIControllable.boolValue)
            {
                EditorGUILayout.PropertyField(m_DetectionRadius);
                EditorGUILayout.PropertyField(m_AttackDistance);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
