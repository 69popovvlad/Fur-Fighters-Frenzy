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

        [Header("Debug")]
        [SerializeField] private GameObject _debugPointPrefab;
        [SerializeField] private float _debugPointLifetime = 3;

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

        private void OnPunchCollision(string entityKey, string partKey, ColliderDataControl colliderData, UnityEngine.Collision collision)
        {
            if (entityKey.Equals(colliderData.CharacterEntityKey))
            {
                return;
            }

            var command = new PunchCollisionCommand(entityKey, partKey, colliderData.CharacterEntityKey, colliderData.OnCollisionEnterKey);
            _collisionBucket.Invoke(command);

            _audioPlayerService.PlayClip(colliderData.transform.position, "punch");

#if UNITY_EDITOR
            if (collision != null)
            {
                var instance = Instantiate(_debugPointPrefab, collision.contacts[0].point, Quaternion.identity);
                instance.transform.SetParent(colliderData.transform);
                Destroy(instance, _debugPointLifetime);
            }
#endif
        }
    }
}