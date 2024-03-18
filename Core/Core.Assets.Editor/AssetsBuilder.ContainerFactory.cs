using System;
using Core.Assets.Containers;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Core.Assets.Editor
{
    public static partial class AssetsBuilder
    {
        public delegate Object LoadAssetEditorDelegate(string path);
        
        public static (AssetsContainerBase container, LoadAssetEditorDelegate editorLoadAssetDelegate) CreateContainer(AssetsType assetsType)
        {
            return assetsType switch
            {
                AssetsType.Object => (ScriptableObject.CreateInstance<ObjectsContainer>(), AssetDatabase.LoadAssetAtPath<Object>),
                AssetsType.Sprite => (ScriptableObject.CreateInstance<SpritesContainer>(), AssetDatabase.LoadAssetAtPath<Sprite>),
                AssetsType.Material => (ScriptableObject.CreateInstance<MaterialsContainer>(), AssetDatabase.LoadAssetAtPath<Material>),
                _ => throw new ArgumentOutOfRangeException(nameof(assetsType), assetsType, null)
            };
        }
    }
}