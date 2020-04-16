using System;
using System.Collections.Generic;

namespace StudyLINQ_ch12
{
    public class Book
    {
        public Guid BookId { get; set; }
        public string Isbn { get; set; }
        public string Notes { get; set; }
        public Int32 PageCount { get; set; }
        public Decimal Price { get; set; }
        public string Summary { get; set; }
        public DateTime PublicationDate { get; set; }
        public string Title { get; set; }
        public string Subject { get; set; }
        public Publisher Publisher { get; set; }
        public Author[] Authors { get; set; }
    }
}