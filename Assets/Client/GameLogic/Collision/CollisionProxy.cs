using UnityEngine;
using UnityEngine.Events;

namespace Client.GameLogic.Collision
{
    public class CollisionProxy : ColliderDataControl
    {
        [SerializeField] private Collider _collider;

        [SerializeField] private UnityEvent<string, string, ColliderDataControl> _onCollisionEnter;

        private bool _enabled;

        private void Awake()
        {
            Enable(_enabled);
        }

        public void Enable(bool enable)
        {
            _enabled = enable;
            _collider.enabled = enable;
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

            _onCollisionEnter.Invoke(CharacterEntityKey, OnCollisionEnterKey, colliderData);
        }
    }
}