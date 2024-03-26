namespace Client.GameLogic.Inputs.Commands.Taking
{
    public struct TakingInputCommand: IInputCommand
    {
        public string OwnerKey { get; private set; }

        public TakingInputCommand(string ownerKey)
        {
            OwnerKey = ownerKey;
        }
    }
}