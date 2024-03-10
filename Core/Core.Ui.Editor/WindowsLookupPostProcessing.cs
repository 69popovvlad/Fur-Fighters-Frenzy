#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using Core.Processing.Editor;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace Core.Ui.Editor
{
    [Postprocessor(0)]
    public static class WindowsLookupPostProcessing
    {
        [PostprocessorStep(0), UsedImplicitly]
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets,
            string[] movedAssets, string[] movedFromAssetPaths)
        {
            var guids = AssetDatabase.FindAssets(string.Empty, new[] { WindowsSystem.ConfigDirectory });
            for (int i = 0, length = guids.Length; i < length; ++i)
            {
                var path = AssetDatabase.GUIDToAssetPath(guids[i]);
                var asset = AssetDatabase.LoadAssetAtPath(path, typeof(WindowsLookupConfig)) as WindowsLookupConfig;
                if (asset != null)
                {
                    var lookup = GetWindowsLookup();
                    if (lookup.Count != asset.Lookup.Count)
                    {
                        asset.UpdateLookup(lookup);
                        EditorUtility.SetDirty(asset);
                        AssetDatabase.SaveAssetIfDirty(asset);
                    
                        Debug.Log($"<color=cyan>Windows lookup was updated</color>");
                    }
                    
                    return;
                }
            }

            if (!Directory.Exists(WindowsSystem.ConfigDirectory))
            {
                Directory.CreateDirectory(WindowsSystem.ConfigDirectory);
            }
            
            var config = ScriptableObject.CreateInstance<WindowsLookupConfig>();
            AssetDatabase.CreateAsset(config, $"{WindowsSystem.ConfigDirectory}/{nameof(WindowsLookupConfig)}.asset");
            AssetDatabase.SaveAssetIfDirty(config);
        }

        private static List<WindowBase> GetWindowsLookup()
        {
            const string title = "Updating windows lookup";
            var lookup = new List<WindowBase>();

            EditorUtility.DisplayProgressBar(title, "Finding prefabs", 0f);
            var guids = AssetDatabase.FindAssets("t:Prefab");
            EditorUtility.DisplayProgressBar(title, "Loading prefabs", 0.1f);

            var step = 0.8f / guids.Length;
            var progress = 0.2f;

            foreach (var guid in guids)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadAssetAtPath<WindowBase>(assetPath);
                EditorUtility.DisplayProgressBar(title, $"Loading asset at: {assetPath}", progress += step);

                if (asset == null)
                {
                    continue;
                }

                lookup.Add(asset);
            }

            AssetDatabase.SaveAssets();

            EditorUtility.ClearProgressBar();
            return lookup;
        }
    }
}
#endif