using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AdventOfCode.Attributes;
using AdventOfCode.Day1;
using AdventOfCode.Interfaces;

namespace AdventOfCode
{
    class Program
    {
        static async Task Main(string[] args)
        {

            var days = new Dictionary<int, Tuple<string,Type>>();

            Assembly mscorlib = typeof(Program).Assembly;
            var dayNum = 1;
            foreach (Type type in mscorlib.GetTypes().Where(t => t.CustomAttributes.Any(c => c.AttributeType == typeof(DayAttribute))).OrderBy(t => t.Name))
            {
                var attribute = type.GetCustomAttribute(typeof(DayAttribute)) as DayAttribute;

                days.Add(dayNum++, new Tuple<string, Type>(attribute.Name, type));
            }


            Type SelectedDayType = null;
            do
            {
                foreach (var num in days)
                {
                    Console.WriteLine($"{num.Key}) {num.Value.Item1}");
                }
                Console.WriteLine("Select Day");

                if (int.TryParse( Console.ReadLine()?.Trim(), out var selection) && days.ContainsKey(selection))
                {
                    SelectedDayType = days[selection].Item2;

                    if (SelectedDayType.CustomAttributes.Any(a => a.AttributeType == typeof(InputAttribute))) continue;
                    Console.WriteLine("Day does not have input attached");
                    SelectedDayType = null;
                }
            } while (SelectedDayType == null);

            var inputAttributes = SelectedDayType.GetCustomAttributes(typeof(InputAttribute)).ToList();
            var inputs = new Dictionary<int, InputAttribute>();
            for (var i = 0; i < inputAttributes.Count(); i++)
                inputs.Add(i, (InputAttribute) inputAttributes[i]);


            IDictionary<string, IInput> SelectedInputs = new Dictionary<string, IInput>();
            if (inputs.Count() == 1)
            {
                var first = inputs.First();
                SelectedInputs.Add(first.Value.Name, first.Value.Input);
            }
            while (!SelectedInputs.Any())
            {

                foreach (var input in inputs)
                {
                    Console.WriteLine($"{input.Key}) {input.Value.Name}");
                }

                Console.WriteLine("Select input:");
                foreach(var input in Console.ReadLine().Split(" "))
                {
                    if (int.TryParse(input, out var selectedInput) &&
                    inputs.ContainsKey(selectedInput))
                    {
                        SelectedInputs.Add(inputs[selectedInput].Name, inputs[selectedInput].Input);
                    }
                }

            }


            foreach (var input in SelectedInputs)
            {
                Console.WriteLine($"Input set: {input.Key}");
                var day = (IDay)Activator.CreateInstance(SelectedDayType);

                try
                {
                    var result1 = day.Task1(input.Value);
                    Console.WriteLine($"Task1: {result1}");
                }
                catch(NotImplementedException) { }

                try
                {
                    var result2 = day.Task2(input.Value);
                    Console.WriteLine($"Task2: {result2}");
                }
                catch(NotImplementedException) { }
            }
        }
    }
}
