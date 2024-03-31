using UnityEngine;
using FishNet.Object;
using Client.GameLogic.Throwing;

namespace Client.GameLogic.Slipping
{
    public class SlipperyItemView : NetworkBehaviour
    {
        [SerializeField] private Collider _collider;
        [SerializeField] private float _actionPause = 2;

        [Header("For throwing item")]
        [SerializeField] private bool _isThrowingObject;
        [SerializeField] private ThrowingItemView _throwingItemView;

        [Header("After slipping")]
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float _impulsePower = 5;

        private bool _isTaken;

        private void Awake()
        {
            if (!_isThrowingObject)
            {
                return;
            }

            _throwingItemView.OnTaken += OnTaken;
            _throwingItemView.OnThrown += OnThrown;
        }

        private void OnDestroy()
        {
            if (!_isThrowingObject)
            {
                return;
            }

            _throwingItemView.OnTaken -= OnTaken;
            _throwingItemView.OnThrown -= OnThrown;
        }

        private void OnCollisionEnter(UnityEngine.Collision other)
        {
            if (_isTaken
                || !IsServerInitialized
                || !other.transform.TryGetComponent<SlippingControl>(out var component))
            {
                return;
            }

            component.Slip();
            var direction = transform.position - other.transform.position;
            AddImpulse(direction);
        }


        [ObserversRpc(RunLocally = true)]
        private void DisableColliderToAllClients()
        {
            _collider.enabled = false;
            Invoke(nameof(EnableColliderToAllClients), _actionPause);
        }

        private void EnableColliderToAllClients()
        {
            _collider.enabled = true;
        }

        private void AddImpulse(Vector3 direction)
        {
            _rigidbody.AddForce(direction.normalized * _impulsePower, ForceMode.Impulse);
        }

        private void OnTaken()
        {
            _isTaken = true;
            _collider.enabled = false;
        }

        private void OnThrown()
        {
            _isTaken = false;
            _collider.enabled = true;
        }
    }
}