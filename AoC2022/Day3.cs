using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoC2016.Common;
namespace AoC2022
{
    public class Day3 : DayBase
    {
        public Day3() : base(3)
        {
            Input("example")
                .RunPart(1, 157)
                .RunPart(2, 70)
            .Input("output")
                .RunPart(1, 8349)
                .RunPart(2, 2681);
        }

        public override object Part1(Input input)
        {
            var sum = 0;
            foreach(var line in input.Lines){
                var comp1 = line.Substring(0, line.Length/2);
                var comp2 = line.Substring(line.Length/2);

                var ch = GetShared(comp1, comp2);

                sum += (ch >= 'a' && ch <= 'z')? ch - 'a'+1: ch-'A'+27;
            }

            return sum;
        }

        public override object Part2(Input input)
        {
            var sum = 0;
            var iter = input.Lines.GetEnumerator();

            while (iter.MoveNext()){
                var elf1 = (string) iter.Current;
                iter.MoveNext();
                var elf2 = (string) iter.Current;
                iter.MoveNext();
                var elf3 = (string) iter.Current;

                var ch = GetBadge(elf1, elf2, elf3);

                sum += (ch >= 'a' && ch <= 'z')? ch - 'a'+1: ch-'A'+27;
            }

            return sum;
        }

        private char GetShared(string comp1, string comp2)
        {
            foreach (var ch in comp1)
            {
                if (comp2.Contains(ch))
                {
                    return ch;
                }
            }
            return '?';
        }
        private char GetBadge(string elf1, string elf2, string elf3)
        {
            foreach (var ch in elf1)
            {
                if (elf2.Contains(ch) && elf3.Contains(ch))
                {
                    return ch;
                }
            }
            return '?';
        }
    }
}
namespace AoC2016.Days.Day2{
}
