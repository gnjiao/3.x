using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Collections
{
    public static class CollectionExtension
    {
        // ******************************************************************
        public static ICollection<T> RemoveAll<T>(this ICollection<T> coll, Func<T, bool> condition)
        {
            var itemsToRemove = coll.Where(condition).ToList();

            foreach (var itemToRemove in itemsToRemove)
            {
                coll.Remove(itemToRemove);
            }

            return coll;
        }

        // ******************************************************************
        public static ICollection<T> RemoveAll<T>(this ICollection<T> coll)
        {
            RemoveAll(coll, (x) => true);
            return coll;
        }

        // ******************************************************************
        public static void AddMany<T>(this ICollection<T> coll, IEnumerable<T> enumT)
        {
            foreach(T t in enumT)
            {
                coll.Add(t);
            }
        }

        // ******************************************************************

    }
}
