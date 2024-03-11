using System.Collections.Generic;
using System.Linq;
using Core.Application;
using Core.Assets.Containers;
using Core.Utilities;
using Object = UnityEngine.Object;

namespace Core.Assets
{
    public class AssetsLoader: IApplicationResource
    {
        public const string OutputPath = PathExtensions.ResourcesPath + "AssetsContainers";
        public const string ConfigsPath = PathExtensions.ResourcesPath + "Configs";
        public const string DirectoriesHolderPath = "DirectoriesHolder";
        public const string ScriptableObjectFormat = ".asset";
        
        private readonly Dictionary<string, List<AssetsContainerBase>> _containers = new Dictionary<string, List<AssetsContainerBase>>();

        private bool _initialized;

        public void Initialize(bool force = false)
        {
            if (!force && _initialized)
            {
                return;
            }
            
            _containers.Clear();

            var containers = UnityEngine.Resources.LoadAll<AssetsContainerBase>(OutputPath.GetResourcePathForLoading());
            foreach (var container in containers)
            {
                container.Load();

                var assetsKey = container.name.Split(new []{'_'})[0];
                if (!_containers.TryGetValue(assetsKey, out var list))
                {
                    list = _containers[assetsKey] = new List<AssetsContainerBase>();
                }
                
                list.Add(container);
            }
            
            _initialized = true;
        }
        
        public IEnumerable<T> GetAssetsFromContainer<T>(string assetsKey) where T: Object =>
            GetContainerByType<T>(assetsKey).GetAssets<T>();

        public Object LoadResource(string assetsKey, string resourceKey) =>
            GetContainerByType<Object>(assetsKey).Get(resourceKey);
        
        public T LoadResource<T>(string assetsKey, string resourceKey) where T: Object =>
            GetContainerByType<T>(assetsKey).Get<T>(resourceKey);

        private AssetsContainerBase GetContainerByType<T>(string assetsKey) where T : Object
        {
            Initialize();
            
            var containerType = typeof(T);
            var list = _containers[assetsKey];
            
            var resultContainer = list.FirstOrDefault(container => container.AssetsType == containerType);
            if (resultContainer != null)
            {
                return resultContainer;
            }
            
            var objectType = typeof(Object);
            return list.First(container => container.AssetsType == objectType);
        }
    }
}