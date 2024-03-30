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
        private AudioPlayerService _audioPlayerService;

        private void OnClientInitialize()
        {
            _audioPlayerService = Ioc.Instance.Get<AudioPlayerService>();
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
        internal void ThrowToAllClients(Vector3 direction)
        {
            transform.SetParent(null);

            _rigidbody.isKinematic = false;
            _collider.enabled = true;

            _rigidbody.AddForce(direction.normalized * _throwPower, ForceMode.Impulse);

            transform.DOScale(_startScale, _scaleReturnDuration);
            Invoke(nameof(AllowEveryone), _availabilityPauseDelay);
        }

        [ObserversRpc]
        private void DestroyToAllClients()
        {
            Instantiate(_destroyParticlePrefab, transform.position, Quaternion.identity);
        }

        private void AllowEveryone()
        {
            _isTaken = false;
            _ownerKey = string.Empty;
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