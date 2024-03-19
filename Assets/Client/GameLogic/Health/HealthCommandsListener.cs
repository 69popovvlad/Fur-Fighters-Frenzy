using System;
using System.Collections.Generic;
using Client.GameLogic.Health.Commands;
using Core.Application;

namespace Client.GameLogic.Health
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class HealthCommandsListener : IApplicationResource
    {
        private readonly HealthBucket _healthBucket;
        private readonly Dictionary<string, HealthEntity> _entities = new Dictionary<string, HealthEntity>();

        public HealthCommandsListener(HealthBucket healthBucket)
        {
            _healthBucket = healthBucket;

            healthBucket.Subscribe<DecreaseHealthCommand>(OnDecreaseHealthCommand);
            healthBucket.Subscribe<IncreaseHealthCommand>(OnIncreaseHealthCommand);
        }

        ~HealthCommandsListener()
        {
            if (_healthBucket == null)
            {
                return;
            }

            _healthBucket.Unsubscribe<DecreaseHealthCommand>(OnDecreaseHealthCommand);
            _healthBucket.Unsubscribe<IncreaseHealthCommand>(OnIncreaseHealthCommand);
        }

        public void Add(HealthEntity healthEntity)
        {
            if (_entities.ContainsKey(healthEntity.Guid))
            {
                throw new Exception($"Entity {healthEntity.Guid} already exists");
            }

            _entities[healthEntity.Guid] = healthEntity;
        }

        public void Remove(HealthEntity healthEntity)
        {
            _entities.Remove(healthEntity.Guid);
        }

        private void OnDecreaseHealthCommand(DecreaseHealthCommand command)
        {
            if (!_entities.TryGetValue(command.ToEntityKey, out var entity))
            {
                return;
            }

            if (entity.Dead)
            {
                return;
            }
            
            entity.Damage(command.Value);
        }

        private void OnIncreaseHealthCommand(IncreaseHealthCommand command)
        {
            if (!_entities.TryGetValue(command.ToEntityKey, out var entity))
            {
                return;
            }

            if (entity.Dead)
            {
                return;
            }
            
            entity.IncreaseMaxHealth(command.Value);
        }
    }
}