using System;
using System.Reflection;

namespace AoC.Base
{
    public class Launcher2
    {
        public void Run(int? day = null){
            var assembly = Assembly.GetExecutingAssembly();
            Console.WriteLine($"run day {day}");
        }
    }
}
