namespace Internals.Mapping
{
    using System.Collections.Generic;


    interface ObjectConverter
    {
        object GetObject(IDictionary<string, object> dictionary);
    }
}