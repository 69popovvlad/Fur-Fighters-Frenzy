using Client.GameLogic.Characters;
using Client.GameLogic.Health;
using Client.GameLogic.Health.Commands;
using Client.GameLogic.Inputs;
using Client.Network.GameLogic.Characters;
using Client.Network.GameLogic.Characters.Commands;
using Core.Assets;
using Core.Entities.Views;
using Core.Ioc;
using FishNet.Object;
using UnityEngine;

namespace Client.Network.GameLogic
{
    public class CharacterSpawner : NetworkBehaviour
    {
        [SerializeField] private PlayerInputHandler _inputPrefab;
        
        private CharacterOwnerBucket _characterOwnerBucket;
        private AssetsLoader _assetsLoader;
        private CharacterView _characterView;
        private HealthBucket _healthBucket;

        private void Awake()
        {
            var ioc = Ioc.Instance;
            _assetsLoader = ioc.Get<AssetsLoader>();
            _characterOwnerBucket = ioc.Get<CharacterOwnerBucket>();
            _healthBucket = ioc.Get<HealthBucket>();
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            
            var input = Instantiate(_inputPrefab);
            
            _characterOwnerBucket.Subscribe<SetCharacterOwnerCommand>(OnSetCharacterOwnerCommand);
            _healthBucket.Subscribe<DeadHealthCommand>(OnDeadHealthCommand);
            
            SpawnCharacter();
        }

        public override void OnStopClient()
        {
            base.OnStopClient();

            UnSubscribe();
        }

        private void UnSubscribe()
        {
            _characterOwnerBucket?.Unsubscribe<SetCharacterOwnerCommand>(OnSetCharacterOwnerCommand);
            _healthBucket?.Unsubscribe<DeadHealthCommand>(OnDeadHealthCommand);
        }

        private void OnSetCharacterOwnerCommand(SetCharacterOwnerCommand command)
        {
            if (OwnerId != command.OwnerId)
            {
                return;
            }

            _characterView = ViewsContainer.GetView<CharacterView>(command.EntityKey);
        }

        private void OnDeadHealthCommand(DeadHealthCommand command)
        {
            if (!_characterView || !_characterView.Guid.Equals(command.ToEntityKey))
            {
                return;
            }

            DespawnCharacter();
            SpawnCharacter();
        }
        
        [ServerRpc]
        private void SpawnCharacter()
        {
            var prefab = _assetsLoader.LoadResource<GameObject>("character", "cat_000"); // TODO: Load key or character data
            var instance = Instantiate(prefab, transform.position, Quaternion.identity);
            ServerManager.Spawn(instance, Owner);
        }

        private void DespawnCharacter()
        {
            DespawnCharacterOnServer(_characterView.gameObject);
            Destroy(_characterView.gameObject);
            _characterView = null;
        }

        [ServerRpc(RequireOwnership = true)]
        private void DespawnCharacterOnServer(GameObject character)
        {
            ServerManager.Despawn(character);
        }
    }
}