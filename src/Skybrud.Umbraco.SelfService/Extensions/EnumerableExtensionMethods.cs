using System;
using System.Collections.Generic;
using System.Linq;
using Skybrud.Umbraco.SelfService.Enums;
using Skybrud.Umbraco.SelfService.Models.ActionPages;

namespace Skybrud.Umbraco.SelfService.Extensions {

    public static class EnumerableExtensionMethods {

        public static IOrderedEnumerable<T> OrderBy<T, TKey>(this IEnumerable<T> collection, Func<T, TKey> func, bool reverse) {
            return reverse ? collection.OrderByDescending(func) : collection.OrderBy(func);
        }

        public static IOrderedEnumerable<T> OrderBy<T, TKey>(this IEnumerable<T> collection, Func<T, TKey> func, SelfServiceSortOrder order) {
            return order == SelfServiceSortOrder.Descending ? collection.OrderByDescending(func) : collection.OrderBy(func);
        }

    }

}