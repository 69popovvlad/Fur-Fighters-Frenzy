using Core.Buckets;

namespace Client.Network.GameLogic.Characters.Commands
{
    public interface IOwnerCommand: IBroadcast
    {
        public string EntityKey { get; }
        public int OwnerId { get; }
    }
}