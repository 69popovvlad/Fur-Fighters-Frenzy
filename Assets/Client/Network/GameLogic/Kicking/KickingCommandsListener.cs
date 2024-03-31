using Client.GameLogic.Characters;
using Client.GameLogic.Collision;
using Client.GameLogic.Collision.Commands;
using Core.Entities.Views;
using Core.Ioc;
using FishNet.Object;

namespace Client.Network.GameLogic.Kicking
{
    public class KickingCommandsListener : NetworkBehaviour
    {
        private CollisionBucket _collisionBucket;

        public override void OnStartServer()
        {
            base.OnStartServer();

            var ioc = Ioc.Instance;
            _collisionBucket = ioc.Get<CollisionBucket>();
            _collisionBucket.Subscribe<KickCollisionCommand>(OnKickCollisionCommand);
        }

        public override void OnStopServer()
        {
            base.OnStopServer();

            _collisionBucket?.Unsubscribe<KickCollisionCommand>(OnKickCollisionCommand);
        }

        private void OnKickCollisionCommand(KickCollisionCommand command)
        {
            var characterViewTo = ViewsContainer.GetView<CharacterView>(command.ToKey);
            if (characterViewTo.Health.Dead)
            {
                return;
            }

            var characterViewFrom = ViewsContainer.GetView<CharacterView>(command.FromKey);
            if (characterViewFrom.Health.Dead)
            {
                return;
            }

            var calculatedPower = 1f;
            // TODO: calculate power here
            // var partEntity = EntitiesContainer.GetEntity(command.FromPartKey);
            // calculatedPower += partEntity.getPartPowerBonus();

            AddRigidbodyImpulseServerLocal(characterViewFrom, characterViewTo, calculatedPower);
        }

        [ObserversRpc(RunLocally = true)]
        private void AddRigidbodyImpulseServerLocal(CharacterView characterViewFrom, CharacterView characterViewTo, float power)
        {
            var direction = characterViewTo.transform.position - characterViewFrom.transform.position;
            characterViewTo.MovementControl.AddKickImpulse(direction, power);
        }
    }
}