using System;

namespace Core.Ioc
{
    public interface IContainer: IDisposable
    {
        event Action<object> Resolved;

        void InjectDependencies(object instance);

        void RegisterInstance(Type type, object instance);
        
        void Register(Type type);
        
        void Register(Type type, Type implementation);
        
        void RegisterResolver(Type type, ICustomResolver resolver);

        T Get<T>() where T: class;
        
        object Get(Type type);
        
        void UnregisterInstance(Type type, object instance);

        void Unregister(Type type, object instance);
        
        void UnregisterResolver(ICustomResolver resolver);
    }
}