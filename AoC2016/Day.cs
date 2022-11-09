using System;
using AoC2016.Core;

namespace AoC2016
{
    public abstract class Day<T>
    {
        public Day(int dayNum){
            DayNum = dayNum;
        }
        public readonly int DayNum;
        protected abstract string Task1(string input);

        protected abstract string Task2(string input);

        private IList<Input> Inputs = new List<Input>();

        public Day<T> Input<T>(string name){
            var input = new Input($"./Inputs/Day{DayNum}", name);
            Inputs.Add(input);
            Console.WriteLine($"createInput {name}");
            return this;
        }

        public void Exec(){
            Console.WriteLine($">>> Day {DayNum} <<<");
            foreach(var input in Inputs){
                Console.WriteLine($"Task1, input {input.FileName}");
                Console.WriteLine(Task1(input.Read()));
            }
            
            foreach(var input in Inputs){
                Console.WriteLine($"Task2, input {input.FileName}");
                Console.WriteLine(Task2(input.Read()));
            }
        }
    }
}