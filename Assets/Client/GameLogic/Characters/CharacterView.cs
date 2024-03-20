using Client.GameLogic.Health;
using Client.GameLogic.Health.Commands;
using Client.Network.Entities;
using Client.Network.GameLogic.Characters;
using Client.Network.GameLogic.Characters.Commands;
using Client.Network.GameLogic.Punching.Commands;
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

        public override void OnStartNetwork()
        {
            base.OnStartNetwork();

            _entity = new CharacterEntity(_maxHealth, ObjectId.ToString());
            Initialize(_entity);

            _health.Initialize(_entity.Health);
        }

        public override void OnStartClient()
        {
            base.OnStartClient();

            var characterOwnerBucket = Ioc.Instance.Get<CharacterOwnerBucket>();
            var command = new SetCharacterOwnerCommand(Guid, OwnerId);
            characterOwnerBucket.Invoke(command);
        }

        protected override void InitializeInternal()
        {
            base.InitializeInternal();

            if (IsClientInitialized)
            {
                ClientManager.RegisterBroadcast<PunchDamageCommand>(OnPunchDamageCommand);
            }
        }

        protected override void DeinitializationInternal()
        {
            base.DeinitializationInternal();

            if (IsClientInitialized)
            {
                ClientManager.UnregisterBroadcast<PunchDamageCommand>(OnPunchDamageCommand);
            }
        }

        private void OnPunchDamageCommand(PunchDamageCommand command, Channel channel)
        {
            if (!Guid.Equals(command.ToKey) || Health.Dead)
            {
                return;
            }

            Health.Damage(command.FromKey, command.Damage);

            if (!Health.Dead)
            {
                return;
            }

            // Sending to respawn our character
            var deadCommand = new DeadHealthCommand(command.FromKey, command.ToKey);
            var healthBucket = Ioc.Instance.Get<HealthBucket>();
            healthBucket.Invoke(deadCommand);
        }
    }
}