using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AoC.LegacyBase;
using AoCBase2;

namespace AoC2023.Days
{
    public class Day18 : LegacyAbstractDay
    {
        public override void PrepateTests(DayState<LegacyAbstractDay> dayState)
        {
            dayState.Test("example1", "./Inputs/Day18/example1.txt")
                    .Part(1).Correct(62)
                    .Part(2).Correct(952408144115)
                .Test("output", "./Inputs/Day18/output.txt")
                    .Part(1).Correct(39194)
                    .Part(2).Correct(0);
        }

        private IEnumerable<InpStruct> readInput1(TestState input){
            var regex = new Regex(@"(.) (\d+) \(\#(.*)\)");
            foreach(var line in input.GetLines()){
                var match = regex.Match(line);
                yield return new InpStruct(){
                    dir = match.Groups[1].Value[0],
                    len = int.Parse(match.Groups[2].Value)
                };
            }
        }
        private IEnumerable<InpStruct> readInput2(TestState input){
            var regex = new Regex(@"(.) (\d+) \(\#(.*)\)");
            foreach(var line in input.GetLines()){
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

        public override string Part1(TestState input)
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

            return ((Math.Abs(pow)+ctr)/2+1).ToString();
        }

        public override string Part2(TestState input)
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

            return ((Math.Abs(pow)+ctr)/2+1).ToString();
        }

        private IEnumerable<(long, long)> Read(Func<TestState, IEnumerable<InpStruct>> readFunc, TestState input){
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
