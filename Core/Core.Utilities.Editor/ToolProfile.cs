using System.IO;
using Core.Data;
using UnityEditor;
using UnityEngine;

namespace Core.Utilities.Editor
{
    public static class ToolProfile
    {
        [MenuItem("Tools/Reset Profile")]
        private static void DeletePlayerPrefs()
        {
            DeleteDirectory(DataService.LocalPath);
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }

        public static void DeleteDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
        }
    }
}
