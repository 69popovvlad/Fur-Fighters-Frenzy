using Client.GameLogic.Inputs;
using Core.Assets;
using Core.Ioc;
using FishNet.Object;
using UnityEngine;

namespace Client.Network.GameLogic
{
    public class CharacterSpawner : NetworkBehaviour
    {
        [SerializeField] private PlayerInputHandler _inputPrefab;

        public override void OnStartClient()
        {
            var input = Instantiate(_inputPrefab);

            var assetsLoader = Ioc.Instance.Get<AssetsLoader>();
            var prefab = assetsLoader.LoadResource<GameObject>("character", "cat_000"); // TODO: Load key or character data
            var instance = Instantiate(prefab);
            ServerManager.Spawn(instance, Owner);
        }
    }
}