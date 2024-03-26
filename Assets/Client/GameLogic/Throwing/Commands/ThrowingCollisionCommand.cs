using Client.GameLogic.Collision.Commands;

namespace Client.GameLogic.Throwing.Commands
{
    public struct ThrowingCollisionCommand: ICollisionCommand
    {
        public string FromKey { get; private set; }
        public string ToKey { get; private set; }
        public int ItemDamage { get; private set; }
        public string ToPartKey { get; private set; }

        public ThrowingCollisionCommand(string fromKey, string toKey, int itemDamage, string toPartKey)
        {
            FromKey = fromKey;
            ToKey = toKey;
            ItemDamage = itemDamage;
            ToPartKey = toPartKey;
        }
    }
}