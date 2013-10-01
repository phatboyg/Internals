namespace Internals.Mapping
{
    using System.Collections.Generic;


    static class ObjectConverterExtensions
    {
        internal static object GetObject(this ObjectConverter converter, IDictionary<string, object> dictionary)
        {
            return converter.GetObject(new DictionaryObjectValueProvider(dictionary));
        }
    }
}