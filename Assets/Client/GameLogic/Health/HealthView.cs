using UnityEngine;
using UnityEngine.UI;

namespace Client.GameLogic.Health
{
    [DefaultExecutionOrder(5000)] // Should start after CharacterView
    public class HealthView : MonoBehaviour
    {
        [SerializeField] private HealthControl _healthControl;
        [SerializeField] private Slider _healthBar;

        private int _maxHealth;
        private int _currentHealth;

        private void Awake()
        {
            _maxHealth = _healthControl.MaxHealth;
            _currentHealth = _healthControl.Health;
            UpdateView();

            _healthControl.HealthObserver.PropertyChanged += OnHealthChanged;
            _healthControl.MaxHealthObserver.PropertyChanged += OnMaxHealthChanged;
        }

        private void OnDestroy()
        {
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