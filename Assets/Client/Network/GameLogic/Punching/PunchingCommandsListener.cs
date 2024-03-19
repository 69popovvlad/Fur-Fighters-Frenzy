using Client.GameLogic.Collision;
using Client.GameLogic.Collision.Commands;
using Core.Ioc;
using FishNet;
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
            InstanceFinder.ServerManager.Broadcast(command);
        }
    }
}