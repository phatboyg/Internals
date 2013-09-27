namespace Internals.Mapping
{
    using System.Collections.Generic;
    using Reflection;


    class ValueArrayObjectMapper<T, TElement> :
        ObjectMapper<T>
    {
        readonly ReadWriteProperty<T> _property;

        public ValueArrayObjectMapper(ReadWriteProperty<T> property)
        {
            _property = property;
        }

        public void ApplyTo(T obj, IDictionary<string, object> dictionary)
        {
            object value;
            if (!dictionary.TryGetValue(_property.Property.Name, out value))
                return;

            if (value == null)
                return;

            var values = value as object[];
            if (values == null)
                return;

            var elements = new TElement[values.Length];

            for (int i = 0; i < values.Length; i++)
                elements[i] = (TElement)values[i];

            _property.Set(obj, elements);
        }
    }
}