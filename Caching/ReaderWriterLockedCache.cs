namespace Internals.Caching
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    class ReaderWriterLockedCache<TKey, TValue> :
        Cache<TKey, TValue>,
        IDisposable
    {
        readonly Cache<TKey, TValue> _cache;
        readonly ReaderWriterLockSlim _lock;
        bool _disposed;

        public ReaderWriterLockedCache(Cache<TKey, TValue> cache)
        {
            _cache = cache;
            _lock = new ReaderWriterLockSlim();
        }

        public IEnumerator<TValue> GetEnumerator()
        {
            _lock.EnterReadLock();
            try
            {
                return _cache.ToList().GetEnumerator();
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        public int Count
        {
            get
            {
                _lock.EnterReadLock();
                try
                {
                    return _cache.Count;
                }
                finally
                {
                    _lock.ExitReadLock();
                }
            }
        }

        public bool Has(TKey key)
        {
            _lock.EnterReadLock();
            try
            {
                return _cache.Has(key);
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        public bool HasValue(TValue value)
        {
            _lock.EnterReadLock();
            try
            {
                return _cache.HasValue(value);
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        public void Each(Action<TValue> callback)
        {
            _lock.EnterReadLock();
            try
            {
                _cache.Each(callback);
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        public void Each(Action<TKey, TValue> callback)
        {
            _lock.EnterReadLock();
            try
            {
                _cache.Each(callback);
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        public bool Exists(Predicate<TValue> predicate)
        {
            _lock.EnterReadLock();
            try
            {
                return _cache.Exists(predicate);
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        public bool Find(Predicate<TValue> predicate, out TValue result)
        {
            _lock.EnterReadLock();
            try
            {
                return _cache.Find(predicate, out result);
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        public TKey[] GetAllKeys()
        {
            _lock.EnterReadLock();
            try
            {
                return _cache.GetAllKeys();
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        public TValue[] GetAll()
        {
            _lock.EnterReadLock();
            try
            {
                return _cache.GetAll();
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        public MissingValueProvider<TKey, TValue> MissingValueProvider
        {
            set { _cache.MissingValueProvider = value; }
        }

        public CacheItemCallback<TKey, TValue> ValueAddedCallback
        {
            set { _cache.ValueAddedCallback = value; }
        }

        public CacheItemCallback<TKey, TValue> ValueRemovedCallback
        {
            set { _cache.ValueRemovedCallback = value; }
        }

        public CacheItemCallback<TKey, TValue> DuplicateValueAdded
        {
            set { _cache.DuplicateValueAdded = value; }
        }

        public KeySelector<TKey, TValue> KeySelector
        {
            set { _cache.KeySelector = value; }
        }

        public TValue Get(TKey key)
        {
            _lock.EnterUpgradeableReadLock();
            try
            {
                if (_cache.Has(key))
                    return _cache.Get(key);

                _lock.EnterWriteLock();
                try
                {
                    return _cache.Get(key);
                }
                finally
                {
                    _lock.ExitWriteLock();
                }
            }
            finally
            {
                _lock.ExitUpgradeableReadLock();
            }
        }

        public TValue Get(TKey key, MissingValueProvider<TKey, TValue> missingValueProvider)
        {
            _lock.EnterUpgradeableReadLock();
            try
            {
                if (_cache.Has(key))
                    return _cache.Get(key, missingValueProvider);

                _lock.EnterWriteLock();
                try
                {
                    return _cache.Get(key, missingValueProvider);
                }
                finally
                {
                    _lock.ExitWriteLock();
                }
            }
            finally
            {
                _lock.ExitUpgradeableReadLock();
            }
        }

        public TValue this[TKey key]
        {
            get { return Get(key); }
            set
            {
                _lock.EnterWriteLock();
                try
                {
                    _cache[key] = value;
                }
                finally
                {
                    _lock.ExitWriteLock();
                }
            }
        }

        public void Add(TKey key, TValue value)
        {
            _lock.EnterWriteLock();
            try
            {
                _cache.Add(key, value);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        public void AddValue(TValue value)
        {
            _lock.EnterWriteLock();
            try
            {
                _cache.AddValue(value);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        public void Remove(TKey key)
        {
            _lock.EnterWriteLock();
            try
            {
                _cache.Remove(key);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        public void RemoveValue(TValue value)
        {
            _lock.EnterWriteLock();
            try
            {
                _cache.RemoveValue(value);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        public void Clear()
        {
            _lock.EnterWriteLock();
            try
            {
                _cache.Clear();
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        public void Fill(IEnumerable<TValue> values)
        {
            _lock.EnterWriteLock();
            try
            {
                _cache.Fill(values);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        public bool WithValue(TKey key, Action<TValue> callback)
        {
            _lock.EnterReadLock();
            try
            {
                return _cache.WithValue(key, callback);
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        public TResult WithValue<TResult>(TKey key,
            Func<TValue, TResult> callback,
            TResult defaultValue)
        {
            _lock.EnterReadLock();
            try
            {
                return _cache.WithValue(key, callback, defaultValue);
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~ReaderWriterLockedCache()
        {
            Dispose(false);
        }

        void Dispose(bool disposing)
        {
            if (_disposed)
                return;
            if (disposing)
            {
                _lock.Dispose();
            }

            _disposed = true;
        }
    }
}