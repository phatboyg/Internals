namespace Internals.Mapping
{
    using System;
    using Caching;


    /// <summary>
    /// Caches the type converter instances
    /// </summary>
    class DictionaryConverterCache
    {
        readonly Cache<Type, DictionaryConverter> _cache;

        public DictionaryConverterCache()
        {
            _cache = new GenericTypeCache<DictionaryConverter>(typeof(ObjectDictionaryConverter<>),
                CreateMissingConverter);
        }

        DictionaryConverter CreateMissingConverter(Type key)
        {
            Type type = typeof(ObjectDictionaryConverter<>).MakeGenericType(key);

            return (DictionaryConverter)Activator.CreateInstance(type, this);
        }

        public DictionaryConverter GetConverter(Type type)
        {
            return _cache[type];
        }
    }
}