using FishNet.Broadcast;

namespace Client.GameLogic.Health.Commands
{
    public struct DeadHealthCommand : IHealthCommand, IBroadcast
    {
        public string FromEntityKey { get; private set; }
        public string ToEntityKey { get; private set; }

        public DeadHealthCommand(string fromEntityKey, string toEntityKey)
        {
            FromEntityKey = fromEntityKey;
            ToEntityKey = toEntityKey;
        }
    }
}