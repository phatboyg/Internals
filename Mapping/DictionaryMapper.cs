namespace Internals.Mapping
{
    using System.Collections.Generic;


    interface DictionaryMapper<in T>
    {
        void WritePropertyToDictionary(IDictionary<string, object> dictionary, T obj);
    }
}