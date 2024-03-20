using FishNet.Broadcast;

namespace Client.Network.GameLogic.Punching.Commands
{
    public struct PunchDamageCommand: IBroadcast
    {
        public string FromKey { get; set; }
        public string ToKey { get; set; }
        public int Damage { get; set; }

        public PunchDamageCommand(string fromKey, string toKey, int damage)
        {
            FromKey = fromKey;
            ToKey = toKey;
            Damage = damage;
        }
    }
}