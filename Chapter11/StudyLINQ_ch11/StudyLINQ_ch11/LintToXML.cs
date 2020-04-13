using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace StudyLINQ_ch11
{
    class LintToXML
    {
        static public void PrintXML()
        {
            XElement booksXml = XElement.Load("books.xml");
            IEnumerable<XElement> bookElements = booksXml.Elements("book");

            var books =
                from bookElement in booksXml.Elements("book")
                select new Book
                {
                    Title = (string)bookElement.Element("title"),
                    Publisher = new Publisher
                    {
                        Name = (string)bookElement.Element("publisher")
                    },
                    PublicationDate = (DateTime)bookElement.Element("publicationDate"),
                    Price = (decimal)bookElement.Element("price"),
                    Isbn = (string)bookElement.Element("isbn"),
                    Notes = (string)bookElement.Element("notes"),
                    Summary = (string)bookElement.Element("summary"),
                    Authors =
                                from authorElement in bookElement.Descendants("author")
                                select new Author
                                {
                                    FirstName = (string)authorElement.Element("firstName"),
                                    LastName = (string)authorElement.Element("lastName")
                                },
                    Reviews =
                                from reviewElement in bookElement.Descendants("review")
                                select new Review
                                {
                                    User = new User { Name = (string)reviewElement.Element("user") },
                                    Rating = (int)reviewElement.Element("rating"),
                                    Comments = (string)reviewElement.Element("comments")
                                }
                };

            foreach (Book book in books)
            {
                Console.WriteLine(book.toString());
            }
        }
        static public void Main(string[] args)
        {
            PrintXML();
            Console.ReadKey();
        }
    } 
}
