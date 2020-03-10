using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace StudyLINQ_ch2
{
    class LanguageFeatures5
    {
        class ProcessData
        {
            public Int32 Id { get; set; }
            public Int64 Memory { get; set; }
            public String Name { get; set; }

            public override string ToString()
            {
                return "Id=" + Id + ",  Name=" + Name + ",  Memory=" + Memory;
            }
        }

        static void DisplayPrcoesses()
        {
            var processes = new List<ProcessData>();
            foreach (var process in Process.GetProcesses())
            {
                processes.Add(new ProcessData { Id = process.Id, Name = process.ProcessName, Memory = process.WorkingSet64 });
                System.Console.WriteLine(process.ToString());
            }

        }
        static void Main(string[] args)
        {
            DisplayPrcoesses();
            Console.ReadKey();
        }
    }
}
