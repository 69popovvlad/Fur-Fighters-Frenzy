using System;
using System.Collections.Generic;
using System.Reflection;
using Core.Reflection;

namespace Core.Ioc
{
    internal partial class Container
    {
        private readonly Type _injectAttributeType = typeof(InjectAttribute);
        
        private void InjectDependencies(Type type, ParameterInfo[] constructorsParameters, ref object instance)
        {
            InjectInnerDependencies(ReflectionUtilities.GetFields(type, MemberFlags), instance, 
                memberInfo => Array.Find(constructorsParameters, c => c.ParameterType == memberInfo.FieldType) == null,
                InjectField);
            
            InjectInnerDependencies(ReflectionUtilities.GetProperties(type, MemberFlags), instance, 
                memberInfo => Array.Find(constructorsParameters, c => c.ParameterType == memberInfo.PropertyType) == null,
                InjectProperty);
        }

        private void InjectInnerDependencies<TMember>(IEnumerable<TMember> members, object instance,
            Func<TMember, bool> predicate, Action<TMember, object> injector) where TMember: MemberInfo
        {
            foreach (var memberInfo in members)
            {
                var inject = memberInfo.GetCustomAttributes(_injectAttributeType, false).Length > 0;

                if (inject && predicate(memberInfo))
                {
                    injector(memberInfo, instance);
                }
            }
        }

        private void InjectField(FieldInfo fieldInfo, object instance)
        {
            var fieldType = fieldInfo.FieldType;
            
            // if (!ValidateTypeCanInject(fieldType))
            // {
            //     return;
            // }
            
            if (fieldType.IsArray || fieldInfo.GetValue(instance) != null)
            {
                return;
            }
            
            fieldInfo.SetValue(instance, Get(fieldType));
        }
        
        private void InjectProperty(PropertyInfo propertyInfo, object instance)
        {
            var propertyType = propertyInfo.PropertyType;
            
            // if (!ValidateTypeCanInject(propertyType))
            // {
            //     return;
            // }

            if (!propertyInfo.CanWrite || propertyType.IsArray ||
                propertyInfo.GetIndexParameters().Length > 0 || propertyInfo.GetValue(instance, null) != null)
            {
                return;
            }
            
            propertyInfo.SetValue(instance, Get(propertyType), null);
        }

        private bool ValidateTypeCanInject(Type type)
        {
            return _resolvers.ContainsKey(type);
        }
    }
}
