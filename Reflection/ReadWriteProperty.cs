namespace Internals.Reflection
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;
    using Extensions;

    class ReadWriteProperty :
        ReadOnlyProperty
    {
        public readonly Action<object, object> SetProperty;

        public ReadWriteProperty(PropertyInfo property)
            : this(property, false)
        {
        }

        public ReadWriteProperty(PropertyInfo property, bool includeReadOnly)
            : base(property)
        {
            SetProperty = GetSetMethod(Property, includeReadOnly);
        }

        public void Set(object instance, object value)
        {
            SetProperty(instance, value);
        }

        static Action<object, object> GetSetMethod(PropertyInfo property, bool includeNonPublic)
        {
            ParameterExpression instance = Expression.Parameter(typeof(object), "instance");
            ParameterExpression value = Expression.Parameter(typeof(object), "value");

            // value as T is slightly faster than (T)value, so if it's not a value type, use that
            UnaryExpression instanceCast;
#if !NETFX_CORE
            if (property.DeclaringType.IsValueType)
#else
            if (property.DeclaringType.GetTypeInfo().IsValueType)
#endif
                instanceCast = Expression.Convert(instance, property.DeclaringType);
            else
                instanceCast = Expression.TypeAs(instance, property.DeclaringType);

            UnaryExpression valueCast;
#if !NETFX_CORE
            if (property.PropertyType.IsValueType)
#else
            if (property.PropertyType.GetTypeInfo().IsValueType)
#endif
                valueCast = Expression.Convert(value, property.PropertyType);
            else
                valueCast = Expression.TypeAs(value, property.PropertyType);

#if !NETFX_CORE
            MethodCallExpression call = Expression.Call(instanceCast, property.GetSetMethod(includeNonPublic), valueCast);
#else
            MethodCallExpression call = Expression.Call(instanceCast, property.SetMethod, valueCast);
#endif

            return Expression.Lambda<Action<object, object>>(call, new[] {instance, value}).Compile();
        }
    }

    class ReadWriteProperty<T> :
        ReadOnlyProperty<T>
    {
        public readonly Action<T, object> SetProperty;

        public ReadWriteProperty(Expression<Func<T, object>> propertyExpression)
            : this(propertyExpression.GetPropertyInfo(), false)
        {
        }

        public ReadWriteProperty(Expression<Func<T, object>> propertyExpression, bool includeNonPublic)
            : this(propertyExpression.GetPropertyInfo(), includeNonPublic)
        {
        }

        public ReadWriteProperty(PropertyInfo property)
            : this(property, false)
        {
        }

        public ReadWriteProperty(PropertyInfo property, bool includeNonPublic)
            : base(property)
        {
            SetProperty = GetSetMethod(Property, includeNonPublic);
        }

        public void Set(T instance, object value)
        {
            SetProperty(instance, value);
        }

        static Action<T, object> GetSetMethod(PropertyInfo property, bool includeNonPublic)
        {
            if (!property.CanWrite)
                return (x, i) => { throw new InvalidOperationException("No setter available on " + property.Name); };


            ParameterExpression instance = Expression.Parameter(typeof(T), "instance");
            ParameterExpression value = Expression.Parameter(typeof(object), "value");
            UnaryExpression valueCast;
#if !NETFX_CORE
            if (property.PropertyType.IsValueType)
#else
            if (property.PropertyType.GetTypeInfo().IsValueType)
#endif
                valueCast = Expression.Convert(value, property.PropertyType);
            else
                valueCast = Expression.TypeAs(value, property.PropertyType);
#if !NETFX_CORE
            MethodCallExpression call = Expression.Call(instance, property.GetSetMethod(includeNonPublic), valueCast);
#else
            MethodCallExpression call = Expression.Call(instance, property.SetMethod, valueCast);
#endif

            return Expression.Lambda<Action<T, object>>(call, new[] {instance, value}).Compile();
        }
    }

    class ReadWriteProperty<T, TProperty> :
        ReadOnlyProperty<T, TProperty>
    {
        public readonly Action<T, TProperty> SetProperty;

        public ReadWriteProperty(Expression<Func<T, object>> propertyExpression)
            : this(propertyExpression.GetPropertyInfo(), false)
        {
        }

        public ReadWriteProperty(Expression<Func<T, object>> propertyExpression, bool includeNonPublic)
            : this(propertyExpression.GetPropertyInfo(), includeNonPublic)
        {
        }

        public ReadWriteProperty(PropertyInfo property)
            : this(property, false)
        {
        }

        public ReadWriteProperty(PropertyInfo property, bool includeNonPublic)
            : base(property)
        {
            SetProperty = GetSetMethod(Property, includeNonPublic);
        }

        public void Set(T instance, TProperty value)
        {
            SetProperty(instance, value);
        }

        static Action<T, TProperty> GetSetMethod(PropertyInfo property, bool includeNonPublic)
        {
            ParameterExpression instance = Expression.Parameter(typeof(T), "instance");
            ParameterExpression value = Expression.Parameter(typeof(TProperty), "value");
#if !NETFX_CORE
            MethodCallExpression call = Expression.Call(instance, property.GetSetMethod(includeNonPublic), value);
#else
            MethodCallExpression call = Expression.Call(instance, property.SetMethod, value);
#endif
            return Expression.Lambda<Action<T, TProperty>>(call, new[] {instance, value}).Compile();
        }
    }
}