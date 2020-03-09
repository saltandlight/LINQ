using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace StudyLINQ_ch2
{
    class LanguageFeatures
    {
        static void DisplayPrcoesses()
        {
            List<String> processes = new List<String>();
            foreach (Process process in Process.GetProcesses())
            {
                processes.Add(process.ProcessName);
                Console.WriteLine(process.ProcessName);
            }

            
        }
        static void Main(string[] args)
        {
            DisplayPrcoesses();
            Console.ReadKey();
        }
    }
}
