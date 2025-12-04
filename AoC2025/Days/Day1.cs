using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoCBase2;

namespace AoC2025.Days
{
    internal class Day1 : IDay
    {
        public static void RunAoC()
        {
            AocRuntime.Day<Day1>(1)
                .Callback(1, (d, t) => d.Part1(t.GetLines()))
                .Callback(2, (d, t) => d.Part2(t.GetLines()))
                .Test("example", "Inputs/Day1/example1.txt")
                    //.Part(1)
                    //.Part(2)
                .Test("input", "Inputs/Day1/input.txt")
                    //.Part(1)
                    //.Part(2)
                .Run();
        }

        private string Part1(IEnumerable<string> lines)
        {
            var dial = new Dial(50);
            var ctr = 0;
            foreach (var line in lines)
                if (dial.Rotate(line) == 0)
                    ctr++;
            return ctr.ToString();
        }

        public string Part2(IEnumerable<string> lines)
        {
            var dial = new Dial(50);
            foreach (var line in lines)
                dial.Rotate(line);
            return dial.Zeroes.ToString();
        }

        private class Dial
        {
            public int State { get; private set; }
            public int Zeroes = 0;

            public Dial(int State)
            {
                this.State = State;
            }

            public int Rotate(string descr)
            {
                var dir = descr[0];
                var dist = int.Parse(descr.Substring(1));

                if (dir == 'L')
                    for (var i = 0; i < dist; i++)
                        RotL();
                else
                    for (var i = 0; i < dist; i++)
                        RotR();
                return State;
            }

            private void RotL()
            {
                State--;
                if (State == 0) Zeroes++;
                if (State == -1) State = 99;
            }
            private void RotR()
            {
                State = (State + 1) % 100;
                if (State == 0) Zeroes++;
            }
        }
    }
}
