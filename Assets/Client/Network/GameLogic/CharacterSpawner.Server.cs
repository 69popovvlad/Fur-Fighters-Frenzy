using Client.GameLogic.Characters;
using Client.GameLogic.Health.Commands;
using UnityEngine;

namespace Client.Network.GameLogic
{
    public partial class CharacterSpawner
    {
        private CharacterView _characterViewServerSide;
        
        private void OnServerSideInitialized()
        {
            _healthBucket.Subscribe<DeadHealthCommand>(OnDeadHealthCommandServer);

            SpawnCharacter();
        }
        
        private void OnServerSideDeinitialized()
        {
            _healthBucket?.Unsubscribe<DeadHealthCommand>(OnDeadHealthCommandServer);
        }
        
        private void OnDeadHealthCommandServer(DeadHealthCommand command)
        {
            if (!_characterViewServerSide || !_characterViewServerSide.Guid.Equals(command.ToEntityKey))
            {
                return;
            }

            Invoke(nameof(RespawnCharacter), 3);
        }

        private void RespawnCharacter()
        {
            DespawnCharacter();
            SpawnCharacter();
        }

        private void SpawnCharacter()
        {
            var prefab =
                _assetsLoader.LoadResource<GameObject>("character", "cat_000"); // TODO: Load key or character data
            var instance = Instantiate(prefab, transform.position, Quaternion.identity);
            ServerManager.Spawn(instance, Owner);

            _characterViewServerSide = instance.GetComponent<CharacterView>();
        }

        private void DespawnCharacter()
        {
            if (_characterViewServerSide == null)
            {
                return;
            }
            
            ServerManager.Despawn(_characterViewServerSide.gameObject);
            _characterViewServerSide = null;
        }
    }
}