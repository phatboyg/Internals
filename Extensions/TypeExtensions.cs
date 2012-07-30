namespace Internals.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Reflection;

    static class TypeExtensions
    {
        static readonly TypeNameFormatter _typeNameFormatter = new TypeNameFormatter();

        public static IEnumerable<PropertyInfo> GetAllProperties(this Type type)
        {
            const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance;

            foreach (PropertyInfo propertyInfo in type.GetProperties(bindingFlags))
            {
                yield return propertyInfo;
            }

            if (type.IsInterface)
            {
                foreach (Type interfaceType in type.GetInterfaces())
                {
                    foreach (PropertyInfo propertyInfo in interfaceType.GetAllProperties())
                    {
                        yield return propertyInfo;
                    }
                }
            }
        }

        public static IEnumerable<PropertyInfo> GetAllStaticProperties(this Type type)
        {
            const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy;

            foreach (PropertyInfo propertyInfo in type.GetProperties(bindingFlags))
            {
                yield return propertyInfo;
            }
        }

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