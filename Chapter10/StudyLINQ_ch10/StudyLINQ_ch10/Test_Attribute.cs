using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace StudyLINQ_ch10
{
    class Test_Attribute
    {
        static public void Main()
        {
            XElement root = XElement.Load("categorizedBooks.xml");
            XElement dotNetCategory = root.Element("category");
            XAttribute name = dotNetCategory.Attribute("name");

            Console.WriteLine((string)name);
            Console.ReadKey();
        }
    }
}
