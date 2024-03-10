using System;
using System.Reflection;
using Core.Logger;

namespace Core.Ioc.Exceptions
{
    internal class ConstructorParametersUnsupportedException: ClientException
    {
        public ConstructorParametersUnsupportedException(Type parameterType, ConstructorInfo constructorInfo)
            : base(
        $"Parameter type {parameterType.Name} is not supported in constructor {constructorInfo.ReflectedType}"
            )
        {
        }
    }
}
