using UnityEngine;

namespace Core.Assets.Containers
{
    [CreateAssetMenu(fileName = "Material assets", menuName = "Client/Assets/Container/Materials", order = 0)]
    public class MaterialsContainer: AssetsContainer<Material>
    {
#pragma warning disable CS0693
        public override Material Get<Material>(string resourceKey)
#pragma warning restore CS0693
        {
            return ResourcesLookup[resourceKey] as Material;
        }
    }
}