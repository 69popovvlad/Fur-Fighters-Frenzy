using Client.GameLogic.Health.Commands;
using Core.Ioc;
using Core.Utilities.ObservableProperty;
using UnityEngine;

namespace Client.GameLogic.Health
{
    public class HealthControl : MonoBehaviour
    {
        private HealthEntity _healthEntity;
        private HealthBucket _healthBucket;
        private HealthCommandsListener _healthCommandsListener;

        public string Key => _healthEntity.Guid;

        public int MaxHealth => _healthEntity.MaxHealth;
        public int Health => _healthEntity.Health;
        public bool Dead => _healthEntity.Dead;
        
        public IReadOnlyObservableProperty<int> MaxHealthObserver => _healthEntity.MaxHealthObserver;
        public IReadOnlyObservableProperty<int> HealthObserver => _healthEntity.HealthObserver;

        public void Initialize(int maxHealth)
        {
            _healthEntity = new HealthEntity(maxHealth);

            _healthBucket = Ioc.Instance.Get<HealthBucket>();

            _healthCommandsListener = Ioc.Instance.Get<HealthCommandsListener>();
            _healthCommandsListener.Add(_healthEntity);
        }

        private void OnDestroy()
        {
            _healthCommandsListener?.Remove(_healthEntity);
            _healthEntity?.Dispose();
        }

        public void Damage(string fromKey, int damage)
        {
            var command = new DecreaseHealthCommand(fromKey, Key, damage);
            _healthBucket.Invoke(command);
        }

        public void IncreaseMaxHealth(string fromKey, int increaseValue)
        {
            var command = new IncreaseHealthCommand(fromKey, Key, increaseValue);
            _healthBucket.Invoke(command);
        }
    }
}