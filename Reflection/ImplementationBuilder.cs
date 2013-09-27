namespace Internals.Reflection
{
    using System;


    public interface ImplementationBuilder
    {
        Type GetImplementationType(Type interfaceType);
    }
}