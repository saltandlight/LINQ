using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace StudyLINQ_ch2
{
    static class LlanguageFeatures8
    {
        public static object ObjectDumper { get; private set; }

        internal class ProcessData
        {
            public Int32 Id { get; set; }
            public Int64 Memory { get; set; }
            public String Name { get; set; }
        }
        static void DisplayPrcoesses(Predicate<Process> match)
        {
            var processes = new List<ProcessData>();
            foreach (var process in Process.GetProcesses())
            {
                if (match(process))
                {
                    processes.Add(new ProcessData
                    {
                        Id = process.Id,
                        Name = process.ProcessName,
                        Memory = process.WorkingSet64
                    });
                }
            }
            Console.WriteLine("Total memory: {0} MB", processes.TotalMemory() / 1024 / 1024);

            var top2Memory =
                processes
                .OrderByDescending(process => process.Memory)
                .Take(2)
                .Sum(process => process.Memory) / 1024 / 1024;
            Console.WriteLine(
                "Memory consumed by the two most hungry processes: {0} MB", top2Memory);

            var results = new
            {
                TotalMemory = processes.TotalMemory() / 1024 / 1024,
                Top2Memory = top2Memory,
                Processes = processes
            };
            ObjectDumper.Write(results, 1);
            ObjectDumper.Write(processes);
        }0

        static Int64 TotalMemory(this IEnumerable<ProcessData> processes)
        {
            Int64 result = 0;

            foreach (var process in processes)
                result += process.Memory;

            return result;
        }

        static void Main(string[] args)
        {
            
            Console.ReadKey();
        }
    }
}
