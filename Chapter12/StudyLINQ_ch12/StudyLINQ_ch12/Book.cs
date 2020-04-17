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

        public string PrintString()
        {
            return string.Format("Id:{0}, Isbn:{1}, Notes:{2}, PageCount:{3}, Price:{4}, Summary:{5}, PublicationDate:{6}, Title:{7}, Subject: {8}, Publisher: {9}, Authors: {10}\n",
                                  this.BookId, this.Isbn, this.Notes, this.PageCount, this.Price, this.Summary, this.PublicationDate, this.Title, 
                                  this.Subject, this.Publisher.toString(), this.Authors.ToString());
        }
    }
}