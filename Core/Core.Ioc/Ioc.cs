using System;
using Core.Utilities;

namespace Core.Ioc
{
    public class Ioc: Singleton<Ioc>, IDisposable
    {
        private readonly IContainer _container = Factory.CreateContainer();
        
        public void Dispose()
        {
            _container.Dispose();
        }

        public void InjectDependencies(object instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            _container.InjectDependencies(instance);
        }

        public void RegisterInstance(Type type, object instance)
        {
            _container.RegisterInstance(type, instance);
        }
        
        public void Register(Type type)
        {
            _container.Register(type);
        }
        
        public void RegisterResolver(Type type, ICustomResolver resolver)
        {
            _container.RegisterResolver(type, resolver);
        }

        public T Get<T>() where T: class
        {
            return _container.Get<T>();
        }

        public object Get(Type type)
        {
            return _container.Get(type);
        }
        
        public void UnregisterInstance(Type type, object instance)
        {
            _container.UnregisterInstance(type, instance);
        }
        
        public void Unregister(Type type, object instance)
        {
            _container.Unregister(type, instance);
        }
        
        public void UnregisterResolver(ICustomResolver resolver)
        {
            _container.UnregisterResolver(resolver);
        }
    }
}
