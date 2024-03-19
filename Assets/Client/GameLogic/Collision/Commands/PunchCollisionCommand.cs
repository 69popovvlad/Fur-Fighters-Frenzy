using FishNet.Broadcast;

namespace Client.GameLogic.Collision.Commands
{
    public struct PunchCollisionCommand: ICollisionCommand, IBroadcast
    {
        public string FromKey { get; set; }
        public string FromPartKey { get; set; }
        public string ToKey { get; set; }
        public string ToPartKey { get; set; }

        public PunchCollisionCommand(string fromKey, string fromPartKey, string toKey, string toPartKey)
        {
            FromKey = fromKey;
            FromPartKey = fromPartKey;
            ToKey = toKey;
            ToPartKey = toPartKey;
        }
    }
}