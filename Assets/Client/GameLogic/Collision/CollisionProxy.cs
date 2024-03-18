using System;
using UnityEngine;
using UnityEngine.Events;

namespace Client.GameLogic.Collision
{
    public class CollisionProxy : ColliderDataControl
    {
        [SerializeField]
        private UnityEvent<string, string, ColliderDataControl>[] _onCollisionEnter =
            Array.Empty<UnityEvent<string, string, ColliderDataControl>>();

        private bool _enabled;
        
        public void Enable(bool enable)
        {
            _enabled = enable;
        }
        
        private void OnCollisionEnter(UnityEngine.Collision other)
        {
            if (!_enabled
                || !other.transform.TryGetComponent<ColliderDataControl>(out var colliderData)
                || colliderData.Id == Id)
            {
                return;
            }

            for (int i = 0, iLen = _onCollisionEnter.Length; i < iLen; ++i)
            {
                _onCollisionEnter[i].Invoke(CharacterEntityKey, OnCollisionEnterKey, colliderData);
            }
        }
    }
}