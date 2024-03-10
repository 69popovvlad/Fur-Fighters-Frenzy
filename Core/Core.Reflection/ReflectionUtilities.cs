using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using Core.Collections.Utilities;

namespace Core.Reflection
{
    public enum AssembliesType
    {
        All,
        My // Without Unity and System libraries
    }
    
    public static class ReflectionUtilities
    {
        private static readonly Dictionary<Assembly, string[]> _assembliesDependencies = new Dictionary<Assembly, string[]>();
        private static readonly Dictionary<Assembly, string[]> _myAssembliesDependencies = new Dictionary<Assembly, string[]>();
        private static readonly ManualResetEventSlim _eventSlim = new ManualResetEventSlim(false);
        
        private static int _assembliesCollected;

        public static IEnumerable<Type> AvailableTypes(AssembliesType assembliesType = AssembliesType.All, Type filterType = null, bool allTypes = false)
        {
            var assemblies = filterType == null
                ? UsedAssemblies(assembliesType)
                : UsedAssemblies(assembliesType, filterType);
            
            foreach (var assembly in assemblies)
            {
                Type[] assemblyTypes;
                try
                {
                    assemblyTypes = allTypes ? assembly.GetTypes() : assembly.GetExportedTypes();
                }
                catch (Exception)
                {
                    continue;
                }

                for (var i = 0; i < assemblyTypes.Length; ++i)
                {
                    yield return assemblyTypes[i];
                }
            }
        }

        public static Dictionary<string, Type> GetImplementationsOf<TInterface>(
            AssembliesType assembliesType = AssembliesType.All)
        {
            var interfaceType = typeof(TInterface);
            if (!interfaceType.IsInterface)
            {
                throw new NotSupportedException("Support only Interfaces");
            }

            var result = new Dictionary<string, Type>();

            foreach (var type in AvailableTypes(assembliesType, interfaceType))
            {
                if (type.GetInterface(interfaceType.Name) != null)
                {
                    result[type.Name] = type;
                }
            }

            return result;
        }

        public static IEnumerable<FieldInfo> GetFields(Type type, BindingFlags bindingFlags)
        {
            var fieldInfos = new List<FieldInfo>();

            fieldInfos.AddRange(type.GetFields(bindingFlags));
            
            if (type.BaseType != null)
            {
                fieldInfos.AddRange(GetFields(type.BaseType, bindingFlags));
            }
            
            return fieldInfos;
        }
        
        public static IEnumerable<PropertyInfo> GetProperties(Type type, BindingFlags bindingFlags)
        {
            var propertyInfos = new List<PropertyInfo>();

            propertyInfos.AddRange(type.GetProperties(bindingFlags));
            
            if (type.BaseType != null)
            {
                propertyInfos.AddRange(GetProperties(type.BaseType, bindingFlags));
            }
            
            return propertyInfos;
        }
        
        public static IEnumerable<MethodInfo> GetMethods(Type type, BindingFlags bindingFlags, Type attributeType = null)
        {
            var methodInfos = new List<MethodInfo>();

            methodInfos.AddRange(Array.FindAll(type.GetMethods(bindingFlags),
                info => attributeType == null || info.GetCustomAttributes(attributeType, false).Length > 0)
            );
            
            if (type.BaseType != null)
            {
                methodInfos.AddRange(GetMethods(type.BaseType, bindingFlags, attributeType));
            }
            
            return methodInfos;
        }
        
        public static IEnumerable<MemberInfo> GetMembers(Type type, BindingFlags bindingFlags, Type attributeType = null)
        {
            var memberInfos = new List<MemberInfo>();

            memberInfos.AddRange(Array.FindAll(type.GetMembers(bindingFlags),
                info => attributeType == null || info.GetCustomAttributes(attributeType, false).Length > 0)
            );
            
            if (type.BaseType != null)
            {
                memberInfos.AddRange(GetMembers(type.BaseType, bindingFlags, attributeType));
            }
            
            return memberInfos;
        }

        public static bool TryGetMemberInfo(string memberName, Type type, BindingFlags bindingFlags, out MemberInfo memberInfo)
        {
            foreach (var member in GetMembers(type, bindingFlags))
            {
                if (member.Name.Equals(memberName))
                {
                    memberInfo = member;
                    return true;
                }
            }

            memberInfo = default;
            return false;
        }

        public static Dictionary<string, Type> GetSubclassesOf<TBaseClass>(
            AssembliesType assembliesType = AssembliesType.All) where TBaseClass: class
        {
            var baseClassType = typeof(TBaseClass);
            var result = new Dictionary<string, Type>();

            foreach (var type in AvailableTypes(assembliesType, baseClassType))
            {
                if (baseClassType.IsAssignableFrom(type) && baseClassType != type)
                {
                    result[type.Name] = type;
                }
            }

            return result;
            
        }

        public static IEnumerable<(Type type, T[] attributes)> ClassesWithAttributeType<T>
            (AssembliesType assembliesType = AssembliesType.All, Type filterType = null, bool allTypes = false) 
            where T: Attribute
        {
            var attributeType = typeof(T);
            
            foreach (var type in AvailableTypes(assembliesType, filterType, allTypes))
            {
                var attributes = type.GetCustomAttributes(attributeType, true);

                if (attributes.Length == 0)
                {
                    // TODO [Validosik]: add TraceLog here
                    continue;
                }

                yield return (type, attributes as T[]);   
            }
        }
        
        public static IEnumerable<(Type type, MethodInfo methodInfo, T[] attributes)> MethodsWithAttributeType<T>
        (AssembliesType assembliesType = AssembliesType.All, Type filterType = null, bool allTypes = false, BindingFlags bindingFlags = BindingFlags.Public) 
            where T: Attribute
        {
            var attributeType = typeof(T);
            
            foreach (var type in AvailableTypes(assembliesType, filterType, allTypes))
            {
                foreach (var methodInfo in GetMethods(type, bindingFlags, attributeType))
                {
                    var attributes = methodInfo.GetCustomAttributes(attributeType, true);

                    if (attributes.Length == 0)
                    {
                        // TODO [Validosik]: add TraceLog here
                        continue;
                    }

                    yield return (type, methodInfo, attributes as T[]);   
                }
            }
        }
        
        private static IEnumerable<Assembly> UsedAssemblies(AssembliesType assembliesType)
        {
            GatherAssembliesDependencies();

            var assemblies = new Dictionary<Assembly, string[]>();
            switch (assembliesType)
            {
                case AssembliesType.All:
                    assemblies = _assembliesDependencies;
                    break;
                case AssembliesType.My:
                    assemblies = _myAssembliesDependencies;
                    break;
            }
            
            foreach (var (key, _) in assemblies)
            {
                yield return key;
            }
        }

        private static IEnumerable<Assembly> UsedAssemblies(AssembliesType assembliesType, Type filterType)
        {
            GatherAssembliesDependencies();
            
            var assembly = filterType.Assembly;
            var assemblyName = assembly.GetName().Name;


            yield return assembly;

            var assemblies = new Dictionary<Assembly, string[]>();
            switch (assembliesType)
            {
                case AssembliesType.All:
                    assemblies = _assembliesDependencies;
                    break;
                case AssembliesType.My:
                    assemblies = _myAssembliesDependencies;
                    break;
            }
            
            foreach (var (key, value) in assemblies)
            {
                for (var i = 0; i < value.Length; ++i)
                {
                    if (value[i] != assemblyName)
                    {
                        continue;
                    }

                    yield return key;
                    break;
                }
            }
        }
        
        private static void GatherAssembliesDependencies()
        {
            if (_eventSlim.IsSet)
            {
                return;
            }
            
            if (Interlocked.CompareExchange(ref _assembliesCollected, 1, 0) != 0)
            {
                _eventSlim.Wait();
                return;
            }

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                var referencedAssemblies = assembly.GetReferencedAssemblies();
                var ilen = referencedAssemblies.Length;
                var referencedAssembliesNames = new string[ilen];

                for (var i = 0; i < ilen; ++i)
                {
                    referencedAssembliesNames[i] = referencedAssemblies[i].Name;
                }

                _assembliesDependencies[assembly] = referencedAssembliesNames;
                
                if (assembly.FullName.Contains("Core.") || assembly.FullName.Contains("Assembly-CSharp,") ||
                    assembly.FullName.Contains("Assembly-CSharp-Editor,"))
                {
                    _myAssembliesDependencies[assembly] = referencedAssembliesNames;
                }
            }

            _eventSlim.Set();
        }
    }
}
