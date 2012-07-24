namespace Internals.Caching
{
    delegate TKey KeySelector<TKey, TValue>(TValue value);
}