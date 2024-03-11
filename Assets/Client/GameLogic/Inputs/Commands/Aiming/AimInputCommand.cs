namespace Client.GameLogic.Inputs.Commands.Aiming
{
    public struct AimInputCommand : IInputCommand
    {
        public string OwnerKey { get; private set; }
        public float XPosition { get; private set; }
        public float YPosition { get; private set; }
        public float ZPosition { get; private set; }

        public AimInputCommand(string ownerKey, float xPosition, float yPosition, float zPosition)
        {
            OwnerKey = ownerKey;
            XPosition = xPosition;
            YPosition = yPosition;
            ZPosition = zPosition;
        }
    }
}