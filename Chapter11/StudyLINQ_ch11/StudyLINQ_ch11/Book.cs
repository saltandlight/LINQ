using System;
using System.Collections.Generic;

namespace StudyLINQ_ch11
{
    internal class Book
    {
        public string Title { get; set; }
        public Publisher Publisher { get; set; }
        public DateTime PublicationDate { get; set; }
        public decimal Price { get; set; }
        public string Isbn { get; set; }
        public string Notes { get; set; }
        public string Summary { get; set; }
        public IEnumerable<Author> Authors { get; set; }
        public IEnumerable<Review> Reviews { get; set; }

        public string toString()
        {
            return string.Format("title:{0}\npublisher:{1}\npublicationDate:{2}\nprice:{3}\n" +
                "isbn: {4}\nnotes: {5}\nsummary: {6}\nauthors: {7}\nreviews: {8}\n", Title, Publisher, PublicationDate, Price, Isbn, 
                Notes, Summary, Authors, Reviews);
        }
    }
}