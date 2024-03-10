using System;

namespace Core.Entities
{
    public interface IEntity: IDisposable
    {
        event Action Disposed;
        
        string Guid { get; }
    }
}