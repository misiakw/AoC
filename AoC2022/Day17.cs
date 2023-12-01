using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AoC.Base;
using AoC.Base.TestInputs;

namespace AoC2022
{
    public class Day17 : AbstractDay<int, IComparableInput<int>>
    {

        public override void PrepateTests(InputBuilder<int, IComparableInput<int>> builder)
        {
            builder.New("example1", "./Inputs/Day17/example1.txt")
                 .Part1(3068);
            builder.New("output", "./Inputs/Day17/output.txt")
                .Part1(3124);
        }

        //szer miejsca 7
        //klocek startuje 2 od lewej, 3 od najnizszego

        public override int Part1(IComparableInput<int> input)
        {
            return 5;
        }

        public override int Part2(IComparableInput<int> input)
        {
            throw new NotImplementedException();
        }

    }
}
