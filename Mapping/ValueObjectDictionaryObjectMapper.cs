namespace Internals.Mapping
{
    using System.Collections.Generic;
    using Reflection;


    class ValueObjectDictionaryObjectMapper<T, TKey, TValue> :
        ObjectMapper<T>
    {
        readonly ReadWriteProperty<T> _property;
        readonly ObjectConverter _valueConverter;

        public ValueObjectDictionaryObjectMapper(ReadWriteProperty<T> property, ObjectConverter valueConverter)
        {
            _property = property;
            _valueConverter = valueConverter;
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
                    TValue elementValue = default(TValue);
                    ObjectValueProvider elementValueProvider;
                    if (elementArray.TryGetValue(1, out elementValueProvider))
                        elementValue = (TValue)_valueConverter.GetObject(elementValueProvider);

                    elements[elementKey] = elementValue;
                }
            }

            _property.Set(obj, elements);
        }
    }
}