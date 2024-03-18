namespace Client.GameLogic.Health
{
    public interface IHealthHolder
    {
        public int MaxHealth { get; }
        
        public int Health { get; }
        
        public bool Dead { get; }
        
        public void Damage(int damage);
        
        public void IncreaseMaxHealth(int increaseValue);
    }
}