﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyLINQ_ch4
{
    static class GenericDictionary
    {
        static void Main()
        {
            Dictionary<int, string> frenchNumbers;
            frenchNumbers = new Dictionary<int, string>();
            frenchNumbers.Add(0, "zero");
            frenchNumbers.Add(1, "un");
            frenchNumbers.Add(2, "deux");
            frenchNumbers.Add(3, "trois");
            frenchNumbers.Add(4, "quatre");

            var evenFrenchNumbers =
                from entry in frenchNumbers
                where (entry.Key % 2) == 0
                select entry.Value;

            foreach (var num in evenFrenchNumbers)
            {
                Console.WriteLine(num.ToString());
            }

            Console.ReadKey();
        }
    }
}
