namespace Client.GameLogic.Inputs.Commands.ChokeHold
{
    public struct ChokeHoldInputCommand  : IInputCommand
    {
        public string OwnerKey { get; private set; }

        public ChokeHoldInputCommand(string ownerKey)
        {
            OwnerKey = ownerKey;
        }
    }
}
