namespace Internals.Caching
{
    delegate void CacheItemCallback<TKey, TValue>(TKey key, TValue value);
}