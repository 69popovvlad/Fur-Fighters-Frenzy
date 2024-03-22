namespace Client.GameLogic.Inputs.Commands.Aiming
{
    public struct AimInputCommand : IInputCommand
    {
        public string OwnerKey { get; private set; }
        public float AxisX { get; private set; }
        public float AxisY { get; private set; }

        public AimInputCommand(string ownerKey, float axisX, float axisY)
        {
            OwnerKey = ownerKey;
            AxisX = axisX;
            AxisY = axisY;
        }
    }
}