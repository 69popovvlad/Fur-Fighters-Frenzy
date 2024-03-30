using Core.Entities;
using Core.Entities.Views;
using FishNet.Object;

namespace Client.Network.Entities
{
    public abstract class NetworkEntityView : NetworkBehaviour, IEntityView
    {
        protected IEntity Entity;
        private bool _initialized;

        public string Guid => Entity.Guid;

        /// <summary>
        /// Initialize in OnStartNetwork
        /// </summary>
        /// <param name="entity">Entity for this view</param>
        public void Initialize(IEntity entity)
        {
            if (_initialized)
            {
                throw new System.Exception($"{nameof(NetworkEntityView)} {gameObject.name} {Guid} initialized already");
            }

            _initialized = true;
            Entity = entity;
            ViewsContainer.AddEntity(this);

            InitializeInternal();
        }

        /// <summary>
        /// Here you should refer to a specific entity type and subscribe to events
        /// </summary>
        protected virtual void InitializeInternal()
        {
            /* Nothing to do */
        }

        /// <summary>
        /// Here you should refer to a specific entity type and unsubscribe from events
        /// </summary>
        protected virtual void DeinitializationInternal()
        {
            /* Nothing to do */
        }

        public override void OnStopNetwork()
        {
            base.OnStopNetwork();

            if (!_initialized)
            {
                return;
            }

            _initialized = false;
            
            DeinitializationInternal();
            ViewsContainer.RemoveEntity(this);
            Entity?.Dispose();
        }
    }
}