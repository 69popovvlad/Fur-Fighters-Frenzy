namespace Client.GameLogic.Inputs.Commands.Movement
{
    public struct MovementCommand : IInputCommand
    {
        public string OwnerKey { get; private set; }
        public float XSpeed { get; private set; }
        public float ZSpeed { get; private set; }

        public MovementCommand(string ownerKey, float xSpeed, float zSpeed)
        {
            OwnerKey = ownerKey;
            XSpeed = xSpeed;
            ZSpeed = zSpeed;
        }
    }
}