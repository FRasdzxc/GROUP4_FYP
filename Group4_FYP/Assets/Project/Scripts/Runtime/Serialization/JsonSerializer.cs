using System.IO;
using System.Text;
using UnityEngine;

namespace PathOfHero.Serialization
{
    public class JsonSerializer : ISerializer
    {
        private static readonly Encoding k_Encoding = Encoding.UTF8;

        public void Serialize(object serializable, FileStream fileStream)
        {
            string rawJson = JsonUtility.ToJson(serializable);
            using var streamWriter = new StreamWriter(fileStream, k_Encoding);
            streamWriter.Write(rawJson);
        }

        public bool Deserialize<T>(T serializable, FileStream fileStream) where T : Object
        {
            try
            {
                using var streamReader = new StreamReader(fileStream, k_Encoding);
                string rawJson = streamReader.ReadToEnd();
                JsonUtility.FromJsonOverwrite(rawJson, serializable);
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"[JSON Serializer] Error deserializing file: {e.GetType()}\n{e.Message}");
                return false;
            }
        }
    }
}
