using System;
using UnityEngine;

namespace Core.Application
{
    public class MonoBehaviourSingleton<T>: MonoBehaviour where T: MonoBehaviour
    {
        private static readonly Type _type = typeof(T);
        private static readonly object _lock = new object();
        
        private static T _instance;

        public static T Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance != null)
                    {
                        return _instance;
                    }

                    _instance = default(T);

                    var instances = FindObjectsOfType(_type);
                    if (instances.Length > 0)
                    {
                        _instance = (T)instances[0];

                        if (instances.Length > 1)
                        {
                            DontDestroyOnLoad(_instance.gameObject);
                            // TODO[Validosik]: Add TraceLog 
                            Debug.LogWarning($"More than one MonoBehaviourSingleton<{_type.Name}>");
                        }
                        else
                        {
                            // TODO[Validosik]: Add TraceLog 
                            Debug.Log($"MonoBehaviourSingleton<{_type.Name}> was created successfully");
                        }
                    }
                    else
                    {
                        _instance = (T)new GameObject(_type.Name, _type).GetComponent(_type);
                        DontDestroyOnLoad(_instance.gameObject);
                        
                        // TODO[Validosik]: Add TraceLog 
                        Debug.Log($"MonoBehaviourSingleton<{_type.Name}> was created successfully");
                    }
                    
                    return _instance;
                }
            }

            protected set => _instance = value;
        }
    }
}