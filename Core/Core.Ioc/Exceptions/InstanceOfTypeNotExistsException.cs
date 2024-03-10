using System;
using Core.Logger;

namespace Core.Ioc.Exceptions
{
    public class InstanceOfTypeNotExistsException: ClientException
    {
        public InstanceOfTypeNotExistsException(Type instanceType):
            base($"{instanceType} already NOT exists in {nameof(Ioc)}'s container")
        {
        }
    }
}