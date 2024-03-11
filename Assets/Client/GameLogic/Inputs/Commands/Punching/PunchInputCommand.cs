namespace Client.GameLogic.Inputs.Commands.Punching
{
    public struct PunchInputCommand: IInputCommand
    {
        public string OwnerKey { get; private set; }
        public bool isLeftHand { get; private set; }

        public PunchInputCommand(string ownerKey, bool isLeftHand)
        {
            OwnerKey = ownerKey;
            this.isLeftHand = isLeftHand;
        }
    }
}