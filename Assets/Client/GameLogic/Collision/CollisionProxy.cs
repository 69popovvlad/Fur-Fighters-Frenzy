using System;
using UnityEngine;
using UnityEngine.Events;

namespace Client.GameLogic.Collision
{
    public class CollisionProxy : ColliderDataControl
    {
        [SerializeField] private Collider _collider;

        [SerializeField] private UnityEvent<string, string, ColliderDataControl>[] _onCollisionEnter =
            Array.Empty<UnityEvent<string, string, ColliderDataControl>>();

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

            for (int i = 0, iLen = _onCollisionEnter.Length; i < iLen; ++i)
            {
                _onCollisionEnter[i].Invoke(CharacterEntityKey, OnCollisionEnterKey, colliderData);
            }
        }
    }
}