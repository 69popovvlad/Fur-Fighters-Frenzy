using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Core.Assets.Containers
{
    [CreateAssetMenu(fileName = "Object assets", menuName = "Client/Assets/Container/Objects", order = 0)]
    public class ObjectsContainer: AssetsContainer<Object>
    {
        public override T Get<T>(string resourceKey)
        {
            return ResourcesLookup[resourceKey] as T;
        }

        public override Object LoadAssetAtPath(string path)
        {
            base.LoadAssetAtPath(path);

            return AssetDatabase.LoadAssetAtPath<Object>(path);
        }
    }
}