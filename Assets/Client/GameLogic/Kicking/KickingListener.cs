using Client.Audio;
using Client.GameLogic.Collision;
using Client.GameLogic.Collision.Commands;
using Core.Ioc;
using UnityEngine;

namespace Client.GameLogic.Kicking
{
    public class KickingListener : MonoBehaviour
    {
        [SerializeField] private CollisionProxy _kickCollisionProxy;

        private CollisionBucket _collisionBucket;
        private AudioPlayerService _audioPlayerService;

        private void Awake()
        {
            var ioc = Ioc.Instance;
            _audioPlayerService = ioc.Get<AudioPlayerService>();
            _collisionBucket = ioc.Get<CollisionBucket>();

            _kickCollisionProxy.OnCollided += OnPunchCollision;
        }

        private void OnDestroy()
        {
            _kickCollisionProxy.OnCollided -= OnPunchCollision;
        }

        private void OnPunchCollision(string entityKey, string partKey, ColliderDataControl colliderData, UnityEngine.Collision collision)
        {
            var command = new KickCollisionCommand(entityKey, partKey, colliderData.CharacterEntityKey);
            _collisionBucket.Invoke(command);

            _audioPlayerService.PlayClip(colliderData.transform.position, "leg_kick");
        }
    }
}