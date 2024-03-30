using System;
using Client.GameLogic.Characters;
using Client.GameLogic.Health.Commands;
using Core.Ioc;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Client.GameLogic.Health
{
    public class HealthStatusControl : MonoBehaviour
    {
        private readonly int DeadHash = Animator.StringToHash("Dead");

        [SerializeField] private Animator _animator;
        [SerializeField] private Rig _rig;
        [SerializeField] private CharacterView _characterView;
        [SerializeField] private GameObject _deadBloodParticle;
        [SerializeField] private GameObject _bleedingParticle;
        [SerializeField, Range(0f, 1f)] private float _bleedingEnablingPercent;

        private HealthBucket _healthBucket;

        private void Awake()
        {
            _healthBucket = Ioc.Instance.Get<HealthBucket>();
            _healthBucket.Subscribe<DecreaseHealthCommand>(OnDecreaseHealthCommand);
            _healthBucket.Subscribe<IncreaseHealthCommand>(OnIncreaseHealthCommand);
            _healthBucket.Subscribe<DeadHealthCommand>(OnDeadHealthCommand);
        }

        private void OnDestroy()
        {
            if (_healthBucket == null)
            {
                return;
            }

            _healthBucket.Unsubscribe<DecreaseHealthCommand>(OnDecreaseHealthCommand);
            _healthBucket.Unsubscribe<IncreaseHealthCommand>(OnIncreaseHealthCommand);
            _healthBucket.Unsubscribe<DeadHealthCommand>(OnDeadHealthCommand);
        }

        private void OnIncreaseHealthCommand(IncreaseHealthCommand command)
        {
            if (!IsMineHealth(command.ToEntityKey))
            {
                return;
            }

            UpdateBleedingView();
        }

        private void OnDecreaseHealthCommand(DecreaseHealthCommand command)
        {
            if (!IsMineHealth(command.ToEntityKey))
            {
                return;
            }

            UpdateBleedingView();
        }

        private void OnDeadHealthCommand(DeadHealthCommand command)
        {
            if (!IsMineCharacter(command.ToEntityKey))
            {
                return;
            }

            _animator.SetTrigger(DeadHash);
            _rig.weight = 0;

            if (_deadBloodParticle == null)
            {
                return;
            }

            _deadBloodParticle.SetActive(true);
        }

        private bool IsMineCharacter(string entityKey)
        {
            return entityKey.Equals(_characterView.Guid);
        }

        private bool IsMineHealth(string entityKey)
        {
            return entityKey.Equals(_characterView.Health.Key);
        }

        private void UpdateBleedingView()
        {
            if (_bleedingParticle == null)
            {
                return;
            }

            var healthControl = _characterView.Health;
            _bleedingParticle.SetActive(healthControl.Health / (float)healthControl.MaxHealth <= _bleedingEnablingPercent);
        }
    }
}