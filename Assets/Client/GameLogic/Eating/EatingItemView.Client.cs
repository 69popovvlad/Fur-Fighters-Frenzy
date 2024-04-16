using FishNet.Object;
using UnityEngine;

namespace Client.GameLogic.Eating
{
    public partial class EatingItemView
    {
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
        protected internal override void DropToAllClients(Vector3 direction)
        {
            /* Nothing to do */
        }

        [ObserversRpc(RunLocally = false)]
        private void DestroyToAllClients()
        {
            Instantiate(_destroyParticlePrefab, transform.position, Quaternion.identity);
            _audioPlayerService.PlayClip(transform.position, "eating");
        }
    }
}