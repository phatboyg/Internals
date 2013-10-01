namespace Internals.Mapping
{
    using Reflection;


    class ObjectObjectMapper<T> :
        ObjectMapper<T>
    {
        readonly ObjectConverter _converter;
        readonly ReadWriteProperty<T> _property;

        public ObjectObjectMapper(ReadWriteProperty<T> property,
            ObjectConverter converter)
        {
            _property = property;
            _converter = converter;
        }

        public void ApplyTo(T obj, ObjectValueProvider valueProvider)
        {
            ObjectValueProvider propertyProvider;
            if (valueProvider.TryGetValue(_property.Property.Name, out propertyProvider))
            {
                object propertyObj = _converter.GetObject(propertyProvider);

                _property.Set(obj, propertyObj);
            }
        }
    }
}