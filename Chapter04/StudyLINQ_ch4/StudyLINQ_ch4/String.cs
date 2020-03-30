using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyLINQ_ch4
{
    public static class String
    {
        static void Main()
        {
            var count =
                "Non-letter characters in this string: 8"
                .Where(c => !Char.IsLetter(c))
                .Count();

            Console.Write(count);
            Console.ReadKey();
        }
    }
}
