namespace System.Collections.Generic
{
    using System;
    using System.Collections.Generic;

#if NETFX_CORE
    public static class ListExtensions
    {
         public static void ForEach<T>(this List<T> list, Action<T> action)
         {
             foreach (var item in list)
                 action(item);
         }
    }
#endif
}