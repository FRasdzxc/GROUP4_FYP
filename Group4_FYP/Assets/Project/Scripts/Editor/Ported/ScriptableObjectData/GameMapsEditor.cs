using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEditor;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using PathOfHero.Serialization;

[CustomEditor(typeof(GameMaps))]
public class GameMapsEditor : Editor
{
    private JsonSerializerSettings m_JsonSettings;

    private void OnEnable()
    {
        m_JsonSettings = new()
        {
            ContractResolver = new GameMapsContractResolver(),
            Converters = new[] { new StringEnumConverter() },
            Formatting = Formatting.Indented
        };
    }

    public override void OnInspectorGUI()
    {
        serializedObject.UpdateIfRequiredOrScript();
        DrawDefaultInspector();
        if (GUILayout.Button("Export"))
        {
            var path = StorageManager.GetFullPath($"GameMaps.json", "Exports");
            if (!Directory.Exists(Path.GetDirectoryName(path)))
                Directory.CreateDirectory(Path.GetDirectoryName(path));

            var gameMaps = (GameMaps)target;
            var rawJson = JsonConvert.SerializeObject(gameMaps, m_JsonSettings);
            using var fileStream = File.Create(path);
            using var streamWriter = new StreamWriter(fileStream, Encoding.UTF8);
            streamWriter.Write(rawJson);
            Debug.Log($"[GameMaps] Exported to UserStorage/Exports/GameMaps.json");
        }
        serializedObject.ApplyModifiedProperties();
    }
}

public class GameMapsContractResolver : DefaultContractResolver
{
    private static readonly string[] k_AllowedProperties = { 
        "maps", "mapId", "mapName", "mapType", "mapDifficulty", "objective", "playerType"
    };

    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
        var property = base.CreateProperty(member, memberSerialization);
        if (!k_AllowedProperties.Contains(property.PropertyName))
            property.ShouldSerialize = _ => false;
        return property;
    }
}
