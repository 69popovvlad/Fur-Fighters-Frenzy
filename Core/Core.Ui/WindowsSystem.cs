using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Ui
{
    public partial class WindowsSystem: MonoBehaviour
    {
        public event Action<WindowBase> Changed;

        public static readonly string ResourcesDirectory = "Configs";
        public static readonly string ConfigDirectory = $"Assets/Resources/{ResourcesDirectory}";
        
        private readonly WindowsScopeContainer[] _scopes = new WindowsScopeContainer[2];
        private readonly Dictionary<Type, WindowBase> _windowsLookup = new Dictionary<Type, WindowBase>();

        private Ioc.Ioc _ioc;
        
        public WindowBase TopmostWindow => _scopes[1].Topmost != null ? _scopes[1].Topmost : _scopes[0].Topmost;
        
        public void Initialize()
        {
            _ioc = Ioc.Ioc.Instance;

            CreatePool();
            
            var asset = Resources.LoadAll<WindowsLookupConfig>(ResourcesDirectory);
            var lookup = asset[0].Lookup;
            
            for (int i = 0, length = lookup.Count; i < length; i++)
            {
                var windowPrefab = lookup[i];
                var windowType = windowPrefab.GetType();
                if (_windowsLookup.ContainsKey(windowType))
                {
                    throw new Exception($"{windowType} already exists!");
                }
                
                _windowsLookup.Add(windowType, windowPrefab);
            }
            
            _scopes[0] = new WindowsScopeContainer(this);
            _scopes[1] = new WindowsScopeContainer(this);
            
            _scopes[0].Changed += OnWindowChanged;
            _scopes[1].Changed += OnWindowChanged;
        }
        
        public T ShowWindow<T>(object data = null, bool withScopeDrop = false) where T : WindowBase
        {
            var windowType = typeof(T);
            var scope = default(WindowsScopeContainer);
            if (typeof(PopUp).IsAssignableFrom(windowType))
            { 
                scope = _scopes[1];
            }
            else
            {
                scope = _scopes[0];
                if (withScopeDrop)
                {
                    _scopes[0].ResetScope();
                    _scopes[1].ResetScope();
                }
            }
            
            var window = scope.ShowWindow<T>(data, withScopeDrop);
            return window;
        }

        public void CloseTopmost()
        {
            if (TopmostWindow == null)
            {
                return;
            }
            
            var topmostWindow = TopmostWindow;
            if (topmostWindow is PopUp)
            {
                var scope = _scopes[1];
                if (scope.TryCloseTopmost())
                {
                    Changed?.Invoke(TopmostWindow);
                    return;
                }
            }
            
            var mainScope = _scopes[0];
            if (mainScope.TryCloseTopmost())
            {
                Changed?.Invoke(TopmostWindow);
            }
            else
            {
                throw new Exception($"Check windows scope cause {topmostWindow} was last window");
            }
        }

        private void Update()
        {
            UpdatePool();
        }

        private void OnDestroy()
        {
            _scopes[0].Changed -= OnWindowChanged;
            _scopes[1].Changed -= OnWindowChanged;
            
            Destroy(_poolRoot);
        }
        
        private void OnWindowChanged(WindowBase window)
        {
            Changed?.Invoke(window);
        }
    }
}