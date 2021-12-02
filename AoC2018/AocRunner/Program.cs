using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Shared;

namespace AocRunner
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        public static Dictionary<string, IDay> AvailableDays = new Dictionary<string, IDay>()
        {
            
            {"Day 4", new Day4.Day4() },
            {"Day 1", new Day1.Program() }
        };
    }
}
