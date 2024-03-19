using UnityEngine;
using UnityEngine.UI;

namespace Client.GameLogic.Health
{
    public class HealthView : MonoBehaviour
    {
        [SerializeField] private Slider _healthBar;

        private HealthControl _healthControl;
        private int _maxHealth;
        private int _currentHealth;

        public void Initialize(HealthControl healthControl)
        {
            _healthControl = healthControl;
            
            _maxHealth = _healthControl.MaxHealth;
            _currentHealth = _healthControl.Health;
            UpdateView();

            _healthControl.HealthObserver.PropertyChanged += OnHealthChanged;
            _healthControl.MaxHealthObserver.PropertyChanged += OnMaxHealthChanged;
        }

        private void OnDestroy()
        {
            if (_healthControl == null)
            {
                return;
            }
            
            _healthControl.HealthObserver.PropertyChanged -= OnHealthChanged;
            _healthControl.MaxHealthObserver.PropertyChanged -= OnMaxHealthChanged;
        }

        private void OnHealthChanged(string _, object newValue, object lastValue)
        {
            _currentHealth = (int)newValue;
            UpdateView();
        }

        private void OnMaxHealthChanged(string _, object newValue, object lastValue)
        {
            _maxHealth = (int)newValue;
            UpdateView();
        }

        private void UpdateView()
        {
            _healthBar.value = (float)_currentHealth / _maxHealth;
        }
    }
}