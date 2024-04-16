using System;
using Client.Network.Entities;
using UnityEngine;

namespace Client.GameLogic.Throwing.Taking
{
    public abstract class TakingItemViewBase : NetworkEntityView
    {
        public abstract event Action OnTaken;
        public abstract event Action OnDropped;

        [SerializeField] protected Rigidbody _rigidbody;
        [SerializeField] protected Collider _collider;
        [SerializeField] protected float _availabilityPauseDelay = 2f;
        [SerializeField] protected float _takenSize = 1f;
        [SerializeField] protected float _scaleReturnDuration = 0.3f;

        [Header("Holding settings")]
        [SerializeField] protected Vector3 _takingItemOffset;
        [SerializeField] protected Vector3 _takingItemRotation;

        protected bool _isTaken;
        protected string _ownerKey;
        protected Vector3 _startScale;

        public bool HasOwner => _isTaken || !string.IsNullOrEmpty(_ownerKey);

        public bool IsThrowing => _collider.enabled;

        public Vector3 TakingItemOffset => _takingItemOffset;

        public Vector3 TakingItemRotation => _takingItemRotation;

        public abstract void Take(string ownerKey);

        public abstract void Drop(Vector3 direction);

        internal protected abstract void TakeToAllClients(string ownerKey);
        internal protected abstract void DropToAllClients(Vector3 direction);

        internal protected virtual void AllowEveryone()
        {
            _isTaken = false;
            _ownerKey = string.Empty;
        }
    }
}