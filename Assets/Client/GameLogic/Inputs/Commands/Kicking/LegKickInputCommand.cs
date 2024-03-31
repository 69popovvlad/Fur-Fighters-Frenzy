namespace Client.GameLogic.Inputs.Commands.Kicking
{
    public struct LegKickInputCommand : IInputCommand
    {
        public string OwnerKey { get; private set; }

        public LegKickInputCommand(string ownerKey)
        {
            OwnerKey = ownerKey;
        }
    }
}