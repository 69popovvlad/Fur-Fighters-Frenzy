using System;
using Core.Assets.Containers;
using UnityEngine;

namespace Core.Assets.Editor
{
    public static partial class AssetsBuilder
    {
        public static AssetsContainerBase CreateContainer(AssetsType assetsType)
        {
            return assetsType switch
            {
                AssetsType.Object => ScriptableObject.CreateInstance<ObjectsContainer>(),
                AssetsType.Sprite => ScriptableObject.CreateInstance<SpritesContainer>(),
                AssetsType.Material => ScriptableObject.CreateInstance<MaterialsContainer>(),
                _ => throw new ArgumentOutOfRangeException(nameof(assetsType), assetsType, null)
            };
        }
    }
}