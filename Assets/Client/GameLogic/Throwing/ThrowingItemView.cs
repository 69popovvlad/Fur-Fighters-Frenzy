using Client.GameLogic.Collision;
using Client.GameLogic.Throwing.Taking;
using UnityEngine;

namespace Client.GameLogic.Throwing
{
    public partial class ThrowingItemView : TakingItemViewBase
    {
        [SerializeField] private float _throwPower = 10;
        [SerializeField] private int _damage = 2;

        [Header("Additional settings")]
        [SerializeField] private bool _isDestroyable;
        [SerializeField] private GameObject _destroyParticlePrefab;

        [Header("Audio settings")]
        [SerializeField] private string _collisionSoundKey;

        private ThrowingItemEntity _entity;

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