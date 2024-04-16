using Client.GameLogic.Collision;
using Client.GameLogic.Throwing.Commands;
using Core.Ioc;
using FishNet.Object;
using UnityEngine;

namespace Client.GameLogic.Throwing
{
    public partial class ThrowingItemView
    {
        private CollisionBucket _collisionBucket;

        private void OnServerInitialize()
        {
            _collisionBucket = Ioc.Instance.Get<CollisionBucket>();
        }

        [ServerRpc(RequireOwnership = false)]
        public override void Take(string ownerKey)
        {
            if (_isTaken)
            {
                return;
            }

            TakeToAllClients(ownerKey);
        }

        [ServerRpc(RequireOwnership = false)]
        public override void Drop(Vector3 direction)
        {
            if (!_isTaken || IsThrowing)
            {
                return;
            }

            DropToAllClients(direction);
        }

        private void OnTriggerEnterServer(Collider other, ColliderDataControl colliderData)
        {
            if (!IsServerInitialized)
            {
                return;
            }

            var command = new ThrowingCollisionCommand(_ownerKey, colliderData.CharacterEntityKey, _damage, colliderData.OnCollisionEnterKey);
            _collisionBucket.Invoke(command);

            if (_destroyParticlePrefab != null)
            {
                DestroyToAllClients();
            }

            if (_isDestroyable)
            {
                Despawn();
            }
        }

        private void OnCollisionEnterServer(UnityEngine.Collision other)
        {
            if (!IsServerInitialized)
            {
                return;
            }

            if (_destroyParticlePrefab != null)
            {
                DestroyToAllClients();
            }

            if (_isDestroyable)
            {
                Despawn();
            }
        }
    }
}