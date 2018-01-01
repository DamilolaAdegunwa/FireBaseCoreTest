using System.Collections.Generic;

namespace FireSharp
{
    public static class Extentions
    {
        public static List<T> ToList<T>(this IReadOnlyCollection<T> collection)
        {
            var list = new List<T>();
            list.AddRange(collection);
            return list;
        }
    }
}
