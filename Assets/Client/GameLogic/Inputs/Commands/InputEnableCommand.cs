namespace Client.GameLogic.Inputs.Commands
{
    public struct InputEnableCommand : IInputCommand
    {
        public string OwnerKey { get; private set; }
        public bool Enable { get; private set; }

        public InputEnableCommand(string ownerKey, bool enable)
        {
            OwnerKey = ownerKey;
            Enable = enable;
        }
    }
}