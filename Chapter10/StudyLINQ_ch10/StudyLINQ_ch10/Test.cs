using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

namespace StudyLINQ_ch10
{
    class Test
    {
        static public void printAttribute()
        {
            XElement root = XElement.Load("categorizedBooks.xml");
            XElement dotNetCategory = root.Element("category");
            XAttribute name = dotNetCategory.Attribute("name");

            Console.WriteLine((string)name);
        }
        static public void printElement()
        {
            XElement root = XElement.Load("categorizedBooks.xml");
            XElement dotNetCategory = root.Element("category");
            Console.WriteLine(dotNetCategory);
        }

        static public void printElements()
        {
            XElement root = XElement.Load("categorizedBooks.xml");
            XElement dotNetCategory = root.Element("category");
            XAttribute name = dotNetCategory.Attribute("name");

            XElement books = dotNetCategory.Element("books");
            IEnumerable<XElement> bookElements = books.Elements("book");

            Console.WriteLine((string)name);
            foreach (XElement bookElement in bookElements)
            {
                Console.WriteLine(" - " + (string)bookElement);
            }
        }

        static public void printDecendants()
        {
            XElement books = XElement.Load("categorizedBooks.xml");
            foreach (XElement bookElement in books.Descendants("book"))
            {
                Console.WriteLine((string)bookElement);
            }
        }

        static public void compareDescendants_DescendantsAndSelf()
        {
            XElement root = XElement.Load("categorizedBooks.xml");
            IEnumerable<XElement> categories = root.Descendants("category");

            Console.WriteLine("Descendants");
            foreach (XElement categoryElement in categories)
            {
                Console.WriteLine(" - "+(string)categoryElement.Attribute("name"));
            }

            categories = root.DescendantsAndSelf("category");
            Console.WriteLine("DescendantsAndSelf");
            foreach (XElement categoryElement in categories)
            {
                Console.WriteLine(" - "+(string)categoryElement.Attribute("name"));
            }
        }

        static public void useLINQ()
        {
            XElement root = XElement.Load("categorizedBooks.xml");
            var books = from book in root.Descendants("book")
                        select (string)book;

            foreach (string book in books)
            {
                Console.WriteLine(book);
            }
        }

        static public void useAncestors()
        {
            XElement root = XElement.Load("categorizedBooks.xml");
            XElement dddBook = root.Descendants("book")
                                    .Where(book =>
                                      (string)book == "Domain Driven Design"
                                      ).First();

            IEnumerable<XElement> ancestors = dddBook.Ancestors("category").Reverse();

            string categoryPath =
                String.Join("/", ancestors.Select(e =>
                (string)e.Attribute("name")).ToArray());

            Console.WriteLine((string)dddBook + " is in the: "+categoryPath+" category.");
        }

        static public void useElementsBeforeSelf()
        {
            XElement root = XElement.Load("categorizedBooks.xml");
            XElement Book = root.Descendants("book")
                                    .Where(book =>
                                      (string)book == "Domain Driven Design"
                                      ).First();

            IEnumerable<XElement> beforeSelf = Book.ElementsBeforeSelf();
            foreach (XElement element in beforeSelf)
            {
                Console.WriteLine((string) element);
            }
        }

        static public void useXPath()
        {
            XElement root = XElement.Load("categorizedBooks.xml");
            var books = from book in root.XPathSelectElements("//book")
                        select book;

            foreach (XElement book in books)
            {
                Console.WriteLine((string)book);
            }
        }
        static public void Main()
        {
            useXPath();
            Console.ReadKey();
        }
    }
}
