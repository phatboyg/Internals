namespace Internals.Mapping
{
    using System.Collections.Generic;


    interface ObjectMapper<in T>
    {
        void ApplyTo(T obj, IDictionary<string, object> dictionary);
    }
}