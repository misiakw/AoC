using System;
using AoC2016.Core;

namespace AoC2016
{
    public abstract class Day<T>
    {
        protected readonly ConfigBase<T> conf;

        public Day(int num){
            conf = new ConfigBase<T>(num);
        }

        public void Exec(){
            Console.WriteLine($">>>> {conf.DayNum} <<<<");

            foreach(var input in conf.Inputs){
                Console.WriteLine(input.Name);
                Console.WriteLine(input.results);
            }


        }
        protected abstract string Task1(T input);

        protected abstract string Task2(T input);
    }
}