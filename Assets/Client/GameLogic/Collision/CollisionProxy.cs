using System;
using UnityEngine;

namespace Client.GameLogic.Collision
{
    public class CollisionProxy : ColliderDataControl
    {
        public event Action<string, string, ColliderDataControl, UnityEngine.Collision> OnCollided; 

        [SerializeField] private Collider _collider;

        private bool _enabled;
        
        public bool Enabled => _enabled;

        private void Awake()
        {
            Enable(_enabled);
        }

        public void Enable(bool enable)
        {
            _enabled = enable;
            _collider.enabled = enable;
        }

        private void OnCollisionEnter(UnityEngine.Collision collision)
        {
            if (!_enabled
                || !collision.transform.TryGetComponent<ColliderDataControl>(out var colliderData)
                || colliderData.Id == Id)
            {
                return;
            }

            Enable(false);

            OnCollided?.Invoke(CharacterEntityKey, OnCollisionEnterKey, colliderData, collision);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_enabled
                || !other.transform.TryGetComponent<ColliderDataControl>(out var colliderData)
                || colliderData.Id == Id)
            {
                return;
            }

            Enable(false);

            OnCollided?.Invoke(CharacterEntityKey, OnCollisionEnterKey, colliderData, null);
        }
    }
}