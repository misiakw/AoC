using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AoC.Base;

namespace AoC2022
{
    public class Day17 : LegacyDayBase
    {

        public override void PrepateTests(InputBuilder<int, IComparableInput<int>> builder)
        {
            builder.New("example1", "./Inputs/Day17/example1.txt")
                 .Part1(3068);
            builder.New("output", "./Inputs/Day17/output.txt")
                .Part1(3124);
        }

        public override object Part1(LegacyInput input)
        {
            throw new NotImplementedException();
        }

        public override object Part2(LegacyInput input)
        {
            throw new NotImplementedException();
        }

        private abstract class Block
        {
            public int X;
            public int Y;

        }
    }
}
