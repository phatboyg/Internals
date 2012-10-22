#if NETFX_CORE
namespace System.Collections.Generic
{
    using System;

    static class ListExtensions
    {
         public static void ForEach<T>(this List<T> list, Action<T> action)
         {
             foreach (var item in list)
                 action(item);
         }
    }
}
#endif
