using AoC.Base;
using AoC.Base.TestInputs;
using AoC.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC2022
{
    public class Day22 : AbstractDay<int, IComparableInput<int>>
    {
        public override void PrepateTests(InputBuilder<int, IComparableInput<int>> builder)
        {
            builder.New("example1", "./Inputs/Day22/example1.txt")
                .Part1(6032);
            //builder.New("output", "./Inputs/Day22/output.txt")
            //    .Part2(0); //75290 Too High
        }

        public override int Part1(IComparableInput<int> input)
        {
            var map = new Map(input);
            
            return 0;
        }

        public override int Part2(IComparableInput<int> input)
        {
            throw new NotImplementedException();
        }

        private class Map{
            public readonly int Height = 0;
            public readonly int Width = 0;
            private IList<string> Data = new List<string>();

            public Map(IComparableInput<int> input){
                var t = input.ReadLines();
                t.Wait();

                foreach(var line in t.Result){
                    if(string.IsNullOrEmpty(line))
                        break;
                    Data.Add(line);
                    Height++;
                    if(Width < line.Length)
                        Width = line.Length;
                }
            }
        }

        [Flags]
        private enum Content{
            Empty = 0,
            Void = 1,
            Rock = 2,
            Left = 4,
            Right = 8,
            Up = 16,
            Down = 32            
        }
    }
}
