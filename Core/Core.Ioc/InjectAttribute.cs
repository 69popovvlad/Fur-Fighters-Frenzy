using System;

namespace Core.Ioc
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class InjectAttribute: Attribute
    {
        
    }
}