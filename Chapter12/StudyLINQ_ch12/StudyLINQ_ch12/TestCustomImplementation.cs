using System;

namespace StudyLINQ_ch12
{
    static class TestCustomImplementation
    {
        static void Main()
        {
            var books =
                from book in SampleData.Books
                where book.Price < 30
                select book.Title;

            ObjectDumper.Write(books);
        }
    }
}
