using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyLINQ_ch12
{
    public static class Enumerable
    {
        public static int Sum(this IEnumerable<int> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            int sum = 0;
            checked
            {
                foreach (int v in source)
                    sum += v;
            }
            return sum;
        }

        internal static IEnumerable<TSource> Where<TSource>(IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public static int? Sum(this IEnumerable<int?> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            int? sum = 0;
            checked
            {
                foreach (int? v in source)
                    if(v!=null)
                        sum += v;
            }
            return sum;
        }

        internal static IEnumerable<TResult> Select<TSource, TResult>(IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            throw new NotImplementedException();
        }

        public static int Sum<T>(this IEnumerable<T> source,
            Func<T, int> selector)
        {
            return Enumerable.Sum(Enumerable.Select(source, selector));
        }

        private static int? Sum<T>(this IEnumerable<T> source, Func<T, int?> selector)
        {
            return Enumerable.Sum(Enumerable.Select(source, selector));
        }

        private static IEnumerable<int> Select<T>(IEnumerable<T> source, Func<T, int> selector)
        {
            throw new NotImplementedException();
        }

        private static IEnumerable<int> Select<T>(IEnumerable<T> source, Func<T, int?> selector)
        {
            throw new NotImplementedException();
        }
    }
}
