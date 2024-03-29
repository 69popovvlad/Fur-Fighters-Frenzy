using Core.Entities;
using Core.Entities.Views;
using FishNet.Object;

namespace Client.Network.Entities
{
    public abstract class NetworkEntityView: NetworkBehaviour, IEntityView
    {
        protected IEntity Entity;
        
        public string Guid => Entity.Guid;
        
        public void Initialize(IEntity entity)
        {
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
        
        private void OnDestroy()
        {
            DeinitializationInternal();
            Entity?.Dispose();
            ViewsContainer.RemoveEntity(this);
        }
    }
}