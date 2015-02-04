using System;
using System.Collections.Generic;
using System.Linq;

namespace BddSharp.Engine.Extensions
{
    public static class EnumerableExtensions
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> coll)
        {
            return coll == null || !coll.Any();
        }

        public static void ForEach<T>(this IEnumerable<T> coll, Action<T> action)
        {
            if (coll != null)
            {
                foreach (var t in coll)
                    action(t);
            }
        }
    }
}
