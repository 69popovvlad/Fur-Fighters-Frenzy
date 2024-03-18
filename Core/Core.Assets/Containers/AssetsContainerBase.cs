using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Core.Assets.Containers
{
    public abstract class AssetsContainerBase: ScriptableObject
    {
        public abstract Type AssetsType { get; }
        
        public abstract void Initialize(object[] assets);

        public abstract void Load();

        public abstract Object Get(string resourceKey);

        public abstract T Get<T>(string resourceKey) where T : class;

        public abstract IEnumerable<T> GetAssets<T>();

        public abstract object CreateAssetData(string resourceName, Object resource);
    }
}