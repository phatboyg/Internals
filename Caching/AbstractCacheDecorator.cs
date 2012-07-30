namespace Internals.Caching
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    abstract class AbstractCacheDecorator<TKey, TValue> :
        Cache<TKey, TValue>
    {
        readonly Cache<TKey, TValue> _cache;

        protected AbstractCacheDecorator(Cache<TKey, TValue> cache)
        {
            _cache = cache;
        }

        public virtual IEnumerator<TValue> GetEnumerator()
        {
            return _cache.GetEnumerator();
        }

        public virtual int Count
        {
            get { return _cache.Count; }
        }

        public virtual bool Has(TKey key)
        {
            return _cache.Has(key);
        }

        public virtual bool HasValue(TValue value)
        {
            return _cache.HasValue(value);
        }

        public virtual void Each(Action<TValue> callback)
        {
            _cache.Each(callback);
        }

        public virtual void Each(Action<TKey, TValue> callback)
        {
            _cache.Each(callback);
        }

        public virtual bool Exists(Predicate<TValue> predicate)
        {
            return _cache.Exists(predicate);
        }

        public virtual bool Find(Predicate<TValue> predicate, out TValue result)
        {
            return _cache.Find(predicate, out result);
        }

        public virtual TKey[] GetAllKeys()
        {
            return _cache.GetAllKeys();
        }

        public virtual TValue[] GetAll()
        {
            return _cache.GetAll();
        }

        public virtual MissingValueProvider<TKey, TValue> MissingValueProvider
        {
            set { _cache.MissingValueProvider = value; }
        }

        public virtual CacheItemCallback<TKey, TValue> ValueAddedCallback
        {
            set { _cache.ValueAddedCallback = value; }
        }

        public virtual CacheItemCallback<TKey, TValue> ValueRemovedCallback
        {
            set { _cache.ValueRemovedCallback = value; }
        }

        public virtual CacheItemCallback<TKey, TValue> DuplicateValueAdded
        {
            set { _cache.DuplicateValueAdded = value; }
        }

        public virtual KeySelector<TKey, TValue> KeySelector
        {
            set { _cache.KeySelector = value; }
        }

        public virtual TValue this[TKey key]
        {
            get { return _cache[key]; }
            set { _cache[key] = value; }
        }

        public virtual TValue Get(TKey key)
        {
            return _cache.Get(key);
        }

        public virtual TValue Get(TKey key, MissingValueProvider<TKey, TValue> missingValueProvider)
        {
            return _cache.Get(key, missingValueProvider);
        }

        public virtual void Add(TKey key, TValue value)
        {
            _cache.Add(key, value);
        }

        public virtual void AddValue(TValue value)
        {
            _cache.AddValue(value);
        }

        public virtual void Remove(TKey key)
        {
            _cache.Remove(key);
        }

        public virtual void RemoveValue(TValue value)
        {
            _cache.RemoveValue(value);
        }

        public virtual void Clear()
        {
            _cache.Clear();
        }

        public virtual void Fill(IEnumerable<TValue> values)
        {
            _cache.Fill(values);
        }

        public virtual bool WithValue(TKey key, Action<TValue> callback)
        {
            return _cache.WithValue(key, callback);
        }

        public virtual TResult WithValue<TResult>(TKey key, Func<TValue, TResult> callback,
            TResult defaultValue)
        {
            return _cache.WithValue(key, callback, defaultValue);
        }

        public TResult WithValue<TResult>(TKey key, Func<TValue, TResult> callback, Func<TKey, TResult> defaultValue)
        {
            return _cache.WithValue(key, callback, defaultValue);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}