using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfMagic.Extensions
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

        public static void AddRange<T>(this IList<T> coll, IEnumerable<T> items)
        {
            if (coll != null && !items.IsNullOrEmpty())
                items.ForEach(t => coll.Add(t));
        }
    }
}
