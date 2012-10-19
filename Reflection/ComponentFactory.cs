namespace Internals.Reflection
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using Extensions;

    /// <summary>
    /// Provides registration and resolution of components within an AppDomain. The registrations are static,
    /// but done to provide a global style of registration without the management of a full container.
    /// </summary>
    static class ComponentFactory
    {
        static readonly MethodInfo _add = new Action<Func<object>>(Add)
#if !NETFX_CORE
            .Method.GetGenericMethodDefinition();
#else
            .GetMethodInfo().GetGenericMethodDefinition();
#endif
        static readonly MethodInfo _get = new Func<object>(Get<object>)
#if !NETFX_CORE
            .Method.GetGenericMethodDefinition();
#else
            .GetMethodInfo().GetGenericMethodDefinition();
#endif
        static readonly MethodInfo _remove = new Action(Remove<object>)
#if !NETFX_CORE
            .Method.GetGenericMethodDefinition();
#else
            .GetMethodInfo().GetGenericMethodDefinition();
#endif

        /// <summary>
        /// Add a type with a factory method for creating the type
        /// </summary>
        /// <typeparam name="T">The type to add</typeparam>
        /// <param name="factoryMethod">The factory method for the class that implements the added type</param>
        public static void Add<T>(Func<T> factoryMethod)
        {
            if (factoryMethod == null)
                throw new ArgumentNullException("factoryMethod", "A valid factory method must be supplied");

            if (Factory<T>.Get != null)
                throw new ArgumentException(string.Format("A factory method for the type '{0}' was already added",
                    typeof(T).Name));

            Factory<T>.Get = factoryMethod;
        }

        /// <summary>
        /// Adds a type with an implementation that has no dependencies
        /// </summary>
        /// <typeparam name="T">The type to add</typeparam>
        /// <typeparam name="TImplementation">The implementation type to add</typeparam>
        public static void Add<T, TImplementation>()
            where TImplementation : T, new()
        {
            Add<T>(() => new TImplementation());
        }

        /// <summary>
        /// Add a type with a implementation type that may have dependencies
        /// </summary>
        /// <typeparam name="T">The type to add</typeparam>
        /// <param name="implementationType">The implementation type to add</param>
        public static void Add<T>(Type implementationType)
        {
            Add(typeof(T), implementationType);
        }

        /// <summary>
        /// Add a concrete type with no dependencies
        /// </summary>
        /// <typeparam name="T">The type to add</typeparam>
        public static void Add<T>()
            where T : new()
        {
            Add(() => new T());
        }

        /// <summary>
        /// Add a type with a single instance implementing the type
        /// </summary>
        /// <typeparam name="T">The type to add</typeparam>
        /// <param name="instance">The instance of a class implementing the added type</param>
        public static void Add<T>(T instance)
        {
            if (Factory<T>.Get != null)
                throw new ArgumentException(string.Format("A factory method for the type '{0}' was already added",
                    typeof(T).Name));

            Factory<T>.Get = () => instance;
        }

        /// <summary>
        /// Add a type with the same implementation type, automatically resolving any dependencies
        /// </summary>
        /// <param name="addType">The type to add</param>
        public static void Add(Type addType)
        {
            try
            {
                Add(addType, addType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Add a type with an implementation type selecting the greediest constructor on the implementation
        /// that can be satisfied by the registered types
        /// </summary>
        /// <param name="addType">The type to add</param>
        /// <param name="implementationType">The implementation type to add</param>
        public static void Add(Type addType, Type implementationType)
        {
            if (!implementationType.IsConcreteType())
                throw new ArgumentException(string.Format("The type '{0}' must be a concrete type",
                    addType.Name));
#if !NETFX_CORE
            IOrderedEnumerable<ConstructorInfo> candidates = implementationType.GetConstructors()
                .OrderByDescending(x => x.GetParameters().Length);
#else
            IOrderedEnumerable<ConstructorInfo> candidates = implementationType.GetTypeInfo().DeclaredConstructors
                .OrderByDescending(x => x.GetParameters().Length);
#endif

            Exception lastException = null;
            foreach (ConstructorInfo candidate in candidates)
            {
                try
                {
                    Add(addType, implementationType, candidate.GetParameters().Select(x => x.ParameterType).ToArray());
                    return;
                }
                catch (Exception ex)
                {
                    lastException = ex;
                }
            }

            throw lastException ?? new InvalidOperationException(
                                       string.Format("No constructor on type '{0}' could be satisfied", addType.Name));
        }


        /// <summary>
        /// Add a type with a specific list of dependency types matching an existing constructor
        /// on the implementation type
        /// </summary>
        /// <param name="addType"> </param>
        /// <param name="implementationType"> </param>
        /// <param name="dependencies"></param>
        public static void Add(Type addType, Type implementationType, params Type[] dependencies)
        {
            if (!implementationType.IsConcreteType())
                throw new ArgumentException(string.Format("The implementation type '{0}' must be a concrete type",
                    implementationType.Name));
#if !NETFX_CORE
            if (!addType.IsAssignableFrom(implementationType))
#else
            if (!addType.GetTypeInfo().IsAssignableFrom(implementationType.GetTypeInfo()))
#endif
            throw new ArgumentException(
                    string.Format("The implementation type '{0}' must be assignable to the added type '{1}'",
                        implementationType.Name, addType.Name));

#if !NETFX_CORE
            ConstructorInfo constructor = implementationType.GetConstructor(dependencies);
#else
            ConstructorInfo constructor = implementationType.GetTypeInfo().DeclaredConstructors.First(ci => FindMatchingCtor(ci, dependencies));
#endif
            if (constructor == null)
                throw new ArgumentException(
                    string.Format("No constructor on type '{0}' accepts ({1})", implementationType.Name,
#if !NET35
                        string.Join(",", dependencies.Select(x => x.Name))));
#else
                        string.Join(",", dependencies.Select(x => x.Name).ToArray())));
#endif

            ParameterInfo[] parameters = constructor.GetParameters();

            IEnumerable<Expression> arguments = dependencies
                .Select((type, index) => GetResolveExpression(parameters[index].ParameterType));

            NewExpression newExpression = Expression.New(constructor, arguments);

            Delegate factoryMethod = Expression.Lambda(Expression.TypeAs(newExpression, implementationType)).Compile();

            try
            {
                _add.MakeGenericMethod(addType).Invoke(null, new object[] {factoryMethod});
            }
            catch (Exception ex)
            {
                throw ex.InnerException ?? ex;
            }
        }

#if NETFX_CORE
        static bool FindMatchingCtor(ConstructorInfo ci, Type[] dependencies)
        {
            var pars = ci.GetParameters();
            return (from c in pars
                    where dependencies.Any(d => d == c.ParameterType)
                          && dependencies.Length == pars.Length
                    select c).FirstOrDefault() != null;
        }
#endif

        /// <summary>
        /// Resolve the specified type
        /// </summary>
        /// <typeparam name="T">The type to resolve</typeparam>
        /// <returns>An implementation of the specified type</returns>
        public static T Get<T>()
        {
            if (Factory<T>.Get == null)
                TryToResolveTypeFactory(typeof(T));

            if (Factory<T>.Get == null)
                throw new InvalidOperationException(
                    string.Format("The type '{0}' has not been added", typeof(T).Name));

            return Factory<T>.Get();
        }

        static void TryToResolveTypeFactory(Type componentType)
        {
            try
            {
                Add(componentType);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    string.Format("The type '{0}' has not been added", componentType.Name), ex);
            }
        }

        /// <summary>
        /// Remove the type
        /// </summary>
        /// <typeparam name="T">The type to remove</typeparam>
        public static void Remove<T>()
        {
            Factory<T>.Get = null;
        }

        /// <summary>
        /// Remove the type
        /// </summary>
        /// <param name="type">The type to remove</param>
        public static void Remove(Type type)
        {
            try
            {
                _remove.MakeGenericMethod(type)
                    .Invoke(null, null);
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }

        static Expression GetResolveExpression(Type type)
        {
            return Expression.Call(null, _get.MakeGenericMethod(type));
        }
    }
}