using System;
using Core.Logger;

namespace Core.Ioc.Exceptions
{
    public class InstanceOfTypeExistsException: ClientException
    {
        public InstanceOfTypeExistsException(Type instanceType):
            base($"{instanceType} already exists in {nameof(Ioc)}'s container")
        {
        }
    }
}