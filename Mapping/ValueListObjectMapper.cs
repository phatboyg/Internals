namespace Internals.Mapping
{
    using System.Collections.Generic;
    using System.Linq;
    using Reflection;


    class ValueListObjectMapper<T, TElement> :
        ObjectMapper<T>
    {
        readonly ReadWriteProperty<T> _property;

        public ValueListObjectMapper(ReadWriteProperty<T> property)
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

            var elements = new List<TElement>(values.Length);
            elements.AddRange(values.Select(t => (TElement)t));

            _property.Set(obj, elements);
        }
    }
}