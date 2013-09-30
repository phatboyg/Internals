namespace Internals.Mapping
{
    using System.Collections.Generic;
    using Reflection;


    class ObjectListObjectMapper<T, TElement> :
        ObjectMapper<T>
    {
        readonly ObjectConverter _converter;
        readonly ReadWriteProperty<T> _property;

        public ObjectListObjectMapper(ReadWriteProperty<T> property,
            ObjectConverter converter)
        {
            _property = property;
            _converter = converter;
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

            for (int i = 0; i < values.Length; i++)
            {
                var elementDictionary = values[i] as IDictionary<string, object>;
                if (elementDictionary != null)
                {
                    var element = (TElement)_converter.GetObject(elementDictionary);
                    elements.Add(element);
                }
            }

            _property.Set(obj, elements);
        }
    }
}