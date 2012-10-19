namespace Internals.Reflection
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;
    using Extensions;

    class ReadOnlyProperty
    {
        public readonly Func<object, object> GetProperty;

        public ReadOnlyProperty(PropertyInfo property)
        {
            Property = property;
            GetProperty = GetGetMethod(Property);
        }

        public PropertyInfo Property { get; private set; }

        public object Get(object instance)
        {
            return GetProperty(instance);
        }

        static Func<object, object> GetGetMethod(PropertyInfo property)
        {
            ParameterExpression instance = Expression.Parameter(typeof(object), "instance");
            UnaryExpression instanceCast;
#if !NETFX_CORE
            if (property.DeclaringType.IsValueType)
#else
            if (property.DeclaringType.GetTypeInfo().IsValueType)
#endif
                instanceCast = Expression.Convert(instance, property.DeclaringType);
            else
                instanceCast = Expression.TypeAs(instance, property.DeclaringType);

#if !NETFX_CORE
            MethodCallExpression call = Expression.Call(instanceCast, property.GetGetMethod());
#else
            MethodCallExpression call = Expression.Call(instanceCast, property.GetMethod);
#endif
            UnaryExpression typeAs = Expression.TypeAs(call, typeof(object));

            return Expression.Lambda<Func<object, object>>(typeAs, instance).Compile();
        }
    }

    class ReadOnlyProperty<T>
    {
        public readonly Func<T, object> GetProperty;

        public ReadOnlyProperty(Expression<Func<T, object>> propertyExpression)
            : this(propertyExpression.GetPropertyInfo())
        {
        }

        public ReadOnlyProperty(PropertyInfo property)
        {
            Property = property;
            GetProperty = GetGetMethod(Property);
        }

        public PropertyInfo Property { get; private set; }

        public object Get(T instance)
        {
            return GetProperty(instance);
        }

        static Func<T, object> GetGetMethod(PropertyInfo property)
        {
            ParameterExpression instance = Expression.Parameter(typeof(T), "instance");
#if !NETFX_CORE
            MethodCallExpression call = Expression.Call(instance, property.GetGetMethod());
#else
            MethodCallExpression call = Expression.Call(instance, property.GetMethod);
#endif
            UnaryExpression typeAs = Expression.TypeAs(call, typeof(object));
            return Expression.Lambda<Func<T, object>>(typeAs, instance).Compile();
        }
    }

    class ReadOnlyProperty<T, TProperty>
    {
        public readonly Func<T, TProperty> GetProperty;

        public ReadOnlyProperty(Expression<Func<T, object>> propertyExpression)
            : this(propertyExpression.GetPropertyInfo())
        {
        }

        public ReadOnlyProperty(PropertyInfo property)
        {
            Property = property;
            GetProperty = GetGetMethod(Property);
        }

        public PropertyInfo Property { get; private set; }

        public TProperty Get(T instance)
        {
            return GetProperty(instance);
        }

        static Func<T, TProperty> GetGetMethod(PropertyInfo property)
        {
            ParameterExpression instance = Expression.Parameter(typeof(T), "instance");
#if !NETFX_CORE
            MethodCallExpression call = Expression.Call(instance, property.GetGetMethod());
#else
            MethodCallExpression call = Expression.Call(instance, property.GetMethod);
#endif

            return Expression.Lambda<Func<T, TProperty>>(call, instance).Compile();
        }
    }
}