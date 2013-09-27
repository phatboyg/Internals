namespace Internals.Mapping
{
    using System.Collections.Generic;
    using Reflection;


    class ValueObjectMapper<T> :
        ObjectMapper<T>
    {
        readonly ReadWriteProperty<T> _property;

        public ValueObjectMapper(ReadWriteProperty<T> property)
        {
            _property = property;
        }

        public void ApplyTo(T obj, IDictionary<string, object> dictionary)
        {
            object value;
            if (dictionary.TryGetValue(_property.Property.Name, out value))
            {
                if (value != null)
                    _property.Set(obj, value);
            }
        }
    }
}