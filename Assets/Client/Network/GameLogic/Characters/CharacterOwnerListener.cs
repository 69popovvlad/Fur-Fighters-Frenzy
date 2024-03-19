using System.Collections.Generic;
using Client.Network.GameLogic.Characters.Commands;
using Core.Application;
using Core.Ioc;

namespace Client.Network.GameLogic.Characters
{
    public class CharacterOwnerListener: IApplicationResource
    {
        private readonly CharacterOwnerBucket _characterOwnerBucket;
        private readonly Dictionary<int, string> _ownerIdToEntityKey = new Dictionary<int, string>();
        private readonly Dictionary<string, int> _entityKeyToOwnerId = new Dictionary<string, int>();

        public CharacterOwnerListener()
        {
           _characterOwnerBucket = Ioc.Instance.Get<CharacterOwnerBucket>();
           _characterOwnerBucket.Subscribe<SetCharacterOwnerCommand>(OnSetCharacterOwnerCommand);
        }

        ~CharacterOwnerListener()
        {
            _characterOwnerBucket?.Unsubscribe<SetCharacterOwnerCommand>(OnSetCharacterOwnerCommand);
        }

        public bool TryGetEntityKey(int ownerId, out string entityKey)
        {
            return _ownerIdToEntityKey.TryGetValue(ownerId, out entityKey);
        }
        
        public bool TryGetOwnerId(string entityKey, out int ownerId)
        {
            return _entityKeyToOwnerId.TryGetValue(entityKey, out ownerId);
        }
        
        private void OnSetCharacterOwnerCommand(SetCharacterOwnerCommand command)
        {
            _ownerIdToEntityKey[command.OwnerId] = command.EntityKey;
            _entityKeyToOwnerId[command.EntityKey] = command.OwnerId;
        }
    }
}