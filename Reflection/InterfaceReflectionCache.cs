namespace Internals.Reflection
{
    using System;
    using System.Linq;
    using Caching;
    using System.Reflection;


    class InterfaceReflectionCache
    {
        readonly Cache<Type, Cache<Type, Type>> _cache;

        public InterfaceReflectionCache()
        {
#if NET35
            _cache = new ReaderWriterLockedCache<Type, Cache<Type, Type>>(new DictionaryCache<Type, Cache<Type,Type>>(typeKey =>
            {
                MissingValueProvider<Type, Type> missingValueProvider = x => GetInterfaceInternal(typeKey, x);

                return new ReaderWriterLockedCache<Type, Type>(new DictionaryCache<Type,Type>(missingValueProvider));
            }));
#else
            _cache = new ConcurrentCache<Type, Cache<Type, Type>>(typeKey =>
                {
                    MissingValueProvider<Type, Type> missingValueProvider = x => GetInterfaceInternal(typeKey, x);

                    return new ConcurrentCache<Type, Type>(missingValueProvider);
                });
#endif
        }

        Type GetInterfaceInternal(Type type, Type interfaceType)
        {
#if !NETFX_CORE
            if (interfaceType.IsGenericTypeDefinition)
#else
            if (interfaceType.GetTypeInfo().IsGenericTypeDefinition)
#endif
                return GetGenericInterface(type, interfaceType);

#if !NETFX_CORE
            Type[] interfaces = type.GetInterfaces();
#else
            var interfaces = type.GetTypeInfo().ImplementedInterfaces.ToArray();
#endif
            for (int i = 0; i < interfaces.Length; i++)
            {
                if (interfaces[i] == interfaceType)
                {
                    return interfaces[i];
                }
            }

            return null;
        }

        public Type GetGenericInterface(Type type, Type interfaceType)
        {
#if !NETFX_CORE
            if (!interfaceType.IsGenericTypeDefinition)
#else
            if (!interfaceType.GetTypeInfo().IsGenericTypeDefinition)
#endif
                throw new ArgumentException(
                    "The interface must be a generic interface definition: " + interfaceType.Name,
                    "interfaceType");

            // our contract states that we will not return generic interface definitions without generic type arguments
            if (type == interfaceType)
                return null;
#if !NETFX_CORE
            if (type.IsGenericType)
#else
            if (type.GetTypeInfo().IsGenericType)
#endif
            {
                if (type.GetGenericTypeDefinition() == interfaceType)
                {
                    return type;
                }
            }
#if !NETFX_CORE
            Type[] interfaces = type.GetInterfaces();
#else
            Type[] interfaces = type.GetTypeInfo().ImplementedInterfaces.ToArray();
#endif
            for (int i = 0; i < interfaces.Length; i++)
            {
#if !NETFX_CORE
                if (interfaces[i].IsGenericType)
#else
                if(interfaces[i].GetTypeInfo().IsGenericType)
#endif
                {
                    if (interfaces[i].GetGenericTypeDefinition() == interfaceType)
                    {
                        return interfaces[i];
                    }
                }
            }

            return null;
        }

        public Type Get(Type type, Type interfaceType)
        {
            return _cache[type][interfaceType];
        }
    }
}