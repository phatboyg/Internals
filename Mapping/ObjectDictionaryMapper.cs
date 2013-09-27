namespace Internals.Mapping
{
    using System.Collections.Generic;
    using Reflection;


    class ObjectDictionaryMapper<T> :
        DictionaryMapper<T>
    {
        readonly DictionaryConverter _converter;
        readonly ReadOnlyProperty<T> _property;

        public ObjectDictionaryMapper(ReadOnlyProperty<T> property, DictionaryConverter converter)
        {
            _property = property;
            _converter = converter;
        }

        public void WritePropertyToDictionary(IDictionary<string, object> dictionary, T obj)
        {
            IDictionary<string, object> value = _converter.GetDictionary(obj);

            dictionary.Add(_property.Property.Name, value);
        }
    }
}