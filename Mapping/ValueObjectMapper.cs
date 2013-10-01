namespace Internals.Mapping
{
    using Reflection;


    class ValueObjectMapper<T> :
        ObjectMapper<T>
    {
        readonly ReadWriteProperty<T> _property;

        public ValueObjectMapper(ReadWriteProperty<T> property)
        {
            _property = property;
        }

        public void ApplyTo(T obj, ObjectValueProvider valueProvider)
        {
            object value;
            if (valueProvider.TryGetValue(_property.Property.Name, out value))
            {
                if (value != null)
                    _property.Set(obj, value);
            }
        }
    }
}