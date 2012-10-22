namespace System.Collections.Generic
{
    using System;
    using Threading.Tasks;


    public static partial class ListExtensions
    {
#if NETFX_CORE
        public static void ForEach<T>(this List<T> list, Action<T> action)
        {
            foreach (var item in list)
                action(item);
        }
#endif

        public static async Task ForEachAsync<T>(this List<T> list, Func<T, Task> callback)
        {
            foreach (var item in list)
                await callback(item);
        }
    }
}