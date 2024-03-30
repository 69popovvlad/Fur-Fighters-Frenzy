using Client.GameLogic.Collision;
using Client.Network.Entities;
using UnityEngine;

namespace Client.GameLogic.Throwing
{
    public partial class ThrowingItemView : NetworkEntityView
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Collider _collider;
        [SerializeField] private float _throwPower = 10;
        [SerializeField] private int _damage = 2;
        [SerializeField] private float _availabilityPauseDelay = 0.5f;
        [SerializeField] private float _takenSize = 0.2f;
        [SerializeField] private float _scaleReturnDuration = 0.5f;

        [Header("Holding settings")]
        [SerializeField] private Vector3 _takingItemOffset;
        [SerializeField] private Vector3 _takingItemRotation;

        [Header("Additional settings")]
        [SerializeField] private bool _isDestroyable;
        [SerializeField] private GameObject _destroyParticlePrefab;

        [Header("Audio settings")]
        [SerializeField] private string _collisionSoundKey;

        private bool _isTaken;
        private string _ownerKey;
        private Vector3 _startScale;
        private ThrowingItemEntity _entity;

        public bool HasOwner => _isTaken || !string.IsNullOrEmpty(_ownerKey);
        public bool IsThrowing => _collider.enabled;

        public Vector3 TakingItemOffset => _takingItemOffset;
        public Vector3 TakingItemRotation => _takingItemRotation;

        public override void OnStartNetwork()
        {
            base.OnStartNetwork();

            _entity = new ThrowingItemEntity(ObjectId.ToString());
            Initialize(_entity);
        }

        public override void OnStartServer()
        {
            base.OnStartServer();

            if (IsServerInitialized)
            {
                OnServerInitialize();
            }
        }

        public override void OnStartClient()
        {
            base.OnStartClient();

            if (IsClientInitialized)
            {
                OnClientInitialize();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!HasOwner)
            {
                return;
            }

            if (!other.TryGetComponent<ColliderDataControl>(out var colliderData)
                || _ownerKey.Equals(colliderData.CharacterEntityKey))
            {
                return;
            }

            OnTriggerEnterClient(other, colliderData);
            OnTriggerEnterServer(other, colliderData);
        }

        private void OnCollisionEnter(UnityEngine.Collision other)
        {
            if (!HasOwner)
            {
                return;
            }

            OnCollisionEnterClient(other);
            OnCollisionEnterServer(other);
        }
    }
}