namespace Client.GameLogic.Inputs.Commands.Movement
{
    public struct DodgeInputCommand : IInputCommand
    {
        public string OwnerKey { get; private set; }
        public float XSpeed { get; private set; }
        public float ZSpeed { get; private set; }

        public DodgeInputCommand(string ownerKey, float xSpeed, float zSpeed)
        {
            OwnerKey = ownerKey;
            XSpeed = xSpeed;
            ZSpeed = zSpeed;
        }
    }
}