using Core.Buckets;

namespace Client.GameLogic.Health.Commands
{
    public interface IHealthCommand: IBroadcast
    {
        public string FromEntityKey { get; }
        public string ToEntityKey { get; }
    }
}