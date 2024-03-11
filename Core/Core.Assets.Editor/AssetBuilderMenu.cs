using UnityEditor;
using UnityEngine;

namespace Core.Assets.Editor
{
    public static class AssetBuilderMenu
    {
        public const string PrefsEditor = "Editor.Assets::AutoBundleBuilder";
        private const string MenuPath = "Tools/Bundles/";
        private const string AutoBuildPath = "Auto build";
        private const string ForceBuildPath = "Force build";
        
        [MenuItem(MenuPath + AutoBuildPath)]
        private static void SwitchBundleAutoBuilder()
        {
            var lastState = PlayerPrefs.GetInt(PrefsEditor, 1);
            var newValue = 1 - lastState;
            PlayerPrefs.SetInt(PrefsEditor, newValue);
            Menu.SetChecked(MenuPath + AutoBuildPath, newValue == 1);
        }

        [MenuItem(MenuPath + ForceBuildPath)]
        private static void ForceBuild()
        {
            var lastState = PlayerPrefs.GetInt(PrefsEditor, 1);
            PlayerPrefs.SetInt(PrefsEditor, 1);
            AssetsBuilder.OnPostprocessAllAssets(null, null, null, null);
            PlayerPrefs.SetInt(PrefsEditor, lastState);
        }
    }
}