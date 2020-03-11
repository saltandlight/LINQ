using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace StudyLINQ_ch2
{
    class LlanguageFeatures7
    {
        
        static void DisplayPrcoesses(Predicate<Process> match)
        {
            var processes = new List<Object>();
            foreach (var process in Process.GetProcesses())
            {
                if (match(process))
                {
                    processes.Add(new {
                        process.Id,
                        process.ProcessName,
                        process.WorkingSet64 });
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
