using Client.GameLogic.Characters;
using Client.GameLogic.Collision;
using Client.GameLogic.Collision.Commands;
using Client.Network.GameLogic.Punching.Commands;
using Core.Entities.Views;
using Core.Ioc;
using FishNet.Object;

namespace Client.Network.GameLogic.Punching
{
    public class PunchingCommandsListener : NetworkBehaviour
    {
        private CollisionBucket _collisionBucket;

        public override void OnStartServer()
        {
            base.OnStartServer();
            
            _collisionBucket = Ioc.Instance.Get<CollisionBucket>();
            _collisionBucket.Subscribe<PunchCollisionCommand>(OnPunchCollisionCommand);
        }

        public override void OnStopServer()
        {
            base.OnStopServer();
            
            _collisionBucket?.Unsubscribe<PunchCollisionCommand>(OnPunchCollisionCommand);
        }

        private void OnPunchCollisionCommand(PunchCollisionCommand command)
        {
            var characterView = ViewsContainer.GetView<CharacterView>(command.ToKey);
            if (characterView.Health.Dead)
            {
                return;
            }
            
            var damage = 1;
            // TODO: calculate damage here and send damage command
            // var partEntity = EntitiesContainer.GetEntity(command.FromPartKey);
            // damage += partEntity.getPartDamageBonus();

            var damageCommand = new PunchDamageCommand(command.FromKey, command.ToKey, damage);
            ServerManager.Broadcast(damageCommand);
            
            characterView.Health.Damage(command.FromKey, damage);
        }
    }
}