using Client.GameLogic.Aiming;
using Client.GameLogic.Health;
using Client.GameLogic.Health.Commands;
using Client.GameLogic.Movement;
using Client.Network.Entities;
using Client.Network.GameLogic.Characters;
using Client.Network.GameLogic.Characters.Commands;
using Client.Network.GameLogic.Punching.Commands;
using Client.Network.GameLogic.Throwing.Commands;
using Core.Ioc;
using FishNet.Transporting;
using UnityEngine;

namespace Client.GameLogic.Characters
{
    public class CharacterView : NetworkEntityView
    {
        [SerializeField] private HealthControl _health;
        [SerializeField] private AimingControl _aimingControl;
        [SerializeField] private MovementControl _movementControl;

        [SerializeField, Tooltip("Start and max health value at the same time")]
        private int _maxHealth = 10;

        [SerializeField] private Transform _cameraTarget;

        private CharacterEntity _entity;

        public HealthControl Health => _health;

        public AimingControl AimingControl => _aimingControl;

        public MovementControl MovementControl => _movementControl;

        public Transform CameraTarget => _cameraTarget;

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

            if (!IsServerInitialized)
            {
                ClientManager.RegisterBroadcast<PunchDamageCommand>(OnPunchDamageCommand);
                ClientManager.RegisterBroadcast<ThrowingDamageCommand>(OnThrowingDamageCommand);
            }

            if (!IsOwner)
            {
                return;
            }

            var characterOwnerBucket = Ioc.Instance.Get<CharacterOwnerBucket>();
            var command = new SetCharacterOwnerCommand(Guid, OwnerId);
            characterOwnerBucket.Invoke(command);
        }

        public override void OnStopClient()
        {
            base.OnStopClient();

            if (!IsServerInitialized)
            {
                ClientManager.UnregisterBroadcast<PunchDamageCommand>(OnPunchDamageCommand);
                ClientManager.UnregisterBroadcast<ThrowingDamageCommand>(OnThrowingDamageCommand);
            }
        }

        private void OnPunchDamageCommand(PunchDamageCommand command, Channel channel) =>
            OnDamageCommand(command.FromKey, command.ToKey, command.Damage, channel);

        private void OnThrowingDamageCommand(ThrowingDamageCommand command, Channel channel) =>
            OnDamageCommand(command.FromKey, command.ToKey, command.Damage, channel);

        private void OnDamageCommand(string from, string to, int damage, Channel channel)
        {
            if (!Guid.Equals(to) || Health.Dead)
            {
                return;
            }

            // TODO: Code block below for kills counter only
            Health.Damage(from, damage);

            if (!Health.Dead)
            {
                return;
            }

            // Sending to respawn our character
            var deadCommand = new DeadHealthCommand(from, to);
            var healthBucket = Ioc.Instance.Get<HealthBucket>();
            healthBucket.Invoke(deadCommand);
        }
    }
}