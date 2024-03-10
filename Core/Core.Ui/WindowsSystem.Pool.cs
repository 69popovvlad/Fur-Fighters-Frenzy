using System;
using System.Collections.Generic;
using Core.Collections.Utilities;
using UnityEngine;

namespace Core.Ui
{
    public partial class WindowsSystem
    {
        [SerializeField] private float _poolingDuration = 20;
        
        private readonly Dictionary<Type, WindowData> _pool = new Dictionary<Type, WindowData>();
        private readonly List<Type> _forRemoving = new List<Type>();
        
        private Transform _poolRoot;

        private void CreatePool()
        {
            _poolRoot = new GameObject("WindowsPool").transform;
            {
                _poolRoot.SetParent(transform);
                _poolRoot.gameObject.SetActive(false);
            }
        }
        
        private void UpdatePool()
        {
            foreach (var (windowType, windowData) in _pool)
            {
                windowData.PoolingDuration += Time.deltaTime;
                
                if (windowData.PoolingDuration > _poolingDuration)
                {
                    _forRemoving.Add(windowType);
                }
            }

            for (var i = _forRemoving.Count - 1; i >= 0; --i)
            {
                var windowType = _forRemoving[i];
                _forRemoving.RemoveAt(i);
                var windowData = _pool[windowType];
                _pool.Remove(windowType);
                Destroy(windowData.Window.gameObject);
            }
        }
        
        internal WindowBase GetOrCreateWindow(Type windowType)
        {
            WindowBase instance; 
            if (!_pool.ContainsKey(windowType))
            {
                instance = CreateWindow(windowType);
            }
            else
            {
                instance = _pool[windowType].Window;
                _pool.Remove(windowType);
            }

            instance.gameObject.SetActive(false);
            instance.transform.SetParent(transform);
            return instance;
        }

        internal void ReleaseWindow(WindowBase window)
        {
            if (window == null)
            {
                return;
            }
            
            var windowType = window.GetType();
            if (!_pool.TryGetValue(windowType, out _))
            {
                _pool[windowType] = new WindowData(window);
                window.transform.SetParent(_poolRoot);
            }
            else
            {
                Destroy(window.gameObject);
            }
        }

        private WindowBase CreateWindow(Type windowType)
        {
            if (!_windowsLookup.TryGetValue(windowType, out var windowPrefab))
            {
                throw new Exception($"{windowType} couldn't found in {nameof(_windowsLookup)}\n" +
                                    $"Check {nameof(WindowsLookupConfig)} and add window you need to it");
            }
            
            var lastActive = windowPrefab.gameObject.activeSelf;
            windowPrefab.gameObject.SetActive(false); // For exclude situation with early calling of OnEnable 
            
            var window = Instantiate(windowPrefab, transform);
            _ioc.InjectDependencies(window);
            
            windowPrefab.gameObject.SetActive(lastActive);
            return window;
        }
        
        private class WindowData
        {
            public readonly WindowBase Window;
            public float PoolingDuration;

            public WindowData(WindowBase window)
            {
                Window = window;
                PoolingDuration = 0;
            }
        }
    }
}