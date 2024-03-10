using System;
using Core.Logger;

namespace Core.Ioc.Exceptions
{
    internal class DependenciesCycleException: ClientException
    {
        public DependenciesCycleException(object from, Type type)
            : base(
                $"{from.GetType().Name}: dependencies cycle was detected for type {type}"
            )
        {
            
        }
    }
}
