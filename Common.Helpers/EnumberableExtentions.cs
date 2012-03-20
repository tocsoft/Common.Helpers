using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Helpers
{
    public static class EnumberableExtentions
    {
        public static IEnumerable<T> Do<T>(this IEnumerable<T> lst, Action<T> action) {
            foreach (var itm in lst) {
                action(itm);
            }

            return lst;
        }

        public static bool ContainsAny<T>(this IEnumerable<T> lst, IEnumerable<T> toFind)
        {
            if (lst == null)
                return false;

            return lst.Where(x => toFind.Contains(x)).Any();
        }
    }
}