namespace Internals.Mapping
{
    interface ObjectValueProvider
    {
        bool TryGetValue(string name, out object value);
        bool TryGetValue<T>(string name, out T value);
    }
}