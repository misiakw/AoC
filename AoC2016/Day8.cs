using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoC2016.Common;
using Step = AoC2016.Days.Day1.Step;

namespace AoC2016
{
    public class Day8 : DayBase
    {
        public Day8() : base(8)
        {
            Input("example1")
                .RunPart(1, 6)
            .Input("output")
                .RunPart(1, 106)
                .RunPart(2);
        }

        public override object Part1(Input input)
        {
            var disp = new Display();

            foreach(var cmd in input.Lines){
                var parts = cmd.Trim().Split(" ");

                if (parts[0] == "rect"){
                    var size = parts[1].Split("x").Select(s => int.Parse(s)).ToArray();
                    disp.Rect(size[0], size[1]);
                }

                if(parts[0] == "rotate"){
                    var A = int.Parse(parts[2].Substring(2, parts[2].Length-2));
                    var B = int.Parse(parts[4]);

                    if(parts[1] == "row")
                        disp.ShiftRow(A, B);
                    if(parts[1] == "column")
                        disp.ShiftColumn(A, B);
                }
            }

            input.Cache = disp;
            return disp.Lit;
        }

        public override object Part2(Input input)
        {
            Console.WriteLine(input.Cache);
            return null;
        }

        private class Display{
            private char[,] arr = new char[6, 50];
            public int Lit {
                get{
                    var count = 0;
                     for(var l=0; l<6; l++)
                    for(var r=0; r<50; r++)
                        if (arr[l,r] == '#') count++;
                    return count;
                }
            }

            public Display(){
                for(var l=0; l<6; l++)
                    for(var r=0; r<50; r++)
                        arr[l,r] = '.';
            }

            public override string ToString()
            {
                var str = String.Empty;
                 for(var l=0; l<6; l++){
                    str += $"{l}|";
                    for(var r=0; r<50; r++)
                        str += arr[l,r];
                    str+='\n';
                 }
                 return str;
            }

            public void Rect(int w, int h){
                for(var y=0; y<h; y++)
                    for(var x=0; x<w; x++)
                        arr[y,x] = '#';
            }

            public void ShiftColumn(int column, int shift){
                var prev = new char[6];
                for(var i = 0; i<6; i++)
                    prev[i] = arr[i, column];

                for(var i = 0; i<6; i++)
                    arr[(i+shift)%6, column] = prev[i];
            }
            public void ShiftRow(int row, int shift){
                var prev = new char[50];
                for(var i = 0; i<50; i++)
                    prev[i] = arr[row, i];

                for(var i = 0; i<50; i++)
                    arr[row, (i+shift)%50] = prev[i];
            }
        }
    }
}