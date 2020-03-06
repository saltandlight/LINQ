using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace StudyLINQ_ch1
{
    class HelloLinqToSql
    {
        [Table(Name = "Contacts")]
        class Contact
        {
            [Column(IsPrimaryKey=true)]
            public int ContactID { get; set; }
            [Column(Name="ContactName")]
            public string Name { get; set; }
            [Column]
            public string City { get; set; }

        }

        static void Main()
        {
            string path =
                System.IO.Path.GetFullPath(@"..\..\..\..\Data\northwnd.mdf");
            DataContext db = new DataContext(path);
            var contacts =
                from contact in db.GetTable<Contact>()
                where contact.City == "Paris"
                select contact;

            foreach (var contact in contacts)
            {
                Console.WriteLine("Bonjour " + contact.Name);
            }
         }
    }
}
