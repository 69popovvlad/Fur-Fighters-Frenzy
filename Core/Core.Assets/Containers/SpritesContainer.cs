using UnityEditor;
using UnityEngine;

namespace Core.Assets.Containers
{
    [CreateAssetMenu(fileName = "Sprite assets", menuName = "Client/Assets/Container/Sprites", order = 0)]
    public class SpritesContainer: AssetsContainer<Sprite>
    {
        public override Sprite Get<Sprite>(string resourceKey)
        {
            return ResourcesLookup[resourceKey] as Sprite;
        }
        
        public override Object LoadAssetAtPath(string path)
        {
            base.LoadAssetAtPath(path);

            return AssetDatabase.LoadAssetAtPath<Sprite>(path);
        }
    }
}