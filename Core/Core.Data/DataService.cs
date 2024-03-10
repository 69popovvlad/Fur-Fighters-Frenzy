using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace Core.Data
{
    public static class DataService
    {
        private static string _encryptKey;
        private static string _localPath;
        private static bool _initialized;

        public static string FileFormat { get; private set; }

        public static string LocalPath =>
            string.IsNullOrEmpty(_localPath) ? _localPath = GetPath("src/data") : _localPath;

        public static void Initialize(string format, string key)
        {
            if (_initialized)
            {
                throw new Exception($"{nameof(DataService)} already initialized");
            }

            _initialized = true;

            FileFormat = format;
            _encryptKey = key;
        }
        
        public static T LoadOrCreateLocalData<T>(string name) where T : LocalData, new() =>
            LoadOrCreateData<T>(LocalPath, name);

        public static T LoadOrCreateData<T>(string path, string name) where T : LocalData, new()
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var profileData = default(T);
            var filePath = $"{path}/{name}{FileFormat}";
            if (File.Exists(filePath))
            {
                profileData = Load<T>(filePath);
                profileData.SetFilename(name);
                profileData.OnLoad();
            }
            else
            {
                profileData = new T();
                profileData.SetFilename(name);
                profileData.OnCreate();
                profileData.OnLoad();
                profileData.Save();
            }

            return profileData;
        }

        public static void Save<T>(T data, string path)
        {
            using var streamWriter = new StreamWriter(path);
            var content = JsonConvert.SerializeObject(data, Formatting.Indented);

            streamWriter.Write(EncryptOrDecrypt(content));
        }

        private static T Load<T>(string path)
        {
            using var streamReader = new StreamReader(path);
            var content = streamReader.ReadToEnd();

            return JsonConvert.DeserializeObject<T>(EncryptOrDecrypt(content));
        }

        private static string EncryptOrDecrypt(string text)
        {
            var result = new StringBuilder();

            for (var c = 0; c < text.Length; c++)
            {
                result.Append((char)(text[c] ^ (uint)_encryptKey[c % _encryptKey.Length]));
            }

            return result.ToString();
        }

        private static string GetPath(string folder)
        {
            var persistentPath = UnityEngine.Application.persistentDataPath;
            if (!Directory.Exists(persistentPath))
            {
                Directory.CreateDirectory(persistentPath);
            }
            
            var fullPath = persistentPath + $@"/{folder}";
            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }

            return fullPath;
        }
    }
}