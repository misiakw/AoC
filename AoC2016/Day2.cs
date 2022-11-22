using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoC2016.Common;
using Step = AoC2016.Days.Day1.Step;

namespace AoC2016
{
    public class Day2 : DayBase
    {
        public Day2() : base(2)
        {
            Input("example1")
                .RunPart(1, 1985)
                .RunPart(2, "5DB3")
            .Input("output")            
                .RunPart(1, 36629)
                .RunPart(2, "99C3D")
            ;
        }

        public override object Part1(Input input)
        {
            var X = 1;
            var Y = 1;
            int output = 0;

            var keypad = new string[3]{
            "123",
            "456",
            "789"};
            foreach(var line in input.Lines){
                foreach(var dir in line){
                    switch(dir){
                        case 'U': X--; 
                        if(X < 0) X++; 
                        break;
                        case 'D': X++; 
                        if(X>=keypad.Length) X--; 
                        break;
                        case 'L': Y--; 
                        if(Y<0) Y++; 
                        break;
                        case 'R': Y++; 
                        if(Y>=keypad[X].Length) Y--; 
                        break;
                    }
                }
                output*=10;
                output+=keypad[X][Y]-'0';
            }

            return output;
        }

        public override object Part2(Input input)
        {
            var X = 2;
            var Y = 0;
            string output = string.Empty;

            var keypad = new string[5]{
            "  1  ",
            " 234 ",
            "56789",
            " ABC ",
            "  D  "};
            foreach(var line in input.Lines){
                foreach(var dir in line){
                    switch(dir){
                        case 'U': X--; 
                        if(X < 0 || keypad[X][Y] == ' ') X++; 
                        break;
                        case 'D': X++; 
                        if(X>=keypad.Length || keypad[X][Y] == ' ') X--; 
                        break;
                        case 'L': Y--; 
                        if(Y<0 || keypad[X][Y] == ' ') Y++; 
                        break;
                        case 'R': Y++; 
                        if(Y>=keypad[X].Length || keypad[X][Y] == ' ') Y--; 
                        break;
                    }
                }
                output+=keypad[X][Y];
            }

            return output;
        }
    }
}
namespace AoC2016.Days.Day2{
}
