using System;

namespace AoC2016
{
    public class Day1: Day<string>
    {
        public Day1() : base(1){
            conf
                .Input("example1")
                    .WithResult(5).OnStep(1)
                    .OnStep(2)
                .Input("example2")
                    .WithResult(2).OnStep(1)
                .Input("example3")
                    .WithResult(12).OnStep(1)
                .Input("output")
                    .OnStep(1);
        }

        protected override string Task1(string input){
            return input;
        }

        protected override string Task2(string input){
            return "task2";
        }
    }
}