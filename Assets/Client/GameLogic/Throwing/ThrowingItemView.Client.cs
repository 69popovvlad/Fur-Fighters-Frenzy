using System;
using Client.Audio;
using Client.GameLogic.Collision;
using Core.Ioc;
using DG.Tweening;
using FishNet.Object;
using UnityEngine;

namespace Client.GameLogic.Throwing
{
    public partial class ThrowingItemView
    {
        public override event Action OnTaken;
        public override event Action OnDropped;

        private AudioPlayerService _audioPlayerService;

        private void OnClientInitialize()
        {
            _audioPlayerService = Ioc.Instance.Get<AudioPlayerService>();
        }

        [ObserversRpc(RunLocally = true)]
        internal protected override void TakeToAllClients(string ownerKey)
        {
            _isTaken = true;
            _ownerKey = ownerKey;

            _rigidbody.isKinematic = true;
            _collider.enabled = false;

            _startScale = transform.localScale;
            transform.localScale = _startScale * _takenSize;

            OnTaken?.Invoke();
        }

        [ObserversRpc(RunLocally = true)]
        internal protected override void DropToAllClients(Vector3 direction)
        {
            transform.SetParent(null);

            _rigidbody.isKinematic = false;
            _collider.enabled = true;

            _rigidbody.AddForce(direction.normalized * _throwPower, ForceMode.Impulse);

            transform.DOScale(_startScale, _scaleReturnDuration);
            Invoke(nameof(AllowEveryone), _availabilityPauseDelay);

            OnDropped?.Invoke();
        }

        [ObserversRpc(RunLocally = false)]
        private void DestroyToAllClients()
        {
            Instantiate(_destroyParticlePrefab, transform.position, Quaternion.identity);
        }

        private void OnTriggerEnterClient(Collider other, ColliderDataControl colliderData)
        {
            if (!IsClientInitialized)
            {
                return;
            }

            _audioPlayerService.PlayClip(other.transform.position, _collisionSoundKey);
        }

        private void OnCollisionEnterClient(UnityEngine.Collision other)
        {
            if (!IsClientInitialized)
            {
                return;
            }

            _audioPlayerService.PlayClip(other.transform.position, _collisionSoundKey);
        }
    }
}