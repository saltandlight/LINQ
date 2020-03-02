using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StudyLINQ_ch1
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] words =
                {"hello", "wonderful", "linq", "beautiful", "world" };

            var groups =
                from word in words
                orderby word ascending
                group word by word.Length into lengthGroups
                orderby lengthGroups.Key descending
                select new { Length = lengthGroups.Key, Words = lengthGroups };

            foreach (var group in groups)
            {
                Console.WriteLine("Words of length " + group.Length);
                foreach (string word in group.Words)
                {
                    Console.WriteLine(" " + word);
                }
            }

            Console.ReadKey();
        }
    }
}
