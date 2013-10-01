namespace Internals.Mapping
{
    using System.Collections.Generic;
    using Reflection;


    class ObjectArrayObjectMapper<T, TElement> :
        ObjectMapper<T>
    {
        readonly ObjectConverter _converter;
        readonly ReadWriteProperty<T> _property;

        public ObjectArrayObjectMapper(ReadWriteProperty<T> property,
            ObjectConverter converter)
        {
            _property = property;
            _converter = converter;
        }

        public void ApplyTo(T obj, ObjectValueProvider valueProvider)
        {
            ArrayValueProvider values;
            if (!valueProvider.TryGetValue(_property.Property.Name, out values))
                return;

            var elements = new List<TElement>();

            for (int i = 0;; i++)
            {
                ObjectValueProvider elementValueProvider;
                if (!values.TryGetValue(i, out elementValueProvider))
                    break;

                var element = (TElement)_converter.GetObject(elementValueProvider);
                elements.Add(element);
            }

            _property.Set(obj, elements.ToArray());
        }
    }
}