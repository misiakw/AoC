using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AoC.Base;
using AoC2022.Days.Day25;

namespace AoC2022
{
    public class Day25 : Day<SafuNum>
    {
        public Day25() : base(25)
        {
            Input("sequence")
            //.RunPart(1, "2=-1=0")
            .Input("example1")
                .RunPart(1, "2=-1=0")
            .Input("output");
        }

        public override SafuNum Parse(string val) => SafuNum.Parse(val);

        public override object Part1(IList<SafuNum> data, Input input)
        {
            var sum = SafuNum.Parse("0");
            foreach (var safu in data)
            {
                sum = sum + safu;
                Console.WriteLine(safu);
            }

            return sum.ToString();
        }

        public override object Part2(IList<SafuNum> data, Input input)
        {
            throw new NotImplementedException();
        }

        public override IList<string> Split(string val)
            => val.Split("\n", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                .ToList();
    }
}

namespace AoC2022.Days.Day25
{
    public class SafuNum
    {
        protected short[] _value;
        protected SafuNum(short[] val)
        {
            _value = val;
        }

        public static SafuNum Parse(string val)
        {
            var sum = new List<short>();
            for (var i = val.Length - 1; i >= 0; i--)
            {
                short tmp = 0;
                switch (val[i])
                {
                    case '=':
                        tmp = -2;
                        break;
                    case '-':
                        tmp = -1;
                        break;
                    default:
                        tmp = (short)(val[i] - '0');
                        break;
                }
                sum.Add(tmp);
            }

            return new SafuNum(sum.ToArray());
        }

        public override string ToString()
        {
            var result = string.Empty;
            foreach (var l in _value)
                result += l >= 0 ? l.ToString() : l == -1 ? '-' : '=';

            return result;
        }

        public static SafuNum operator +(SafuNum A, SafuNum B)
        {
            return A;
        }
    }
}