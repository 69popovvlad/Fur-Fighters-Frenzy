using FishNet.Broadcast;

namespace Client.Network.GameLogic.Throwing.Commands
{
    public struct ThrowingDamageCommand: IBroadcast
    {
        public string FromKey { get; set; }
        public string ToKey { get; set; }
        public int Damage { get; set; }

        public ThrowingDamageCommand(string fromKey, string toKey, int damage)
        {
            FromKey = fromKey;
            ToKey = toKey;
            Damage = damage;
        }
    }
}