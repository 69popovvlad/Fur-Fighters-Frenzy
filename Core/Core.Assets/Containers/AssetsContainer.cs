using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

namespace Core.Assets.Containers
{
    [CreateAssetMenu(fileName = "Assets", menuName = "Client/Assets/Container", order = 0)]
    public abstract class AssetsContainer<T> : AssetsContainerBase where T: Object
    {
        [FormerlySerializedAs("Asset")] [SerializeField] protected AssetData<T>[] Assets = Array.Empty<AssetData<T>>();

        protected readonly Dictionary<string, Object> ResourcesLookup = new Dictionary<string, Object>();

        public override Type AssetsType => typeof(T);

        public override void Initialize(object[] assets)
        {
            var length = assets.Length;
            Assets = new AssetData<T>[length];
            Array.Copy(assets, Assets, length);
        }

        public override void Load()
        {
            ResourcesLookup.Clear();
            
            for (int i = 0, length = Assets.Length; i < length; ++i)
            {
                var data = Assets[i];
                ResourcesLookup.Add(data.AssetPath, data.Resource);
            }
        }

        public override Object Get(string resourceKey) =>
            ResourcesLookup[resourceKey];

        public override IEnumerable<T> GetAssets<T>() =>
            Assets.Select(data => (T)(object)data.Resource);

        public override object CreateAssetData(string resourceName, Object resource) =>
            new AssetData<T>(resourceName, (T)resource);

        public override Object LoadAssetAtPath(string path)
        {
#if !UNITY_EDITOR
            throw new Exception(
                $"Method {nameof(LoadAssetAtPath)} for {nameof(ObjectsContainer)} should be used only in editor-time");
#endif
            return null;
        }
    }
}