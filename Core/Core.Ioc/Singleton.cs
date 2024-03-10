using System;

namespace Core.Ioc
{
    public abstract class Singleton<T> where T : class, IDisposable, new()
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new T();
                }

                return _instance;
            }
        }
    }
}
