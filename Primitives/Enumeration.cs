namespace Internals.Primitives
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using Caching;
    using Extensions;

#if !NETFX_CORE
    [Serializable]
#endif
    public abstract class Enumeration :
        IComparable
    {
        static readonly Cache<Type, EnumerationInfo> _cache;

        public readonly int Index;
        public readonly string Name;

        static Enumeration()
        {
#if NET35
            _cache = new ReaderWriterLockedCache<Type, EnumerationInfo>(new DictionaryCache<Type, EnumerationInfo>(type => new EnumerationInfo(type)));
#else
            _cache = new ConcurrentCache<Type, EnumerationInfo>(type => new EnumerationInfo(type));
#endif
        }

        protected Enumeration()
        {
        }

        protected Enumeration(int index, string name)
        {
            Index = index;
            Name = name;
        }

        public virtual int CompareTo(object other)
        {
            return Index.CompareTo(((Enumeration)other).Index);
        }

        public override string ToString()
        {
            return Name;
        }

        public static IEnumerable<T> GetAll<T>()
            where T : Enumeration, new()
        {
            return _cache[typeof(T)].Cast<T>();
        }

        public static IEnumerable<Enumeration> GetAll(Type type)
        {
            return _cache[type];
        }

        public static T FromIndex<T>(int index)
            where T : Enumeration
        {
            return _cache[typeof(T)].FromIndex(index) as T;
        }

        public static T Parse<T>(string name)
            where T : Enumeration
        {
            return _cache[typeof(T)].Parse(name) as T;
        }

        public override bool Equals(object obj)
        {
            var otherValue = obj as Enumeration;
            if (otherValue == null)
                return false;

            return GetType() == obj.GetType() && Index.Equals(otherValue.Index);
        }

        public override int GetHashCode()
        {
            return Index.GetHashCode();
        }

        protected static T Init<T>(Expression<Func<T>> expression, int index, params object[] args)
        {
            string name = expression.GetMemberName();

            IEnumerable<object> arguments = new object[] {index, name};
            if (args != null)
                arguments = arguments.Concat(args);

            return (T)Activator.CreateInstance(typeof(T), arguments.ToArray());
        }

        class EnumerationInfo :
            IEnumerable<Enumeration>
        {
            readonly Cache<int, Enumeration> _indices;
            readonly Cache<string, Enumeration> _names;

            public EnumerationInfo(Type type)
            {
                _names = new DictionaryCache<string, Enumeration>();
                _indices = new DictionaryCache<int, Enumeration>();
#if !NETFX_CORE
                const BindingFlags flags = BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly;
                IEnumerable<FieldInfo> fields = type.GetFields(flags);
#else
                IEnumerable<FieldInfo> fields = type.GetTypeInfo().DeclaredFields; // TODO: statics??!
#endif

                foreach (FieldInfo field in fields)
                {
                    var value = (Enumeration)field.GetValue(null);

                    _names.Add(value.Name, value);
                    _indices.Add(value.Index, value);
                }
            }

            public IEnumerator<Enumeration> GetEnumerator()
            {
                return _indices.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public Enumeration FromIndex(int index)
            {
                if (_indices.Has(index))
                    return _indices[index];

                throw new ArgumentException("The index was not found: " + index, "index");
            }

            public Enumeration Parse(string name)
            {
                if (_names.Has(name))
                    return _names[name];

                throw new ArgumentException("The name was not found: " + name, "name");
            }
        }
    }
}