namespace Internals.Mapping
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Reflection;


    class DynamicObjectConverter<T, TImplementation> :
        ObjectConverter
        where TImplementation : T, new()
        where T : class
    {
        readonly ObjectConverterCache _cache;
        readonly ObjectMapper<TImplementation>[] _converters;
        readonly ReadWritePropertyCache<TImplementation> _propertyCache;

        public DynamicObjectConverter(ObjectConverterCache cache)
        {
            _cache = cache;
            _propertyCache = new ReadWritePropertyCache<TImplementation>();
            _converters = _propertyCache
                .Select(property => GetDictionaryToObjectConverter(property, property.Property.PropertyType))
                .ToArray();
        }

        public object GetObject(IDictionary<string, object> dictionary)
        {
            var implementation = new TImplementation();

            for (int i = 0; i < _converters.Length; i++)
                _converters[i].ApplyTo(implementation, dictionary);


            return implementation;
        }

        ObjectMapper<TImplementation> GetDictionaryToObjectConverter(
            ReadWriteProperty<TImplementation> property, Type valueType)
        {
            Type underlyingType = Nullable.GetUnderlyingType(valueType);
            if (underlyingType != null)
            {
                Type converterType =
                    typeof(NullableValueObjectMapper<,>).MakeGenericType(typeof(TImplementation),
                        underlyingType);

                return (ObjectMapper<TImplementation>)Activator.CreateInstance(converterType, property);
            }

            if (valueType.IsEnum)
                return new EnumObjectMapper<TImplementation>(property);

            if (valueType.IsArray)
            {
                Type elementType = valueType.GetElementType();
                if (elementType.IsValueType || elementType == typeof(string))
                {
                    Type valueConverterType = typeof(ValueArrayObjectMapper<,>).MakeGenericType(
                        typeof(TImplementation), elementType);
                    return (ObjectMapper<TImplementation>)Activator.CreateInstance(valueConverterType, property);
                }

                ObjectConverter elementConverter = _cache.GetConverter(elementType);

                Type converterType = typeof(ObjectArrayObjectMapper<,>).MakeGenericType(typeof(TImplementation),
                    elementType);
                return
                    (ObjectMapper<TImplementation>)Activator.CreateInstance(converterType, property, elementConverter);
            }

            //            if (valueType.HasInterface(typeof(IEnumerable<>)))
            //            {
            //                Type[] genericArguments = valueType.GetClosingArguments(typeof(IEnumerable<>)).ToArray();
            //
            ////                if (valueType.HasInterface(typeof(IDictionary<,>)))
            ////                    return CreateDictionarySerializer(type, genericArguments[0], genericArguments[1]);
            ////
            ////                if (type.HasInterface(typeof(IList<>)) || type.ImplementsGeneric(typeof(IEnumerable<>)))
            ////                    return CreateListSerializer(type, genericArguments[0]);
            //            }
            //
            if (valueType.IsValueType || valueType == typeof(string))
                return new ValueObjectMapper<TImplementation>(property);

            return new ObjectObjectMapper<TImplementation>(property,
                _cache.GetConverter(valueType));
        }
    }
}