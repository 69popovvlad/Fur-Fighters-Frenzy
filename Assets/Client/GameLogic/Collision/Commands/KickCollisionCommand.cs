namespace Client.GameLogic.Collision.Commands
{
    public struct KickCollisionCommand : ICollisionCommand
    {
        public string FromKey { get; private set; }
        public string FromPartKey { get; private set; }
        public string ToKey { get; private set; }

        public KickCollisionCommand(string fromKey, string fromPartKey, string toKey)
        {
            FromKey = fromKey;
            FromPartKey = fromPartKey;
            ToKey = toKey;
        }
    }
}