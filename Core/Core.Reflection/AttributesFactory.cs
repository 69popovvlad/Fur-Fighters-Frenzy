using System;
using System.Collections.Generic;

namespace Core.Reflection
{
    public static class AttributesFactory<TObjectType, TAttributeType, TAttributeKey>  
        where TObjectType: class
        where TAttributeType: Attribute, IKeyableAttribute<TAttributeKey>
    {
        public static readonly Dictionary<TAttributeKey, Type> ObjectTypeByAttribute = new Dictionary<TAttributeKey, Type>();
        public static readonly Dictionary<Type, TAttributeType> AttributeByObjectType = new Dictionary<Type, TAttributeType>();
        public static readonly Dictionary<Type, TAttributeKey> KeyByObjectType = new Dictionary<Type, TAttributeKey>();

        static AttributesFactory()
        {
            foreach (var (type, attributes) in
                ReflectionUtilities.ClassesWithAttributeType<TAttributeType>(AssembliesType.My, typeof(TObjectType)))
            {
                if (type.IsAbstract)
                {
                    continue;
                }

                var attribute = (IKeyableAttribute<TAttributeKey>) attributes[0];
                ObjectTypeByAttribute[attribute.Key] = type;
                AttributeByObjectType[type] = (TAttributeType) attributes[0];
                KeyByObjectType[type] = attribute.Key;
            }
        }
    }
}
