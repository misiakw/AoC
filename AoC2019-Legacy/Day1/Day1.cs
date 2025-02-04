using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;

namespace AdventOfCode.Day1
{
    [Day("Day1")]
    [Input("target", typeof(Day1Input))]
    public class Day1: IDay
    {
        public string Task1(IInput input)
        {
            var fuelWeight = new List<long>();
            var lines = input.Input.Split("\n");            

            foreach (var line in lines.Select(x => x.Trim()))
            {
                var weight = long.Parse(line);
                fuelWeight.Add(GetFuel(weight));
            }


            return fuelWeight.Sum().ToString();
        }

        public string Task2(IInput input)
        {
            var fuelWeight = new List<long>();
            var lines = input.Input.Split("\n");

            foreach (var line in lines.Select(x => x.Trim()))
            {
                var weight = long.Parse(line);

                do
                {
                    var calcFuel = GetFuel(weight);
                    if (calcFuel > 0)
                        fuelWeight.Add(calcFuel);
                    weight = calcFuel;
                } while (weight > 0);
            }

            return fuelWeight.Sum().ToString();
        }

        private long GetFuel(long weight)
        {
            return (long) (Math.Floor((decimal) weight / 3) - (decimal) 2);
        }
    }
}