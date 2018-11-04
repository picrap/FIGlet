// A FIGlet generation library - MIT license
// https://github.com/picrap/FIGlet

namespace FIGlet.Utility
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Extensions to <see cref="IEnumerable{T}"/>
    /// </summary>
    public static  class EnumerableExtensions
    {
        /// <summary>
        /// Gets the index at which predicate matches condition
        /// </summary>
        /// <typeparam name="TItem">The type of the item.</typeparam>
        /// <param name="items">The items.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public static int? IndexOf<TItem>(this IEnumerable<TItem> items, Predicate<TItem> predicate)
        {
            var index = 0;
            foreach (var item in items)
            {
                if (predicate(item))
                    return index;
                index++;
            }

            return null;
        }
    }
}