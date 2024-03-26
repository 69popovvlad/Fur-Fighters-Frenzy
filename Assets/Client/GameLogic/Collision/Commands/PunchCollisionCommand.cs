namespace Client.GameLogic.Collision.Commands
{
    public struct PunchCollisionCommand: ICollisionCommand
    {
        public string FromKey { get; private set; }
        public string FromPartKey { get; private set; }
        public string ToKey { get; private set; }
        public string ToPartKey { get; private set; }

        public PunchCollisionCommand(string fromKey, string fromPartKey, string toKey, string toPartKey)
        {
            FromKey = fromKey;
            FromPartKey = fromPartKey;
            ToKey = toKey;
            ToPartKey = toPartKey;
        }
    }
}