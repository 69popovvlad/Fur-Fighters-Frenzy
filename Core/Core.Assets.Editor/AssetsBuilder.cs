using System.Collections.Generic;
using System.IO;
using System.Linq;
using Core.Assets.Containers;
using Core.Processing.Editor;
using Core.Utilities;
using Core.Utilities.Editor;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace Core.Assets.Editor
{
    [Postprocessor(1)]
    public static partial class AssetsBuilder
    {
        private const string OutputPath = AssetsLoader.OutputPath;
        private const string ConfigsPath = AssetsLoader.ConfigsPath;
        private const string DirectoriesHolderPath = AssetsLoader.DirectoriesHolderPath;
        private const string ScriptableObjectFormat = AssetsLoader.ScriptableObjectFormat;

        [PostprocessorStep(0), UsedImplicitly]
        public static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets,
            string[] movedAssets, string[] movedFromAssetPaths)
        {
            if (PlayerPrefs.GetInt(AssetBuilderMenu.PrefsEditor, 1) == 0)
            {
                return;
            }

            var directoriesHolder = default(AssetsDirectoriesHolder);
            var assetFullPath = Path.Combine(ConfigsPath, DirectoriesHolderPath) + ScriptableObjectFormat;
            if (!File.Exists(assetFullPath))
            {
                Debug.log($"Try to craete a new {nameof(AssetsDirectoriesHolder)}");
                CreateDirectoryIfNeed(ConfigsPath);

                Debug.log($"Try to craete 0");
                directoriesHolder = ScriptableObject.CreateInstance<AssetsDirectoriesHolder>();
                Debug.log($"Try to craete 1");
                AssetDatabase.CreateAsset(directoriesHolder, assetFullPath);
                Debug.log($"Try to craete 2");
                AssetDatabase.SaveAssetIfDirty(directoriesHolder);
                Debug.Log($"{nameof(AssetsDirectoriesHolder)} was created. Please, fill this file");
            }
            else
            {
                Debug.log($"Try to load 0");
                directoriesHolder = Resources.Load<AssetsDirectoriesHolder>(assetFullPath.GetResourcePathForLoading());
                Debug.Log(
                    $"{directoriesHolder.name} was loaded with {directoriesHolder.AllDirectories.Length} directories");
            }

            ClearOutputPath();

            for (int i = 0, iLength = directoriesHolder.AllDirectories.Length; i < iLength; ++i)
            {
                CreateAssetsContainer(directoriesHolder.AllDirectories[i]);
            }
        }

        private static void ClearOutputPath()
        {
            ToolProfile.DeleteDirectory(OutputPath);
            CreateDirectoryIfNeed(OutputPath);
        }

        private static void CreateDirectoryIfNeed(string directory)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        private static void CreateAssetsContainer(AssetsDirectoriesData data)
        {
            var (container, editorLoadAssetDelegate) = CreateContainer(data.AssetsType);

            var assets = new List<object>();
            var resourceTypeKey = data.AssetsKey.ToString();
            for (int j = 0, jLength = data.Directories.Length; j < jLength; ++j)
            {
                var newAssets = CollectAssets(container, data.Directories[j], editorLoadAssetDelegate);
                assets.AddRange(newAssets);
            }

            container.Initialize(assets.ToArray());

            var containersAssetPath = Path.Combine(OutputPath, resourceTypeKey) + "_" + data.AssetsType +
                                      ScriptableObjectFormat;
            AssetDatabase.CreateAsset(container, containersAssetPath);
            AssetDatabase.SaveAssetIfDirty(container);
        }

        private static IEnumerable<object> CollectAssets(AssetsContainerBase container, string directory,
            LoadAssetEditorDelegate @delegate)
        {
            return AssetDatabase.FindAssets(string.Empty, new[] { directory })
                .Select(guid => @delegate(AssetDatabase.GUIDToAssetPath(guid)))
                .Select(resource => container.CreateAssetData(resource.name, resource))
                .DefaultIfEmpty();
        }
    }
}