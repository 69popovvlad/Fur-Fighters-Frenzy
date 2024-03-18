namespace Client.GameLogic.Health.Commands
{
    public struct DecreaseHealthCommand : IHealthCommand
    {
        public string FromEntityKey { get; private set; }
        public string ToEntityKey { get; private set; }
        public int Value { get; private set; }

        public DecreaseHealthCommand(string fromEntityKey, string toEntityKey, int value)
        {
            FromEntityKey = fromEntityKey;
            ToEntityKey = toEntityKey;
            Value = value;
        }
    }
}