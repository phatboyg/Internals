namespace Internals.Mapping
{
    using System;
    using Caching;
    using Reflection;


    /// <summary>
    /// Caches dictionary to object converters for the types requested, including the implementation
    /// builder for interfaces that are dynamically proxied
    /// </summary>
    class DynamicObjectConverterCache :
        ObjectConverterCache
    {
        readonly Cache<Type, ObjectConverter> _cache;
        readonly ImplementationBuilder _implementationBuilder;

        public DynamicObjectConverterCache(ImplementationBuilder implementationBuilder)
        {
            _implementationBuilder = implementationBuilder;
            _cache = new ConcurrentCache<Type, ObjectConverter>(CreateMissingConverter);
        }

        public ObjectConverter GetConverter(Type type)
        {
            return _cache[type];
        }

        ObjectConverter CreateMissingConverter(Type type)
        {
            Type implementationType = _implementationBuilder.GetImplementationType(type);
            Type converterType = typeof(DynamicObjectConverter<,>).MakeGenericType(type, implementationType);

            return (ObjectConverter)Activator.CreateInstance(converterType, this);
        }
    }
}