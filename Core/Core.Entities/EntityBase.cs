using System;

namespace Core.Entities
{
    public abstract class EntityBase: IEntity
    {
        public event Action Disposed;
        
        public string Guid { get; private set; }

        protected EntityBase(string guid = null)
        {
            Guid = guid ?? EntitiesContainer.GenerateEntityGuid();
            EntitiesContainer.AddEntity(this);
        }

        public void Dispose()
        {
            Disposed?.Invoke();
            EntitiesContainer.RemoveEntity(this);
        }
    }
}