using Client.GameLogic.Health;
using Client.Network.Entities;
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

        private bool _isTaken;
        private string _ownerKey;
        private ThrowingItemEntity _entity;

        public bool IsTaken => _isTaken;

        public string OwnerKey => _ownerKey;

        public override void OnStartNetwork()
        {
            base.OnStartNetwork();

            _entity = new ThrowingItemEntity(ObjectId.ToString());
            Initialize(_entity);
        }

        [ServerRpc(RequireOwnership = false)]
        public void Take(string ownerKey)
        {
            TakeToAllClients(ownerKey);
        }

        [ObserversRpc(RunLocally = true)]
        private void TakeToAllClients(string ownerKey)
        {
            if (_isTaken)
            {
                return;
            }

            _isTaken = true;
            _ownerKey = ownerKey;

            _rigidbody.isKinematic = true;
            _collider.enabled = false;
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
        private void ThrowToAllClients(Vector3 direction)
        {
            _rigidbody.isKinematic = false;
            _collider.enabled = true;

            transform.SetParent(null);
            _rigidbody.AddForce(direction.normalized * _throwPower, ForceMode.Impulse);
            
            _isTaken = false;
            _ownerKey = string.Empty;
        }

        private void OnCollisionEnter(UnityEngine.Collision other)
        {
            if (!IsServerInitialized)
            {
                return;
            }
            
            if (_isTaken || !other.transform.TryGetComponent<HealthControl>(out var health))
            {
                return;
            }
            
            health.Damage(_ownerKey, _damage);
        }
    }
}