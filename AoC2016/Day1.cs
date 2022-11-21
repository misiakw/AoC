﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Step = AoC2016.Days.Day1.Step;

namespace AoC2016
{
    public class Day1 : Day<Step>
    {
        public Day1() : base(1, false)
        {
            Input("example1")
                .RunPart(1, 42)
                .RunPart(2)
            .Input("example2")
                .RunPart(1)
            .Input("example3")
            .Input("output");
        }

        public override Step Parse(string val) => new Step{
                Rot = val[0],
                Len = int.Parse(val.Substring(1))
            };

        public override IList<string> Split(string val) => val.Split(",");
        public override object Part1(IList<Step> data, Input input)
        {
            return 15;
        }


        public override object Part2(IList<Step> data, Input input)
        {
            return "Part 2";
        }

    }
}
namespace AoC2016.Days.Day1{
    public class Step{
        public char Rot;
        public int Len;
    }
}
