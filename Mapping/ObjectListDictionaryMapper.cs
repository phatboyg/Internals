namespace Internals.Mapping
{
    using System.Collections.Generic;
    using Reflection;


    class ObjectListDictionaryMapper<T, TElement> :
        DictionaryMapper<T>
    {
        readonly DictionaryConverter _elementConverter;
        readonly ReadOnlyProperty<T> _property;

        public ObjectListDictionaryMapper(ReadOnlyProperty<T> property, DictionaryConverter elementConverter)
        {
            _property = property;
            _elementConverter = elementConverter;
        }

        public void WritePropertyToDictionary(IDictionary<string, object> dictionary, T obj)
        {
            object value = _property.Get(obj);
            if (value == null)
                return;

            var values = value as IList<TElement>;
            if (values == null)
                return;

            var elements = new object[values.Count];
            for (int i = 0; i < values.Count; i++)
                elements[i] = _elementConverter.GetDictionary(values[i]);

            dictionary.Add(_property.Property.Name, elements);
        }
    }
}