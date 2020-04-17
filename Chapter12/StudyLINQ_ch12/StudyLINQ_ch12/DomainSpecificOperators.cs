using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyLINQ_ch12
{
    static class DomainSpecificOperators
    {
        public static IEnumerable<Book> Where(
            this IEnumerable<Book> source,
            Func<Book, Boolean> predicate)
        {
            foreach (Book book in source)
            {
                Console.WriteLine(
                    "processing book \"{0}\" in "+
                    "DomainSpecificOperators.Where",
                    book.Title);
                if (predicate(book))
                    yield return book;
            }
        }

        public static IEnumerable<TResult> Select<TResult>(
            this IEnumerable<Book> source, Func<Book, TResult> selector)
        {
            foreach (Book book in source)
            {
                Console.WriteLine(
                    "processing book \"{0}\" in " +
                    "DomainSpecificOperators.Select<TResult>",
                    book.Title);
                yield return selector(book);
            }
        }

        public static IEnumerable<TResult>
            GroupJoin<TOuter, TInner, TKey, TResult>(
            this TOuter outer,
            IEnumerable<TInner> inner,
            Func<TOuter, TKey> outerKeySelector,
            Func<TOuter, TKey> innerKeySelector,
            Func<TOuter, IEnumerable<TInner>, TResult> resultSelector)
        {
            ILookup<TKey, TInner> lookup =
                inner.ToLookup(innerKeySelector);
            yield return resultSelector(outer,
                                        lookup[outerKeySelector(outer)]);
        }

        static public void Main()
        {
            var books =
                from book in SampleData.Books
                where book.Price < 30
                select book.Title;

            foreach (string book in books)
            {
                Console.WriteLine(book);
            }

            Console.ReadKey();
        }
    }
}
