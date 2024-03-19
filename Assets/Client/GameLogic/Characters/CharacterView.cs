using Client.GameLogic.Collision.Commands;
using Client.GameLogic.Health;
using Client.GameLogic.Health.Commands;
using Client.Network.Entities;
using Client.Network.GameLogic.Characters;
using Client.Network.GameLogic.Characters.Commands;
using Core.Ioc;
using FishNet.Transporting;
using UnityEngine;

namespace Client.GameLogic.Characters
{
    public class CharacterView : NetworkEntityView
    {
        [SerializeField] private HealthControl _health;
        [SerializeField, Tooltip("Start and max health value at the same time")]
        private int _maxHealth = 10;

        private CharacterEntity _entity;

        public HealthControl Health => _health;

        public override void OnStartClient()
        {
            _entity = new CharacterEntity(_maxHealth, ObjectId.ToString());
            Initialize(_entity);

            _health.Initialize(_entity.Health);
            
            var characterOwnerBucket = Ioc.Instance.Get<CharacterOwnerBucket>();
            var command = new SetCharacterOwnerCommand(Guid, OwnerId);
            characterOwnerBucket.Invoke(command);
        }

        protected override void InitializeInternal()
        {
            base.InitializeInternal();
            
            ClientManager.RegisterBroadcast<PunchCollisionCommand>(OnPunchCollisionCommand);
        }

        protected override void DeinitializationInternal()
        {
            base.DeinitializationInternal();
            
            ClientManager.UnregisterBroadcast<PunchCollisionCommand>(OnPunchCollisionCommand);
        }

        private void OnPunchCollisionCommand(PunchCollisionCommand command, Channel channel)
        {
            if (!Guid.Equals(command.ToKey) || Health.Dead)
            {
                return;
            }
            
            var damage = 1;
            // TODO: calculate damage here and send damage command
            // var partEntity = EntitiesContainer.GetEntity(command.FromPartKey);
            // damage += partEntity.getPartDamageBonus();
            
            Health.Damage(command.FromKey, damage);

            if (!Health.Dead)
            {
                return;
            }
            
            var deadCommand = new DeadHealthCommand(command.FromKey, command.ToKey);
            var healthBucket = Ioc.Instance.Get<HealthBucket>();
            healthBucket.Invoke(deadCommand);
        }
    }
}