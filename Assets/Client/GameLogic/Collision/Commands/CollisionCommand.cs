using Core.Buckets;

namespace Client.GameLogic.Collision.Commands
{
    public interface ICollisionCommand: IBroadcast
    {
        public string FromKey { get; }
        public string ToKey { get; }
    }
}