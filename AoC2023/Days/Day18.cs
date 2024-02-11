using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AoC.Base;
using AoC.Base.TestInputs;
using AoC.Common;
using AoC.Common.Maps;

namespace AoC2023.Days
{
    public class Day18 : AbstractDay<long, IComparableInput<long>>
    {
        public override void PrepateTests(InputBuilder<long, IComparableInput<long>> builder)
        {
            builder.New("example1", "./Inputs/Day18/example1.txt")
                .Part1(62)
                .Part2(952408144115);
            builder.New("output", "./Inputs/Day18/output.txt")
                .Part1(39194)
                .Part2(0);
        }

        private IEnumerable<InpStruct> readInput1(IComparableInput<long> input){
            var regex = new Regex(@"(.) (\d+) \(\#(.*)\)");
            foreach(var line in ReadLines(input)){
                var match = regex.Match(line);
                yield return new InpStruct(){
                    dir = match.Groups[1].Value[0],
                    len = int.Parse(match.Groups[2].Value)
                };
            }
        }
        private IEnumerable<InpStruct> readInput2(IComparableInput<long> input){
            var regex = new Regex(@"(.) (\d+) \(\#(.*)\)");
            foreach(var line in ReadLines(input)){
                var match = regex.Match(line).Groups[3].Value;
                yield return new InpStruct(){
                    dir = match[5] switch{
                        '0' => 'R',
                        '1' => 'D',
                        '2' => 'L',
                        _ => 'U'
                    },
                    len = int.Parse(string.Join("", match.ToCharArray().Take(5)), System.Globalization.NumberStyles.HexNumber)
                };
            }
        }

        public override long Part1(IComparableInput<long> input)
        {
            var coords = Read(readInput1, input).ToArray();
            var ctr = 0l;
            foreach(var order in readInput1(input))
                ctr += order.len;

            var pow = 0l;
            for(var i=0; i < coords.Length-2; i++){
                pow += coords[i].Item1 * coords[i+1].Item2;
                pow -= coords[i].Item2 * coords[i+1].Item1;
            }

            return (Math.Abs(pow)+ctr)/2+1;
        }

        public override long Part2(IComparableInput<long> input)
        {
            var coords = Read(readInput2, input).ToArray();
            var ctr = 0l;
            foreach(var order in readInput2(input))
                ctr += order.len;

            var pow = 0l;
            for(var i=0; i < coords.Length-2; i++){
                pow += coords[i].Item1 * coords[i+1].Item2;
                pow -= coords[i].Item2 * coords[i+1].Item1;
            }

            return (Math.Abs(pow)+ctr)/2+1;
        }

        private IEnumerable<(long, long)> Read(Func<IComparableInput<long>, IEnumerable<InpStruct>> readFunc, IComparableInput<long> input){
            var x=0l; var y=0l;
            yield return (x, y);
            foreach(var order in readFunc(input)){
                switch(order.dir){
                    case 'U':
                        y -= order.len;
                        break;
                    case 'D':
                        y += order.len;
                        break;
                    case 'L':
                        x-= order.len;
                        break;
                    case 'R':
                        x+= order.len;
                        break;
                }
                yield return (x, y);
            }
        }

        private struct InpStruct{
            public char dir;
            public int len;
        }
    }
}
