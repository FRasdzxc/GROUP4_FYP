using System;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DungeonMapData))]
public class DungeonMapDataEditor : Editor
{
    private SerializedProperty isTimedProperty;
    private SerializedProperty timeLimitProperty;

    void OnEnable()
    {
        isTimedProperty = serializedObject.FindProperty("isTimed");
        timeLimitProperty = serializedObject.FindProperty("timeLimit");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (isTimedProperty.boolValue)
        {
            timeLimitProperty.intValue = EditorGUILayout.IntField(new GUIContent("Time Limit", "Unit: seconds"), timeLimitProperty.intValue);
            EditorGUILayout.HelpBox($"Time Limit - {TimeSpan.FromSeconds(timeLimitProperty.intValue).ToString("mm':'ss")}", MessageType.Info);
        }

        serializedObject.ApplyModifiedProperties();
    }
}

[CustomEditor(typeof(WaveDungeonMapData))]
public class WaveDungeonMapDataEditor : DungeonMapDataEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}

[CustomEditor(typeof(BossDungeonMapData))]
public class BossDungeonMapDataEditor : DungeonMapDataEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}