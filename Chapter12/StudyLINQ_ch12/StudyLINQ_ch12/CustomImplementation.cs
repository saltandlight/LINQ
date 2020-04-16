using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyLINQ_ch12
{
    public static class CustomImplementation
    {
        public static IEnumerable<TSource> Where<TSource>(
            this IEnumerable<TSource> source, Func<TSource, Boolean> predicate)
        {
            Console.WriteLine("in CustomImplementation.Where<TSource>");
            return Enumerable.Where(source, predicate);
        }

        public static IEnumerable<TResult> Select<TSource, TResult>(
            this IEnumerable<TSource> source,
            Func<TSource, TResult> selector)
        {
            Console.WriteLine(
                "in CustomImplementation.Select<TSource, TResult>");
            return Enumerable.Select(source, selector);
        }
    }
}
