using UnityEditor;
using UnityEngine;

namespace Core.Assets.Containers
{
    [CreateAssetMenu(fileName = "Material assets", menuName = "Client/Assets/Container/Materials", order = 0)]
    public class MaterialsContainer: AssetsContainer<Material>
    {
        public override Material Get<Material>(string resourceKey)
        {
            return ResourcesLookup[resourceKey] as Material;
        }
        
        public override Object LoadAssetAtPath(string path)
        {
            base.LoadAssetAtPath(path);

            return AssetDatabase.LoadAssetAtPath<Material>(path);
        }
    }
}