using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace StudyLINQ_ch2
{
    class LlanguageFeatures6
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

        static void DisplayPrcoesses(Predicate<Process> match)
        {
            var processes = new List<ProcessData>();
            foreach (var process in Process.GetProcesses())
            {
                if (match(process))
                {
                    processes.Add(new ProcessData { Id = process.Id, Name = process.ProcessName, Memory = process.WorkingSet64 });
                    System.Console.WriteLine(process.ToString());
                }
            }
        }

        static Boolean Filter(Process process)
        {
            return process.WorkingSet64 >= 20 * 1024 * 1024;
        }

        static void Main(string[] args)
        {
            DisplayPrcoesses(Filter);
            Console.ReadKey();
        }
        
    }
}
