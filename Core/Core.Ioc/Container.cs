using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Core.Collections.Utilities;
using Core.Ioc.Exceptions;

namespace Core.Ioc
{
    [SuppressMessage("ReSharper", "ForCanBeConvertedToForeach")]
    [SuppressMessage("ReSharper", "ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator")]
    internal partial class Container: IContainer
    {
        const BindingFlags ConstructorsFlags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
        const BindingFlags MemberFlags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
        
        public event Action<object> Resolved;
        
        private readonly ConstructorsComparer _constructorsComparer = new ConstructorsComparer();
        private readonly ParameterInfo[] _emptyParams = Array.Empty<ParameterInfo>();
        
        private readonly Dictionary<Type, ICustomResolver> _resolvers = new Dictionary<Type, ICustomResolver>();
        private readonly Dictionary<Type, object> _instances = new Dictionary<Type, object>();
        private readonly HashSet<Type> _usedTypes = new HashSet<Type>();
        
        public void Dispose()
        {
            _resolvers.Clear();

            foreach (var (_, value) in _instances)
            {
                if (value is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }
        }

        public void InjectDependencies(object instance)
        {
            InjectDependencies(instance.GetType(), _emptyParams, ref instance);
        }
        
        public void RegisterInstance(Type type, object instance)
        {
            if (_instances.ContainsKey(type))
            {
                throw new InstanceOfTypeExistsException(type);
            }
            
            _instances[type] = instance;
            InjectDependencies(instance);
        }

        public void Register(Type type)
        {
            if (type.IsAbstract)
            {
                return;
            }
            
            _instances[type] = Get(type);
        }

        public void Register(Type type, Type implementation)
        {
            _instances[type] = Get(implementation);
        }

        public void RegisterResolver(Type type, ICustomResolver resolver)
        {
            _resolvers[type] = resolver ?? throw new ArgumentException(nameof(resolver));

            _instances.Remove(type);
        }

        public T Get<T>() where T: class
        {
            return (T) Get(typeof(T));
        }

        public object Get(Type type)
        {
            if (_resolvers.TryGetValue(type, out var resolver))
            {
                return resolver.Resolve(type, this);
            }
            
            if (_instances.TryGetValue(type, out var instance))
            {
                return instance;
            }

            if (TryGetInstance(type, out instance))
            {
                return instance;
            }

            if (!_usedTypes.Add(type))
            {
                throw new DependenciesCycleException(this, type);
            }

            var constructors =  type.GetConstructors(ConstructorsFlags);
            Array.Sort(constructors, _constructorsComparer);

            Exception exception = null;
            for (var i = 0; i < constructors.Length; i++)
            {
                var constructorInfo = constructors[i];
                var parameters = constructorInfo.GetParameters();
                if (!ValidateConstructorsParametersTypes(parameters, out var unsupportedType))
                {
                    exception = new ConstructorParametersUnsupportedException(unsupportedType, constructorInfo);
                    continue;
                }

                var resolveParameters = new object[parameters.Length];
                for (var j = 0; j < parameters.Length; ++j)
                {
                    resolveParameters[j] = Get(parameters[j].ParameterType);
                }

                return CreateInstance(type, constructorInfo, resolveParameters);
            }

            if (exception != null)
            {
                throw exception;
            }

            throw new ConstructorUndefinedException(type);
        }
        
        public void UnregisterInstance(Type type, object instance)
        {
            if (!_instances.ContainsKey(type))
            {
                throw new InstanceOfTypeNotExistsException(type);
            }
            
            _instances.Remove(type);
        }

        public void Unregister(Type type, object instance)
        {
            _instances.Remove(type);    
        }

        public void UnregisterResolver(ICustomResolver resolver)
        {
            var resolvers = new List<Type>();
            
            foreach (var kvp in _resolvers)
            {
                if (kvp.Value == resolver)
                {
                    resolvers.Add(kvp.Key);
                }
            }

            for (var i = 0; i < resolvers.Count; ++i)
            {
                _resolvers.Remove(resolvers[i]);
            }
        }

        private object CreateInstance(Type type, ConstructorInfo constructorInfo, object[] parameters)
        {
            _usedTypes.Remove(type);
            
            var instance = constructorInfo.Invoke(parameters);
            _instances[type] = instance;
            InjectDependencies(type, constructorInfo.GetParameters(), ref instance);
            
            Resolved?.Invoke(instance);
            return instance;
        }
        
        private bool TryGetInstance(Type type, out object instance)
        {
            foreach (var (_, value) in _instances)
            {
                if (type == value.GetType())
                {
                    instance = value;
                    return true;
                }
            }

            instance = null;
            return false;
        }

        private bool ValidateConstructorsParametersTypes(ParameterInfo[] parameterInfos, out Type unsupportedType)
        {
            for (var i = 0; i < parameterInfos.Length; ++i)
            {
                var parameterType = parameterInfos[i].ParameterType;
                if (parameterType.IsPrimitive || parameterType.IsInterface && !_resolvers.ContainsKey(parameterType))
                {
                    unsupportedType = parameterType;
                    return false;
                }
            }
            
            unsupportedType = null;
            return true;
        }
    }
}
