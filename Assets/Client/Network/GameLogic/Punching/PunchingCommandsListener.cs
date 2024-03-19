using Client.GameLogic.Collision;
using Client.GameLogic.Collision.Commands;
using Core.Application;
using Core.Ioc;
using FishNet;
using FishNet.Object;

namespace Client.Network.GameLogic.Punching
{
    public class PunchingCommandsListener : NetworkBehaviour, IApplicationResource
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
            
            _collisionBucket?.Subscribe<PunchCollisionCommand>(OnPunchCollisionCommand);
        }

        private void OnPunchCollisionCommand(PunchCollisionCommand command)
        {
            InstanceFinder.ServerManager.Broadcast(command);
        }
    }
}