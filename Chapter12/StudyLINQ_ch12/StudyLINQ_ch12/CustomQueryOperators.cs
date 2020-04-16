using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyLINQ_ch12
{
    public static class CustomQueryOperators
    {
        public static Decimal TotalPrice(this IEnumerable<Book> books)
        {
            if (books == null)
                throw new ArgumentNullException("books");

            Decimal result = 0;
            foreach (Book book in books)
                if (book != null)
                    result += book.Price;
            return result;
        }

        public static Book Min(this IEnumerable<Book> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            Book result = null;
            foreach (Book book in source)
            {
                if ((result == null) || (book.PageCount < result.PageCount))
                    result = book;
            }
            return result;
        }

        static public IEnumerable<Book> Books(this Publisher publisher, IEnumerable<Book> books)
        {
            return books.Where(book => book.Publisher == publihser);
        }

        public static Boolean IsExpensive(this Book book)
        {
            if (book == null)
                throw new ArgumentNullException("book");

            return (book.Price > 50) ||
                    ((book.Price / book.PageCount) > 0.10M);
            //책이 비싸다 = 절대적인 가격이 높거나 페이지 수에 비해 상대적으로 가격이 높은 경우임
        }
    }
}
