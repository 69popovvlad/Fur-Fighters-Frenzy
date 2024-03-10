using System;

namespace Core.Application.Environment
{
    public static class Environment
    {
        private static IEnvironment _env;

        public static void Init(IEnvironment env)
        {
            CurrentEnvironment = env;
        }
        
        public static IEnvironment CurrentEnvironment
        {
            get
            {
                if (_env == null)
                {
                    throw new InitializationException();
                }

                return _env;
            }
            set => _env = value;
        }

        private sealed class InitializationException : Exception
        {
            public InitializationException()
                : base($"Make sure {nameof(Environment)}.{nameof(Init)} is called.")
            {
                /* Nothing to do */
            }
        }
    }
}