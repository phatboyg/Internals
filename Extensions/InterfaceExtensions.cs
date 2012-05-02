namespace Internals.Extensions
{
    using System;
    using Internals.Caching;

    static class InterfaceExtensions
    {
        static readonly Cache<Type, Cache<Type, Type>> _cache;

        static InterfaceExtensions()
        {
            _cache = new ConcurrentCache<Type, Cache<Type, Type>>(typeKey =>
                {
                    MissingValueProvider<Type, Type> missingValueProvider = x => GetInterfaceInternal(typeKey, x);

                    return new ConcurrentCache<Type, Type>(missingValueProvider);
                });
        }

        public static bool HasInterface<T>(this object obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");
            Type type = obj.GetType();

            return HasInterface(type, typeof (T));
        }

        public static bool HasInterface<T>(this Type type)
        {
            return HasInterface(type, typeof (T));
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
                return GetGenericInterface(type, interfaceType) != null;

            return interfaceType.IsAssignableFrom(type);
        }

        public static Type GetInterface<T>(this object obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");
            Type type = obj.GetType();

            return GetInterface(type, typeof (T));
        }

        public static Type GetInterface<T>(this Type type)
        {
            return GetInterface(type, typeof (T));
        }

        public static Type GetInterface(this Type type, Type interfaceType)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            if (interfaceType == null)
                throw new ArgumentNullException("interfaceType");
            if (!interfaceType.IsInterface)
                throw new ArgumentException("The interface type must be an interface: " + interfaceType.Name);

            return _cache[type][interfaceType];
        }

        static Type GetInterfaceInternal(Type type, Type interfaceType)
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

        static Type GetGenericInterface(this Type type, Type interfaceType)
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

        public static bool Closes(this Type type, Type openType)
        {
            if (!openType.IsGenericType && !openType.IsGenericTypeDefinition)
                return false;

            bool closes = false;

            if (openType.IsInterface)
                closes = type.ImplementsOpenGenericInterface(openType) && type.IsGenericTypeDefinition == false;
            else
            {
                if (type.BaseType != null)
                {
                    if (type.BaseType.IsGenericType)
                        closes = type.BaseType.GetGenericTypeDefinition() == openType;
                    else
                        closes = type.BaseType == openType;
                }
            }
            if (closes)
                return true;
            return type.BaseType == null ? false : type.BaseType.Closes(openType);
        }

        public static bool IsOpenGeneric(this Type type)
        {
            return type.IsGenericTypeDefinition || type.ContainsGenericParameters;
        }

        public static bool IsConcreteAndAssignableTo(this Type pluggedType, Type pluginType)
        {
            return pluggedType.IsConcreteType() && pluginType.IsAssignableFrom(pluggedType);
        }

        public static bool ImplementsOpenGenericInterface(this Type type, Type interfaceType)
        {
            if (!type.IsConcreteType())
                return false;

            for (int i = 0; i < type.GetInterfaces().Length; i++)
            {
                if (type.GetInterfaces()[i].IsGenericType && type.GetInterfaces()[i].GetGenericTypeDefinition() == interfaceType)
                    return true;
            }

            return false;
        }

        public static bool IsConcreteType(this Type type)
        {
            return !type.IsAbstract && !type.IsInterface;
        }
    }
}