namespace Internals.Caching
{
    delegate TValue MissingValueProvider<TKey, TValue>(TKey key);
}