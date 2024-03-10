using System;
using Core.Logger;

namespace Core.Ioc.Exceptions
{
    public class ConstructorUndefinedException: ClientException
    {
        public ConstructorUndefinedException(Type type)
            : base($"Constructor wasn't defined for type {type}")
        {
        }
    }
}
