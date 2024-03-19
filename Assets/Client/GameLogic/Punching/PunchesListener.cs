using Client.GameLogic.Collision;
using Client.GameLogic.Collision.Commands;
using Core.Ioc;
using UnityEngine;

namespace Client.GameLogic.Punching
{
    public class PunchesListener : MonoBehaviour
    {
        [Header("Left arm")]
        [SerializeField] private CollisionProxy _leftCollisionProxy;

        [Header("Right arm")]
        [SerializeField] private CollisionProxy _rightCollisionProxy;

        private CollisionBucket _collisionBucket;

        private void Awake()
        {
            _collisionBucket = Ioc.Instance.Get<CollisionBucket>();
            
            _leftCollisionProxy.OnCollided += OnPunchCollision;
            _rightCollisionProxy.OnCollided += OnPunchCollision;
        }

        private void OnDestroy()
        {
            _leftCollisionProxy.OnCollided -= OnPunchCollision;
            _rightCollisionProxy.OnCollided -= OnPunchCollision;
        }
        
        private void OnPunchCollision(string entityKey, string partKey, ColliderDataControl colliderData)
        {
            var command = new PunchCollisionCommand(entityKey, partKey, colliderData.CharacterEntityKey, colliderData.OnCollisionEnterKey);
            _collisionBucket.Invoke(command);
        }
    }
}