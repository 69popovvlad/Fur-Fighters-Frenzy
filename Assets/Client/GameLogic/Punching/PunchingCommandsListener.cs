using Client.GameLogic.Characters;
using Client.GameLogic.Collision;
using Client.GameLogic.Collision.Commands;
using Core.Application;
using Core.Entities.Views;
using UnityEngine;

namespace Client.GameLogic.Punching
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class PunchingCommandsListener : IApplicationResource
    {
        private readonly CollisionBucket _collisionBucket;

        public PunchingCommandsListener(CollisionBucket collisionBucket)
        {
            _collisionBucket = collisionBucket;

            _collisionBucket.Subscribe<PunchCollisionCommand>(OnPunchCollisionCommand);
        }

        ~PunchingCommandsListener()
        {
            _collisionBucket?.Subscribe<PunchCollisionCommand>(OnPunchCollisionCommand);
        }

        private void OnPunchCollisionCommand(PunchCollisionCommand command)
        {
            Debug.Log($"{command.FromPartKey} => {command.ToPartKey}");

            var character = ViewsContainer.GetView<CharacterView>(command.ToKey);
            var damage = 1;
            // TODO: calculate damage here and send damage command
            // var partEntity = EntitiesContainer.GetEntity(command.FromPartKey);
            // damage += partEntity.getPartDamageBonus();
            
            character.Health.Damage(command.FromKey, damage);
        }
    }
}