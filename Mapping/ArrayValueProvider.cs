namespace Internals.Mapping
{
    interface ArrayValueProvider
    {
        bool TryGetValue(int index, out object value);
        bool TryGetValue<T>(int index, out T value);
    }
}