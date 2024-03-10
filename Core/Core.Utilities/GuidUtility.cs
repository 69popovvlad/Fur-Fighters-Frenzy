using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Core.Reflection;

namespace Core.Utilities
{
    public static class GuidUtility<TType>
    {
        private static readonly Dictionary<string, Type> _typeByGuid = new Dictionary<string, Type>();
        private static readonly Dictionary<Type, string> _guidByType = new Dictionary<Type, string>();
        
        static GuidUtility()
        {
            foreach (var (type, attributes) in ReflectionUtilities.ClassesWithAttributeType<GuidAttribute>(AssembliesType.My,
                typeof(TType)))
            {
                var attribute = attributes[0];
                if (_typeByGuid.ContainsKey(attribute.Value))
                {
                    throw new GuidExistException(attribute.Value, nameof(_guidByType), typeof(TType));
                }

                _typeByGuid[attribute.Value] = type;
                _guidByType[type] = attribute.Value;
            }
        }

        public static bool TryGetType(string guid, out Type type) =>
            _typeByGuid.TryGetValue(guid, out type);
        
        public static bool TryGetGuid(Type type, out string guid) =>
            _guidByType.TryGetValue(type, out guid);
        
        private class GuidExistException: Exception
        {
            public GuidExistException(string what, string where, Type type): 
                base($"{type}@{what} already exist in {where}")
            {
            }
        }
    }
}
