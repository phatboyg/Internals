namespace Internals.Caching
{
    delegate TValue MissingValueProvider<in TKey, out TValue>(TKey key);
}