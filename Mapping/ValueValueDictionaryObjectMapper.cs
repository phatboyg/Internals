namespace Internals.Mapping
{
    using System.Collections.Generic;
    using Reflection;


    class ValueValueDictionaryObjectMapper<T, TKey, TValue> :
        ObjectMapper<T>
    {
        readonly ReadWriteProperty<T> _property;

        public ValueValueDictionaryObjectMapper(ReadWriteProperty<T> property)
        {
            _property = property;
        }

        public void ApplyTo(T obj, ObjectValueProvider valueProvider)
        {
            ArrayValueProvider values;
            if (!valueProvider.TryGetValue(_property.Property.Name, out values))
                return;

            var elements = new Dictionary<TKey, TValue>();

            for (int i = 0;; i++)
            {
                ArrayValueProvider elementArray;
                if (!values.TryGetValue(i, out elementArray))
                    break;

                TKey elementKey;
                if (elementArray.TryGetValue(0, out elementKey))
                {
                    TValue elementValue;
                    elementArray.TryGetValue(1, out elementValue);

                    elements[elementKey] = elementValue;
                }
            }

            _property.Set(obj, elements);
        }
    }
}