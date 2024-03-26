using Client.GameLogic.Collision;
using Client.GameLogic.Throwing.Commands;
using Client.Network.Entities;
using Core.Ioc;
using DG.Tweening;
using FishNet.Object;
using UnityEngine;

namespace Client.GameLogic.Throwing
{
    public class ThrowingItemView : NetworkEntityView
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Collider _collider;
        [SerializeField] private float _throwPower = 10;
        [SerializeField] private int _damage = 2;
        [SerializeField] private float _availabilityPauseDelay = 0.5f;
        [SerializeField] private float _takenSize = 0.2f;
        [SerializeField] private float _scaleReturnDuration = 0.5f;

        private bool _isTaken;
        private string _ownerKey;
        private ThrowingItemEntity _entity;
        private CollisionBucket _collisionBucket;
        private Vector3 _startScale;

        public override void OnStartNetwork()
        {
            base.OnStartNetwork();

            _entity = new ThrowingItemEntity(ObjectId.ToString());
            Initialize(_entity);

            if (!IsServerInitialized)
            {
                return;
            }

            _collisionBucket = Ioc.Instance.Get<CollisionBucket>();
        }

        [ServerRpc(RequireOwnership = false)]
        public void Take(string ownerKey)
        {
            if (_isTaken)
            {
                return;
            }
            
            TakeToAllClients(ownerKey);
        }

        [ServerRpc(RequireOwnership = false)]
        public void Throw(Vector3 direction)
        {
            if (!_isTaken)
            {
                return;
            }

            ThrowToAllClients(direction);
        }

        [ObserversRpc(RunLocally = true)]
        private void TakeToAllClients(string ownerKey)
        {
            _isTaken = true;
            _ownerKey = ownerKey;

            _rigidbody.isKinematic = true;
            _collider.enabled = false;

            _startScale = transform.localScale;
            transform.localScale = _startScale * _takenSize;
        }

        [ObserversRpc(RunLocally = true)]
        private void ThrowToAllClients(Vector3 direction)
        {
            transform.SetParent(null);
            
            _rigidbody.isKinematic = false;
            _collider.enabled = true;

            _rigidbody.AddForce(direction.normalized * _throwPower, ForceMode.Impulse);
            
            transform.DOScale(_startScale, _scaleReturnDuration);
            Invoke(nameof(AllowEveryone), _availabilityPauseDelay);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!IsServerInitialized
                || string.IsNullOrEmpty(_ownerKey)
                || !other.TryGetComponent<ColliderDataControl>(out var colliderData)
                || _ownerKey.Equals(colliderData.CharacterEntityKey))
            {
                return;
            }
            
            var command = new ThrowingCollisionCommand(_ownerKey, colliderData.CharacterEntityKey, _damage, colliderData.OnCollisionEnterKey);
            _collisionBucket.Invoke(command);
        }

        private void AllowEveryone()
        {
            _isTaken = false;
            _ownerKey = string.Empty;
        }
    }
}