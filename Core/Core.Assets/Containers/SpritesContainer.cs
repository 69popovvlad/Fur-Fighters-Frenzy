using UnityEngine;

namespace Core.Assets.Containers
{
    [CreateAssetMenu(fileName = "Sprite assets", menuName = "Client/Assets/Container/Sprites", order = 0)]
    public class SpritesContainer: AssetsContainer<Sprite>
    {
#pragma warning disable CS0693
        public override Sprite Get<Sprite>(string resourceKey)
#pragma warning restore CS0693
        {
            return ResourcesLookup[resourceKey] as Sprite;
        }
    }
}