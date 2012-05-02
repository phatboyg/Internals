namespace Internals.Caching
{
    delegate TKey KeySelector<out TKey, in TValue>(TValue value);
}