namespace Internals.Mapping
{
    using System.Collections.Generic;
    using Reflection;


    class NullableValueDictionaryMapper<T, TValue> :
        DictionaryMapper<T>
        where TValue : struct
    {
        readonly ReadOnlyProperty<T> _property;

        public NullableValueDictionaryMapper(ReadOnlyProperty<T> property)
        {
            _property = property;
        }

        public void WritePropertyToDictionary(IDictionary<string, object> dictionary, T obj)
        {
            object value = _property.Get(obj);
            if (value == null)
                return;

            var nullableValue = (TValue?)value;
            dictionary.Add(_property.Property.Name, nullableValue.Value);
        }
    }
}