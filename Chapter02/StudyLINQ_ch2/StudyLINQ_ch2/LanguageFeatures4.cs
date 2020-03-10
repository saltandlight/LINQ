using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace StudyLINQ_ch2
{
    class LanguageFeatures4
    {
        class ProcessData
        {
            private string processName;
            private long workingSet64;

            public ProcessData(int id, string processName, long workingSet64)
            {
                Id = id;
                this.processName = processName;
                this.workingSet64 = workingSet64;
            }

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
                processes.Add( new ProcessData(process.Id, process.ProcessName, process.WorkingSet64));
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
