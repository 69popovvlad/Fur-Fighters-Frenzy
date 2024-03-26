using Client.GameLogic.Health;
using Client.Network.Entities;
using FishNet.Object;
using UnityEngine;

namespace Client.GameLogic.Throwing
{
    public class ThrowingItemView : NetworkEntityView
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float _throwPower = 10;
        [SerializeField] private int _damage = 2;

        private bool _isTaken;
        private string _ownerKey;

        public bool IsTaken => _isTaken;

        public string OwnerKey => _ownerKey;

        public override void OnStartNetwork()
        {
            base.OnStartNetwork();

            if (IsServerInitialized)
            {
                return;
            }

            Destroy(this);
        }

        [ServerRpc]
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
            
            _rigidbody.Sleep();
        }

        [ServerRpc]
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
            _rigidbody.WakeUp();
            transform.SetParent(null);
            _rigidbody.AddForce(direction.normalized * _throwPower, ForceMode.Impulse);
            
            _isTaken = false;
            _ownerKey = string.Empty;
        }

        private void OnCollisionEnter(UnityEngine.Collision other)
        {
            if (!_isTaken || !other.transform.TryGetComponent<HealthControl>(out var health))
            {
                return;
            }
            
            health.Damage(_ownerKey, _damage);
        }
    }
}