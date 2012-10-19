namespace Internals.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Reflection;
    using System.Reflection;


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
#if !NETFX_CORE
            if (!interfaceType.IsInterface)
#else
            var interfaceTypeInfo = interfaceType.GetTypeInfo();
            if (!interfaceTypeInfo.IsInterface)
#endif
                throw new ArgumentException("The interface type must be an interface: " + interfaceType.Name);

#if !NETFX_CORE
            if (interfaceType.IsGenericTypeDefinition)
#else
            if (interfaceTypeInfo.IsGenericTypeDefinition)
#endif
            return _cache.GetGenericInterface(type, interfaceType) != null;

#if !NETFX_CORE
            return interfaceType.IsAssignableFrom(type);
#else
            return interfaceTypeInfo.IsAssignableFrom(type.GetTypeInfo());
#endif
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
#if !NETFX_CORE
            if (!interfaceType.IsInterface)
#else
            var interfaceTypeInfo = interfaceType.GetTypeInfo();
            if (!interfaceTypeInfo.IsInterface)
#endif
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
#if !NETFX_CORE
            if (openType.IsInterface)
#else
            if (openType.GetTypeInfo().IsInterface)
#endif
            {
                if (!openType.IsOpenGeneric())
                    throw new ArgumentException("The interface type must be an open generic interface: " + openType.Name);

                Type interfaceType = type.GetInterface(openType);
                if (interfaceType == null)
                    return false;
#if !NETFX_CORE
                return !interfaceType.IsGenericTypeDefinition && !interfaceType.ContainsGenericParameters;
#else
                var typeInfo = interfaceType.GetTypeInfo();
                return !typeInfo.IsGenericTypeDefinition && !typeInfo.ContainsGenericParameters;
#endif
            }

            Type baseType = type;
            while (baseType != null && baseType != typeof(object))
            {
#if !NETFX_CORE
                if (baseType.IsGenericType && baseType.GetGenericTypeDefinition() == openType)
                    return !baseType.IsGenericTypeDefinition && !baseType.ContainsGenericParameters;

                if (!baseType.IsGenericType && baseType == openType)
                    return true;

                baseType = baseType.BaseType;
#else
                var baseTypeInfo = baseType.GetTypeInfo();
                if (baseTypeInfo.IsGenericType && baseTypeInfo.GetGenericTypeDefinition() == openType)
                    return !baseTypeInfo.IsGenericTypeDefinition && !baseTypeInfo.ContainsGenericParameters;
#endif
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
#if !NETFX_CORE
            if (openType.IsInterface)
#else
            if (openType.GetTypeInfo().IsInterface)
#endif
            {
                if (!openType.IsOpenGeneric())
                    throw new ArgumentException("The interface type must be an open generic interface: " + openType.Name);

                Type interfaceType = type.GetInterface(openType);
                if (interfaceType == null)
                    throw new ArgumentException("The interface type is not implemented by: " + type.Name);
#if !NETFX_CORE
                return interfaceType.GetGenericArguments().Where(x => !x.IsGenericParameter);
#else
                return interfaceType.GetTypeInfo().GenericTypeArguments.Where(x => !x.IsGenericParameter);
#endif
            }

            Type baseType = type;
            while (baseType != null && baseType != typeof(object))
            {
#if !NETFX_CORE
                if (baseType.IsGenericType && baseType.GetGenericTypeDefinition() == openType)
                    return baseType.GetGenericArguments().Where(x => !x.IsGenericParameter);

                if (!baseType.IsGenericType && baseType == openType)
                    return baseType.GetGenericArguments().Where(x => !x.IsGenericParameter);

                baseType = baseType.BaseType;
#else
                var baseTypeInfo = baseType.GetTypeInfo();
                if (baseTypeInfo.IsGenericType && baseType.GetGenericTypeDefinition() == openType)
                    return baseTypeInfo.GenericTypeArguments.Where(x => !x.IsGenericParameter);

                if (!baseTypeInfo.IsGenericType && baseType == openType)
                    return baseTypeInfo.GenericTypeArguments.Where(x => !x.IsGenericParameter);

                baseType = baseTypeInfo.BaseType;
#endif
            }
            
            throw new ArgumentException("Could not find open type in type: " + type.Name);
        }

        public static IEnumerable<Type> GetClosingArguments(this object obj, Type openType)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            Type objectType = obj.GetType();

            return GetClosingArguments(objectType, openType);
        }
    }
}