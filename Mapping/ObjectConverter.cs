namespace Internals.Mapping
{
    interface ObjectConverter
    {
        object GetObject(ObjectValueProvider valueProvider);
    }
}