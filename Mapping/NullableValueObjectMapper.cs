namespace Internals.Mapping
{
    using System.Collections.Generic;
    using Reflection;


    class NullableValueObjectMapper<T, TValue> :
        ObjectMapper<T>
        where TValue : struct
    {
        readonly ReadWriteProperty<T> _property;

        public NullableValueObjectMapper(ReadWriteProperty<T> property)
        {
            _property = property;
        }

        public void ApplyTo(T obj, IDictionary<string, object> dictionary)
        {
            object value;
            if (dictionary.TryGetValue(_property.Property.Name, out value))
            {
                var nullableValue = (TValue?)value;
                _property.Set(obj, nullableValue);
            }
        }
    }
}