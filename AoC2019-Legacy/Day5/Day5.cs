using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Day5
{
    [Day("Day5")]
    [Input("test", typeof(Day5TestInput))]
    [Input("aoc", typeof(Day5TAoCInput))]
    public class Day5 : IDay
    {
        public string Task1(IInput input)
        {
            var code = input.Input.Split("").Select(int.Parse).ToList() ;

            var prog = new Program(code);

            return "";
        }

        public string Task2(IInput input)
        {
            throw new NotImplementedException();
        }

        private class Program
        {
            private readonly IList<int> _code;
            private int _ptr = 0;

            public Program(IList<int> code)
            {
                _code = code;
            }

        }
    }
}
