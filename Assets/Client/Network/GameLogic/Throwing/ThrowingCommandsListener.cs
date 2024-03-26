using Client.GameLogic.Characters;
using Client.GameLogic.Collision;
using Client.GameLogic.Health;
using Client.GameLogic.Health.Commands;
using Client.GameLogic.Throwing.Commands;
using Client.Network.GameLogic.Throwing.Commands;
using Core.Entities.Views;
using Core.Ioc;
using FishNet.Object;

namespace Client.Network.GameLogic.Throwing
{
    public class ThrowingCommandsListener: NetworkBehaviour
    {
        private CollisionBucket _collisionBucket;
        private HealthBucket _healthBucket;

        public override void OnStartServer()
        {
            base.OnStartServer();
            
            var ioc = Ioc.Instance;
            _healthBucket = ioc.Get<HealthBucket>();
            
            _collisionBucket = ioc.Get<CollisionBucket>();
            _collisionBucket.Subscribe<ThrowingCollisionCommand>(OnThrowingCollisionCommand);
        }

        public override void OnStopServer()
        {
            base.OnStopServer();
            
            _collisionBucket?.Unsubscribe<ThrowingCollisionCommand>(OnThrowingCollisionCommand);
        }

        private void OnThrowingCollisionCommand(ThrowingCollisionCommand command)
        {
            var characterView = ViewsContainer.GetView<CharacterView>(command.ToKey);
            if (characterView.Health.Dead)
            {
                return;
            }
            
            var calculatedDamage = command.ItemDamage;
            // TODO: calculate damage here and send damage command
            // var partEntity = EntitiesContainer.GetEntity(command.FromPartKey);
            // calculatedDamage += partEntity.getPartDamageBonus();
            // var toPartEntity = EntitiesContainer.GetEntity(command.ToPartKey);
            // calculatedDamage -= toPartEntity.getPartDamageBonus();

            var damageCommand = new ThrowingDamageCommand(command.FromKey, command.ToKey, calculatedDamage);
            ServerManager.Broadcast(damageCommand);
            
            ChangeHealthServerLocal(characterView, command, calculatedDamage);
        }
        
        private void ChangeHealthServerLocal(CharacterView characterView, ThrowingCollisionCommand command, int damage)
        {
            var health = characterView.Health;
            health.Damage(command.FromKey, damage);
            
            if (!health.Dead)
            {
                return;
            }

            // Sending to respawn our character
            var deadCommand = new DeadHealthCommand(command.FromKey, command.ToKey);
            _healthBucket.Invoke(deadCommand);
        }
    }
}