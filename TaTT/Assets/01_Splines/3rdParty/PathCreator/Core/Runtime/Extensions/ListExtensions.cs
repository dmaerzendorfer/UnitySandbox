using System.Collections.Generic;
using System.Linq;

namespace _3rdParty.PathCreator.Core.Runtime.Extensions
{
    public static class ListExtensions
    {
        //    list: List<T> to resize
        //    size: desired new size
        // element: default value to insert
        /// <summary>
        /// resizes a given list to a given size. keeps elements, for new ones inserts element.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="size"></param>
        /// <param name="element"></param>
        /// <typeparam name="T"></typeparam>
        public static void Resize<T>(this List<T> list, int size, T element = default(T))
        {
            int count = list.Count;

            if (size < count)
            {
                list.RemoveRange(size, count - size);
            }
            else if (size > count)
            {
                if (size > list.Capacity)   // Optimization
                    list.Capacity = size;

                list.AddRange(Enumerable.Repeat(element, size - count));
            }
        }
    }
}