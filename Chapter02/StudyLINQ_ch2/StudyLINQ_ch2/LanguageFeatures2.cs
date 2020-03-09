using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace StudyLINQ_ch2
{
    class LanguageFeatures2
    {
        class ProcessData
        {
            public Int32 Id;
            public Int64 Memory;
            public String Name;

            public override string ToString()
            {
               return "Id=" + Id + ",  Name=" + Name + ",  Memory=" + Memory;
            }
        }

        static void DisplayPrcoesses()
        {
            List<ProcessData> processes = new List<ProcessData>();
            foreach (Process process in Process.GetProcesses())
            {
                ProcessData data = new ProcessData();
                data.Id = process.Id;
                data.Name = process.ProcessName;
                data.Memory = process.WorkingSet64;
                processes.Add(data);
                Console.WriteLine(data.ToString());
            }

        }
        static void Main(string[] args)
        {
            DisplayPrcoesses();
            Console.ReadKey();
        }
    }
}
