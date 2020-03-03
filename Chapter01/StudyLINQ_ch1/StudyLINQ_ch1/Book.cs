using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace StudyLINQ_ch1
{
    class Book
    {
        public string Publisher;
        public string Title;
        public int Year;

        public Book(string title, string publisher, int year)
        {
            Title = title;
            Publisher = publisher;
            Year = year;
        }
    }

    static class HelloLinqToXml
    {
        static void Main()
        {
            Book[] books = new Book[] {
              new Book("Ajax in Action", "Manning", 2005),
              new Book("Windows Forms in Action", "Manning", 2006),
              new Book("RSS and Atom in Action", "Manning", 2006)
            };

            XmlDocument doc = new XmlDocument();
            XmlElement root = doc.CreateElement("books");
            foreach (Book book in books)
            {
                if (book.Year == 2006)
                {
                    XmlElement element = doc.CreateElement("book");
                    element.SetAttribute("title", book.Title);

                    XmlElement publisher = doc.CreateElement("publisher");
                    publisher.InnerText = book.Publisher;
                    element.AppendChild(publisher);

                    root.AppendChild(element);
                }
            }

            doc.AppendChild(root);

            doc.Save(Console.Out);
            Console.ReadKey();
        }
    }
}
