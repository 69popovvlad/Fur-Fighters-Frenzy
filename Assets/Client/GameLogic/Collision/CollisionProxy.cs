using System;
using System.Collections.Generic;
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
            
#if UNITY_EDITOR
            _collisions.Add((other.contacts[0].point, 5));
#endif
            
        }

#if UNITY_EDITOR

        private readonly List<(Vector3 position, float liveDuration)> _collisions = new List<(Vector3, float)>();
        
        private void OnDrawGizmos()
        {
            for (int i = 0, iLen = _collisions.Count; i < iLen; ++i)
            {
                var point = _collisions[i];
                if (point.liveDuration <= 0)
                {
                    continue;   
                }
                
                Gizmos.DrawSphere(point.position, 0.05f);

                point.liveDuration -= Time.deltaTime;
            }
        }
#endif
    }
}