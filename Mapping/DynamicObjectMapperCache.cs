namespace Internals.Mapping
{
    using System;
    using Reflection;


    class DynamicObjectMapperCache :
        ObjectMapperCache
    {
        readonly ObjectConverterCache _dtoCache;
        readonly ImplementationBuilder _implementationBuilder;
        readonly DictionaryConverterCache _otdCache;

        public DynamicObjectMapperCache()
        {
            _implementationBuilder = new DynamicImplementationBuilder();
            _dtoCache = new DynamicObjectConverterCache(_implementationBuilder);
            _otdCache = new DictionaryConverterCache();
        }

        public ObjectConverter GetObjectConverter(Type type)
        {
            return _dtoCache.GetConverter(type);
        }

        public DictionaryConverter GetDictionaryConverter(Type type)
        {
            return _otdCache.GetConverter(type);
        }
    }
}