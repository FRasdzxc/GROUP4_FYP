using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace PathOfHero.Serialization
{
    public static class StorageManager
    {
        private static readonly string k_StoragePath =
#if UNITY_EDITOR
            Path.Combine(System.Environment.CurrentDirectory, "UserStorage");
#else
            Application.persistentDataPath;
#endif

        // Change this to switch to another type of container format (e.g. XML)
        private static readonly ISerializer s_Serializer = new JsonSerializer();

        public static string GetFullPath(string fileName, string subFolder)
            => Path.Combine(k_StoragePath, subFolder, fileName);

        public static bool FileExists(string fileName, string subFolder)
            => File.Exists(GetFullPath(fileName, subFolder));

        public static bool RenameFile(string fileName, string newFileName,  string subFolder)
        {
            string path = Path.Combine(k_StoragePath, subFolder);

            string file = Path.Combine(path, fileName);
            string newFile = Path.Combine(path, newFileName);
            if (!File.Exists(file) || File.Exists(newFile))
                return false;

            File.Move(file, newFile);
            return true;
        }

        public static bool DeleteFile(string fileName, string subFolder)
        {
            string path = GetFullPath(fileName, subFolder);
            if (!File.Exists(path))
                return false;

            File.Delete(path);
            return true;
        }

        public static bool WriteFile<T>(T serializable, string fileName, string subFolder) where T : Object
        {
            try
            {
                string path = Path.Combine(k_StoragePath, subFolder);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                string fullName = Path.Combine(path, fileName);
                if (File.Exists(fullName))
                {
                    string backupFullName = fullName + ".backup";
                    if (File.Exists(backupFullName))
                        File.Delete(backupFullName);

                    File.Move(fullName, backupFullName);
                }

                using var fileStream = File.Create(fullName);
                s_Serializer.Serialize(serializable, fileStream);
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[StorageManager] Unable to write file '{subFolder}/{fileName}': {e.GetType()}\n{e.Message}");
                return false;
            }
        }

        public static bool ReadFile<T>(T serializable, string fileName, string subFolder) where T : Object
        {
            string fullName = GetFullPath(fileName, subFolder);
            if (!File.Exists(fullName))
            {
                Debug.LogWarning($"[StorageManager] File '{subFolder}/{fileName}' does not exist.");
                return false;
            }

            using var fileStream = File.Open(fullName, FileMode.Open);
            return s_Serializer.Deserialize<T>(serializable, fileStream);
        }

        public static T[] ReadFiles<T>(string fileExt, string subFolder) where T : ScriptableObject
        {
            string path = Path.Combine(k_StoragePath, subFolder);
            if (!Directory.Exists(path))
                return System.Array.Empty<T>();

            var filepaths = Directory.GetFiles(path, $"*{fileExt}");
            var result = new List<T>();
            foreach (var filepath in filepaths)
            {
                var instance = ScriptableObject.CreateInstance<T>();
                if (ReadFile(instance, Path.GetFileName(filepath), subFolder))
                    result.Add(instance);
            }
            return result.ToArray();
        }
    }
}
