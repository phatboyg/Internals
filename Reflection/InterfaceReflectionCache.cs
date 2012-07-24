namespace Internals.Reflection
{
    using System;
    using Caching;

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
            if (interfaceType.IsGenericTypeDefinition)
                return GetGenericInterface(type, interfaceType);

            Type[] interfaces = type.GetInterfaces();
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
            if (!interfaceType.IsGenericTypeDefinition)
                throw new ArgumentException(
                    "The interface must be a generic interface definition: " + interfaceType.Name,
                    "interfaceType");

            // our contract states that we will not return generic interface definitions without generic type arguments
            if (type == interfaceType)
                return null;

            if (type.IsGenericType)
            {
                if (type.GetGenericTypeDefinition() == interfaceType)
                {
                    return type;
                }
            }

            Type[] interfaces = type.GetInterfaces();
            for (int i = 0; i < interfaces.Length; i++)
            {
                if (interfaces[i].IsGenericType)
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