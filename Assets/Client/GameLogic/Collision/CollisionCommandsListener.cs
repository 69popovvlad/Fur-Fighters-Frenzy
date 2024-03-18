using Client.GameLogic.Collision.Commands;
using Core.Application;
using UnityEngine;

namespace Client.GameLogic.Collision
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class CollisionCommandsListener : IApplicationResource
    {
        private readonly CollisionBucket _collisionBucket;

        public CollisionCommandsListener(CollisionBucket collisionBucket)
        {
            _collisionBucket = collisionBucket;

            _collisionBucket.Subscribe<PunchCollisionCommand>(OnPunchCollisionCommand);
        }

        ~CollisionCommandsListener()
        {
            _collisionBucket?.Subscribe<PunchCollisionCommand>(OnPunchCollisionCommand);
        }

        private void OnPunchCollisionCommand(PunchCollisionCommand command)
        {
            Debug.Log($"{command.FromPartKey} => {command.ToPartKey}");
            
            // throw new System.NotImplementedException();
            // TODO: calculate damage here and send damage command 
        }
    }
}