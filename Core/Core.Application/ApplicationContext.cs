using System;
using System.Collections.Generic;
using Core.Application.Events;
using Core.Reflection;

namespace Core.Application
{
   public partial class ApplicationContext: MonoBehaviourSingleton<ApplicationContext>, IDisposable
    {
        private readonly List<IUnityUpdateListener> _updateListeners = new List<IUnityUpdateListener>();
        private readonly List<IUnityApplicationPauseListener> _applicationPauseListeners = new List<IUnityApplicationPauseListener>();
        private readonly List<IUnityApplicationQuitListener> _applicationQuitListeners = new List<IUnityApplicationQuitListener>();

        public void Dispose()
        {
            _updateListeners.Clear();
            _applicationPauseListeners.Clear();
        }

        public void Run()
        {
            var ioc = Ioc.Ioc.Instance;
            
            RegisterApplicationResources(ioc);
        }

        private void RegisterApplicationResources(Ioc.Ioc ioc)
        {
            foreach (var type in ReflectionUtilities.GetImplementationsOf<IApplicationResource>(AssembliesType.My).Values)
            {
                if (type.IsAbstract)
                {
                    continue;
                }
                
                ioc.Register(type);
                var instance = ioc.Get(type);
                ioc.InjectDependencies(instance);

                if (instance is IInitializableResource initializableResource)
                {
                    initializableResource.Initialize();
                }
                    
                RegisterUnityEventListeners(instance);
            }
        }
        
        private void RegisterUnityEventListeners(object instance)
        {
            if (instance is IUnityUpdateListener updateListener)
            {
                _updateListeners.Add(updateListener);
            }
            
            if (instance is IUnityApplicationPauseListener pauseListener)
            {
                _applicationPauseListeners.Add(pauseListener);
            }
            
            if (instance is IUnityApplicationQuitListener quitListener)
            {
                _applicationQuitListeners.Add(quitListener);
            }
        }
        
        public void RegisterUnityUpdateListener(IUnityUpdateListener listener) =>
            _updateListeners.Add(listener);
        
        public void UnregisterUnityUpdateListener(IUnityUpdateListener listener) =>
            _updateListeners.Remove(listener);
        
        public void RegisterUnityApplicationPauseListener(IUnityApplicationPauseListener listener) =>
            _applicationPauseListeners.Add(listener);
        
        public void UnregisterUnityApplicationPauseListener(IUnityApplicationPauseListener listener) =>
            _applicationPauseListeners.Remove(listener);
    }
}
