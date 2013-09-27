namespace Internals.Mapping
{
    using System.Collections.Generic;


    interface DictionaryConverter
    {
        IDictionary<string, object> GetDictionary(object obj);
    }
}