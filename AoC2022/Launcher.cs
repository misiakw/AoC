using System;
using System.Reflection;

namespace AoC.Base
{
    public class Launcher
    {
        public void Run(int? dayNum = null)
        {
            var days = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.BaseType == typeof(DayBase));

            if (dayNum.HasValue)
            {
                var type = days.FirstOrDefault(d => d.Name == $"Day{dayNum}");
                if (type != null)
                {
                    RunDay(type);
                    return;
                }


            }

            foreach (var day in days)
            {
                Console.WriteLine(day.FullName);
            }

            Console.WriteLine($"run day {dayNum}");
        }

        public void RunDay<T>() where T : DayBase
        {
            DayBase day = (DayBase)Activator.CreateInstance<T>();
            day.Execute();
        }
        private void RunDay(Type t)
        {
            MethodInfo method = typeof(Launcher).GetMethod(nameof(Launcher.RunDay));
            MethodInfo generic = method.MakeGenericMethod(t);
            generic.Invoke(this, null);
        }
    }
}
