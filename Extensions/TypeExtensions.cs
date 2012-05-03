namespace Internals.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    static class TypeExtensions
    {
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

        /// <summary>
        /// Determines if the type can be instantiated
        /// </summary>
        /// <param name="type">The type</param>
        /// <returns>True if the type is concrete, otherwise false</returns>
        public static bool IsConcreteType(this Type type)
        {
            return !type.IsAbstract && !type.IsInterface;
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
    }
}