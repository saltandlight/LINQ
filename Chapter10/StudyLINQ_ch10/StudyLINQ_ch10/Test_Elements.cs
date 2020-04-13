using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace StudyLINQ_ch10
{
    class Test_Elements
    {
        static public void Main()
        {
            XElement root = XElement.Load("categorizedBooks.xml");
            XElement dotNetCategory = root.Element("category");
            XAttribute name = dotNetCategory.Attribute("name");

            XElement books = dotNetCategory.Element("books");
            IEnumerable<XElement> bookElements = books.Elements("book");

            Console.WriteLine((string) name);
            foreach(XElement bookElement in bookElements)
            {
                Console.WriteLine(" - "+(string)bookElement);
            }
            Console.ReadKey();
        }
    }
}
