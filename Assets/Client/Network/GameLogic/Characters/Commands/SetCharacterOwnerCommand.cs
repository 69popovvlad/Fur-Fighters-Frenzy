using FishNet.Broadcast;

namespace Client.Network.GameLogic.Characters.Commands
{
    public struct SetCharacterOwnerCommand: IOwnerCommand, IBroadcast
    {
        public string EntityKey { get; private set; }
        public int OwnerId { get; private set; }

        public SetCharacterOwnerCommand(string entityKey, int ownerId)
        {
            EntityKey = entityKey;
            OwnerId = ownerId;
        }
    }
}