namespace Client.GameLogic.Inputs.Commands.Punching
{
    public struct PunchInputCommand : IInputCommand
    {
        public string OwnerKey { get; private set; }
        public byte ButtonState { get; private set; }
        public bool IsLeftHand { get; private set; }

        public PunchInputCommand(string ownerKey, bool isLeftHand, byte buttonState)
        {
            OwnerKey = ownerKey;
            IsLeftHand = isLeftHand;
            ButtonState = buttonState;
        }
    }
}