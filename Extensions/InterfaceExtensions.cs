namespace Internals.Extensions
{
    using System;
    using System.Collections.Generic;
    using Reflection;

    static class InterfaceExtensions
    {
        static readonly InterfaceReflectionCache _cache;

        static InterfaceExtensions()
        {
            _cache = new InterfaceReflectionCache();
        }

        public static bool HasInterface<T>(this object obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");
            Type type = obj.GetType();

            return HasInterface(type, typeof(T));
        }

        public static bool HasInterface<T>(this Type type)
        {
            return HasInterface(type, typeof(T));
        }

        public static bool HasInterface(this Type type, Type interfaceType)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            if (interfaceType == null)
                throw new ArgumentNullException("interfaceType");
            if (!interfaceType.IsInterface)
                throw new ArgumentException("The interface type must be an interface: " + interfaceType.Name);

            if (interfaceType.IsGenericTypeDefinition)
                return _cache.GetGenericInterface(type, interfaceType) != null;

            return interfaceType.IsAssignableFrom(type);
        }

        public static bool HasInterface(this object obj, Type interfaceType)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");
            Type type = obj.GetType();

            return HasInterface(type, interfaceType);
        }

        public static Type GetInterface<T>(this object obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");
            Type type = obj.GetType();

            return GetInterface(type, typeof(T));
        }

        public static Type GetInterface<T>(this Type type)
        {
            return GetInterface(type, typeof(T));
        }

        public static Type GetInterface(this Type type, Type interfaceType)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            if (interfaceType == null)
                throw new ArgumentNullException("interfaceType");
            if (!interfaceType.IsInterface)
                throw new ArgumentException("The interface type must be an interface: " + interfaceType.Name);

            return _cache.Get(type, interfaceType);
        }

        public static bool ClosesType(this Type type, Type openType)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            if (openType == null)
                throw new ArgumentNullException("openType");

            if (!openType.IsOpenGeneric())
                throw new ArgumentException("The interface type must be an open generic interface: " + openType.Name);

            if (openType.IsInterface)
            {
                if (!openType.IsOpenGeneric())
                    throw new ArgumentException("The interface type must be an open generic interface: " + openType.Name);

                Type interfaceType = type.GetInterface(openType);
                if (interfaceType == null)
                    return false;

                return !interfaceType.IsGenericTypeDefinition;
            }


            Type baseType = type.BaseType;
            while (baseType != null && baseType != typeof(object))
            {
                if (baseType.IsGenericType && baseType.GetGenericTypeDefinition() == openType)
                    return !baseType.IsGenericTypeDefinition;

                if (!baseType.IsGenericType && baseType == openType)
                    return true;

                baseType = baseType.BaseType;
            }

            return false;
        }

        public static IEnumerable<Type> GetClosingArguments(this Type type, Type openType)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            if (openType == null)
                throw new ArgumentNullException("openType");

            if (!openType.IsOpenGeneric())
                throw new ArgumentException("The interface type must be an open generic interface: " + openType.Name);

            if (openType.IsInterface)
            {
                if (!openType.IsOpenGeneric())
                    throw new ArgumentException("The interface type must be an open generic interface: " + openType.Name);

                Type interfaceType = type.GetInterface(openType);
                if (interfaceType == null)
                    throw new ArgumentException("The interface type is not implemented by: " + type.Name);

                return interfaceType.GetGenericArguments();
            }


            Type baseType = type;
            while (baseType != null && baseType != typeof(object))
            {
                if (baseType.IsGenericType && baseType.GetGenericTypeDefinition() == openType)
                {
                    return baseType.GetGenericArguments();
                }

                if (!baseType.IsGenericType && baseType == openType)
                    return baseType.GetGenericArguments();

                baseType = baseType.BaseType;
            }

//                foreach (Type declaredType in interfaceType.GetGenericArguments())
//                {
//                    if (declaredType.IsGenericParameter)
//                        continue;
//
//                    yield return declaredType;
//                }

            throw new ArgumentException("Could not find open type in type: " + type.Name);
        }

        public static IEnumerable<Type> GetClosingArguments(this object obj, Type openType)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            Type objectType = obj.GetType();

            return GetClosingArguments(objectType, openType);
        }


        public static bool IsConcreteAndAssignableTo(this Type pluggedType, Type pluginType)
        {
            return pluggedType.IsConcreteType() && pluginType.IsAssignableFrom(pluggedType);
        }

        public static bool ImplementsOpenGenericInterface(this Type type, Type interfaceType)
        {
            if (!type.IsConcreteType())
                return false;

            Type[] interfaces = type.GetInterfaces();
            for (int i = 0; i < interfaces.Length; i++)
            {
                if (interfaces[i].IsGenericType
                    && interfaces[i].GetGenericTypeDefinition() == interfaceType)
                    return true;
            }

            return false;
        }
    }
}