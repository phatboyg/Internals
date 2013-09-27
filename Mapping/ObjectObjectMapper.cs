namespace Internals.Mapping
{
    using System.Collections.Generic;
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

        public void ApplyTo(T obj, IDictionary<string, object> dictionary)
        {
            object value;
            if (dictionary.TryGetValue(_property.Property.Name, out value))
            {
                if (value != null)
                {
                    var propertyDictionary = value as IDictionary<string, object>;

                    if (propertyDictionary != null)
                    {
                        object propertyObj = _converter.GetObject(propertyDictionary);

                        _property.Set(obj, propertyObj);
                    }
                }
            }
        }
    }
}