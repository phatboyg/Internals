namespace Internals.Mapping
{
    using System.Collections.Generic;
    using Reflection;


    class ValueListObjectMapper<T, TElement> :
        ObjectMapper<T>
    {
        readonly ReadWriteProperty<T> _property;

        public ValueListObjectMapper(ReadWriteProperty<T> property)
        {
            _property = property;
        }

        public void ApplyTo(T obj, ObjectValueProvider valueProvider)
        {
            ArrayValueProvider values;
            if (!valueProvider.TryGetValue(_property.Property.Name, out values))
                return;

            var elements = new List<TElement>();

            for (int i = 0;; i++)
            {
                TElement element;
                if (!values.TryGetValue(i, out element))
                    break;

                elements.Add(element);
            }

            _property.Set(obj, elements.ToArray());
        }
    }
}