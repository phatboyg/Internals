namespace Internals.Mapping
{
    interface ObjectMapper<in T>
    {
        void ApplyTo(T obj, ObjectValueProvider valueProvider);
    }
}