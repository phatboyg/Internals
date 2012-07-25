namespace Internals.Reflection
{
    using System;

    /// <summary>
    /// Type factory
    /// </summary>
    /// <typeparam name="T">The factory component type</typeparam>
    static class Factory<T>
    {
        /// <summary>
        /// The factory method to get an instance of the component type
        /// </summary>
        internal static Func<T> Get { get; set; }
    }
}