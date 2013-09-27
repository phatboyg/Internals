namespace Internals.Mapping
{
    using System;


    interface ObjectMapperCache
    {
        ObjectConverter GetObjectConverter(Type type);
        DictionaryConverter GetDictionaryConverter(Type type);
    }
}