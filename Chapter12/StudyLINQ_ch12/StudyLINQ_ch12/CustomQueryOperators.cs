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
    }
}
