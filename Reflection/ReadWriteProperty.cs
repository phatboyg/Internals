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

        public ReadWriteProperty(PropertyInfo property, bool includeReadOnly = false)
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
            if (property.DeclaringType.IsValueType)
                instanceCast = Expression.Convert(instance, property.DeclaringType);
            else
                instanceCast = Expression.TypeAs(instance, property.DeclaringType);

            UnaryExpression valueCast;
            if (property.PropertyType.IsValueType)
                valueCast = Expression.Convert(value, property.PropertyType);
            else
                valueCast = Expression.TypeAs(value, property.PropertyType);

            MethodCallExpression call = Expression.Call(instanceCast, property.GetSetMethod(includeNonPublic), valueCast);

            return Expression.Lambda<Action<object, object>>(call, new[] {instance, value}).Compile();
        }
    }

    class ReadWriteProperty<T> :
        ReadOnlyProperty<T>
    {
        public readonly Action<T, object> SetProperty;

        public ReadWriteProperty(Expression<Func<T, object>> propertyExpression, bool includeNonPublic = false)
            : this(propertyExpression.GetPropertyInfo(), includeNonPublic)
        {
        }

        public ReadWriteProperty(PropertyInfo property, bool includeNonPublic = false)
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
            if (property.PropertyType.IsValueType)
                valueCast = Expression.Convert(value, property.PropertyType);
            else
                valueCast = Expression.TypeAs(value, property.PropertyType);

            MethodCallExpression call = Expression.Call(instance, property.GetSetMethod(includeNonPublic), valueCast);

            return Expression.Lambda<Action<T, object>>(call, new[] {instance, value}).Compile();
        }
    }

    class ReadWriteProperty<T, TProperty> :
        ReadOnlyProperty<T, TProperty>
    {
        public readonly Action<T, TProperty> SetProperty;

        public ReadWriteProperty(Expression<Func<T, object>> propertyExpression, bool includeNonPublic = false)
            : this(propertyExpression.GetPropertyInfo(), includeNonPublic)
        {
        }

        public ReadWriteProperty(PropertyInfo property, bool includeNonPublic = false)
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
            MethodCallExpression call = Expression.Call(instance, property.GetSetMethod(includeNonPublic), value);

            return Expression.Lambda<Action<T, TProperty>>(call, new[] {instance, value}).Compile();
        }
    }
}