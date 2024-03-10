using System;
using UnityEngine;

namespace Core.Entities.Views
{
    public abstract class EntityView: MonoBehaviour
    {
        protected IEntity Entity;
        private bool _initialized;
        
        public string Guid => Entity.Guid;

        public void Initialize(IEntity entity)
        {
            Entity = entity;
            Entity.Disposed += OnEntityDisposed;
            ViewsContainer.AddEntity(this);

            InitializeInternal();
            
            _initialized = true;
        }

        /// <summary>
        /// Here you should refer to a specific entity type and subscribe to events
        /// </summary>
        protected virtual void InitializeInternal()
        {
            /* Nothing to do */
        }

        private void Start()
        {
            if (!_initialized)
            {
                throw new Exception($"{gameObject.name} with type {GetType().Name} should be initialized first");
            }
        }

        private void OnDestroy()
        {
            ViewsContainer.RemoveEntity(this);
        }

        private void OnEntityDisposed()
        {
            Entity.Disposed -= OnEntityDisposed;
            Destroy(gameObject);
        }
    }
}