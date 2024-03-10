using System;
using System.Collections.Generic;

namespace Core.Ui
{
    internal class WindowsScopeContainer
    {
        public event Action<WindowBase> Changed;
        
        private readonly Stack<ScopeWindowData> _scope = new Stack<ScopeWindowData>();
        private readonly WindowsSystem _windowsSystem;

        private WindowBase _topmost;
        
        public WindowBase Topmost => _topmost;

        internal WindowsScopeContainer(WindowsSystem windowsSystem)
        {
            _windowsSystem = windowsSystem;
        }

        internal T ShowWindow<T>(object data = null, bool withScopeDrop = false) where T : WindowBase
        {
            var windowType = typeof(T);
            if (!typeof(PopUp).IsAssignableFrom(windowType))
            {
                if (_topmost != null)
                {
                    _windowsSystem.ReleaseWindow(_topmost);
                }
                
                if (withScopeDrop)
                {
                    while (_scope.Count > 0)
                    {
                        _scope.Pop();
                    }
                }
            }
            
            var window = _windowsSystem.GetOrCreateWindow(windowType);
            _topmost = window;
            Show(window, data);
            
            _scope.Push(new ScopeWindowData(windowType, data));
            
            return (T) window;
        }

        internal bool TryCloseTopmost()
        {
            if (_scope.Count > 0)
            {
                var topmostWindow = _topmost;
                _topmost = null;
                _windowsSystem.ReleaseWindow(topmostWindow);
                _scope.Pop();
                
                if (_scope.Count > 0)
                {
                    var windowData = _scope.Peek();
                    var window = _windowsSystem.GetOrCreateWindow(windowData.WindowType);
                    Show(window, windowData.Data);
                    _topmost = window;
                }

                return true;
            }

            return false;
        }

        internal void ResetScope()
        {
            if (_scope.Count < 1)
            {
                _topmost = null;
                return;
            }
                
            var topmostWindow = _topmost;
            _windowsSystem.ReleaseWindow(topmostWindow);
            _scope.Pop();
            
            while (_scope.Count > 0)
            {
                _scope.Pop();
            }

            _topmost = null;
        }

        private void Show(WindowBase window, object data)
        {
            window.Initialize(data);
            Changed?.Invoke(window);
            window.gameObject.SetActive(true);
        }
        
        private readonly struct ScopeWindowData
        {
            public readonly Type WindowType;
            public readonly object Data;

            public ScopeWindowData(Type windowType, object data)
            {
                WindowType = windowType;
                Data = data;
            }
        }
    }
}