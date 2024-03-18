using System;
using Core.Entities;
using Core.Utilities.ObservableProperty;

namespace Client.GameLogic.Health
{
    public class HealthEntity: EntityBase, IHealthHolder
    {
        public event Action<HealthEntity> OnDead;

        private readonly ObservableProperty<int> _maxHealth;
        private readonly ObservableProperty<int> _health;
        private bool _dead;

        public int MaxHealth => _maxHealth.Value;
        public IReadOnlyObservableProperty<int> MaxHealthObserver => _maxHealth;
        
        public int Health => _health.Value;
        public IReadOnlyObservableProperty<int> HealthObserver => _health;

        public bool Dead => _dead;

        public HealthEntity(int maxHealth)
        {
            _maxHealth = new ObservableProperty<int>(maxHealth);
            _health = new ObservableProperty<int>(maxHealth);
        }

        public void Damage(int damage)
        {
            if (Dead)
            {
                return;
            }
            
            _health.Value -= damage;
            if (_health.Value <= 0)
            {
                Die();
            }
        }

        public void IncreaseMaxHealth(int increaseValue)
        {
            if (Dead)
            {
                return;
            }
            
            _maxHealth.Value += increaseValue;
            _health.Value += increaseValue;
        }

        private void Die()
        {
            if (Dead)
            {
                return;
            }
            
            _dead = true;
            OnDead?.Invoke(this);
        }
    }
}