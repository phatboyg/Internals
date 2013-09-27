namespace Internals.Mapping
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Extensions;
    using Reflection;


    class ObjectDictionaryConverter<T> :
        DictionaryConverter
        where T : class
    {
        readonly DictionaryConverterCache _cache;
        readonly DictionaryMapper<T>[] _mappers;
        readonly ReadOnlyPropertyCache<T> _propertyCache;

        public ObjectDictionaryConverter(DictionaryConverterCache cache)
        {
            _cache = cache;
            _propertyCache = new ReadOnlyPropertyCache<T>();
            _mappers = _propertyCache
                .Select(property => GetDictionaryMapper(property, property.Property.PropertyType))
                .ToArray();
        }

        public IDictionary<string, object> GetDictionary(object obj)
        {
            var dictionary = new Dictionary<string, object>();
            if (obj == null)
                return dictionary;

            var value = (T)obj;
            for (int i = 0; i < _mappers.Length; i++)
                _mappers[i].WritePropertyToDictionary(dictionary, value);

            return dictionary;
        }

        DictionaryMapper<T> GetDictionaryMapper(ReadOnlyProperty<T> property, Type valueType)
        {
            Type underlyingType = Nullable.GetUnderlyingType(valueType);
            if (underlyingType != null)
            {
                Type converterType = typeof(NullableValueDictionaryMapper<,>).MakeGenericType(typeof(T),
                    underlyingType);

                return (DictionaryMapper<T>)Activator.CreateInstance(converterType, property);
            }

            if (valueType.IsEnum)
                return new EnumDictionaryMapper<T>(property);

            if (valueType.IsArray)
            {
                Type elementType = valueType.GetElementType();
                if (elementType.IsValueType || elementType == typeof(string))
                    return new ValueDictionaryMapper<T>(property);

                DictionaryConverter elementConverter = _cache.GetConverter(elementType);

                Type converterType = typeof(ObjectArrayDictionaryMapper<,>).MakeGenericType(typeof(T), elementType);
                return (DictionaryMapper<T>)Activator.CreateInstance(converterType, property, elementConverter);
            }

            if (valueType.HasInterface(typeof(IEnumerable<>)))
            {
                Type[] genericArguments = valueType.GetClosingArguments(typeof(IEnumerable<>)).ToArray();

//                if (valueType.HasInterface(typeof(IDictionary<,>)))
//                    return CreateDictionarySerializer(type, genericArguments[0], genericArguments[1]);
//
//                if (type.HasInterface(typeof(IList<>)) || type.ImplementsGeneric(typeof(IEnumerable<>)))
//                    return CreateListSerializer(type, genericArguments[0]);
            }

            if (valueType.IsValueType || valueType == typeof(string))
                return new ValueDictionaryMapper<T>(property);

            return new ObjectDictionaryMapper<T>(property, _cache.GetConverter(valueType));
        }
    }
}