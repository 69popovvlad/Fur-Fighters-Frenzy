using System;

namespace Core.Assets
{
    [Serializable]
    public struct AssetData<T>
    {
        public string AssetPath;
        public T Resource;

        public AssetData(string assetPath, T resource)
        {
            AssetPath = assetPath;
            Resource = resource;
        }
    }
}