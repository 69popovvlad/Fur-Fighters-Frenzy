using Client.Audio;
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
        private AudioPlayerService _audioPlayerService;

        private void Awake()
        {
            var ioc = Ioc.Instance;
            _audioPlayerService = ioc.Get<AudioPlayerService>();
            _collisionBucket = ioc.Get<CollisionBucket>();

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
            if (entityKey.Equals(colliderData.CharacterEntityKey))
            {
                return;
            }

            var command = new PunchCollisionCommand(entityKey, partKey, colliderData.CharacterEntityKey, colliderData.OnCollisionEnterKey);
            _collisionBucket.Invoke(command);

            _audioPlayerService.PlayClip(colliderData.transform.position, "punch");
        }
    }
}