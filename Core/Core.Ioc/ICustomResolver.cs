using System;

namespace Core.Ioc
{
    public interface ICustomResolver
    {
        object Resolve(Type type, IContainer container);
    }
}