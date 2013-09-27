namespace Internals.Mapping
{
    using System;


    interface ObjectConverterCache
    {
        ObjectConverter GetConverter(Type type);
    }
}