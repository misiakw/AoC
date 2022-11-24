using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoC2016.Common;
using Step = AoC2016.Days.Day1.Step;

namespace AoC2016
{
    public class Day9 : DayBase
    {
        public Day9() : base(9)
        {
            Input("ex1")
                .RunPart(1, 6);
            Input("ex2")
                .RunPart(1, 7);
            Input("ex3")
                .RunPart(1, 9);
            Input("ex4")
                .RunPart(1, 11);
            Input("ex5")
                .RunPart(1, 6);
            Input("ex6")
                .RunPart(1, 18);
            Input("output")
                .TooLow(1, 112585);
        }

        public override object Part1(Input input)
        {
            var word  = input.Raw;
            var output = string.Empty;

            var i = 0;
            while(i<word.Length){
                if(word[i] == '('){
                    var len = ReadNum(word, ref i);
                    i++; // x
                    var reps = ReadNum(word, ref i);
                    i++; // )

                    var source = string.Empty;
                    for(;len > 0 && i<=word.Length; len--){
                        
                        source += word[i+1];
                        i++;
                    }
                    for(; reps>0; reps--)
                    output += source;
                }else{
                    output += word[i];
                }
                i++;
            }

            return output.Length;
        }

        public override object Part2(Input input)
        {
            throw new NotImplementedException();
        }

        private int ReadNum(string val, ref int ctr){
            var result = 0;
            ctr++;
            while(val[ctr] >= '0' && val[ctr] < '9'){
                result = result*10 + (val[ctr]-'0');
                ctr++;
            }
            ctr--;
            return result;
        }
    }
}
namespace AoC2016.Days.Day2{
}
