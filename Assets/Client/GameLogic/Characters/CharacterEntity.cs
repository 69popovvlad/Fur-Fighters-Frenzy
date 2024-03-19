using Client.GameLogic.Health;
using Core.Entities;

namespace Client.GameLogic.Characters
{
    public class CharacterEntity : EntityBase
    {
        public HealthEntity Health { get; private set; }

        public CharacterEntity(int maxHealth, string guidBase) : base(guidBase + $"_{nameof(CharacterEntity)}")
        {
            Health = new HealthEntity(maxHealth, guidBase);
        }
    }
}