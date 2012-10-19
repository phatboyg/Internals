namespace Internals.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Reflection;

    static class TypeExtensions
    {
        static readonly TypeNameFormatter _typeNameFormatter = new TypeNameFormatter();

#if !NETFX_CORE
        public static IEnumerable<PropertyInfo> GetAllProperties(this Type type)
        {
            const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance;

            PropertyInfo[] properties = type.GetProperties(bindingFlags);
            IEnumerable<PropertyInfo> properties = type.GetTypeInfo().DeclaredProperties;
            if (type.IsInterface)
            {
                return properties.Concat(type.GetInterfaces().SelectMany(x => x.GetProperties(bindingFlags)));
            }

            return properties;
        }
#else
        public static IEnumerable<PropertyInfo> GetAllProperties(this Type type)
        {
            //BindingFlags.Public | BindingFlags.Instance;

            var typeInfo = type.GetTypeInfo();

            var properties = typeInfo.DeclaredProperties;
            if (typeInfo.IsInterface)
            {
                return properties.Concat(typeInfo.ImplementedInterfaces.SelectMany(x => x.GetTypeInfo().DeclaredProperties));
            }

            return properties;
        }
#endif

#if !NETFX_CORE
        public static IEnumerable<PropertyInfo> GetAllStaticProperties(this Type type)
        {
            const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy;

            return type.GetProperties(bindingFlags);
        }
#else
        public static IEnumerable<PropertyInfo> GetAllStaticProperties(this Type type)
        {
            return type.GetMethods();
        }
#endif
        /// <summary>
        /// Determines if a type is neither abstract nor an interface and can be constructed.
        /// </summary>
        /// <param name="type">The type to check</param>
        /// <returns>True if the type can be constructed, otherwise false.</returns>
        public static bool IsConcreteType(this Type type)
        {
            return !type.IsAbstract && !type.IsInterface;
        }

        /// <summary>
        /// Determines if a type can be constructed, and if it can, additionally determines
        /// if the type can be assigned to the specified type.
        /// </summary>
        /// <param name="type">The type to evaluate</param>
        /// <param name="assignableType">The type to which the subject type should be checked against</param>
        /// <returns>True if the type is concrete and can be assigned to the assignableType, otherwise false.</returns>
        public static bool IsConcreteAndAssignableTo(this Type type, Type assignableType)
        {
            return IsConcreteType(type) && assignableType.IsAssignableFrom(type);
        }

        /// <summary>
        /// Determines if a type can be constructed, and if it can, additionally determines
        /// if the type can be assigned to the specified type.
        /// </summary>
        /// <param name="type">The type to evaluate</param>
        /// <typeparam name="T">The type to which the subject type should be checked against</typeparam>
        /// <returns>True if the type is concrete and can be assigned to the assignableType, otherwise false.</returns>
        public static bool IsConcreteAndAssignableTo<T>(this Type type)
        {
            return IsConcreteType(type) && typeof(T).IsAssignableFrom(type);
        }

        /// <summary>
        /// Determines if the type is an open generic with at least one unspecified generic argument
        /// </summary>
        /// <param name="type">The type</param>
        /// <returns>True if the type is an open generic</returns>
        public static bool IsOpenGeneric(this Type type)
        {
            return type.IsGenericTypeDefinition || type.ContainsGenericParameters;
        }

        public static string GetTypeName(this Type type)
        {
            return _typeNameFormatter.GetTypeName(type);
        }
    }
}