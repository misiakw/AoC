using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AbstractDay
{
    public class Program
    {
        public static void Main(string[] args)
        {
        }
    }

    public abstract class AbstractProgram<T>
    {
        

        protected AbstractProgram()
        {
            var input = ParseInput(ReadInput());
            Console.WriteLine("Task 1:");
            Console.WriteLine(Task1(input));
            Console.WriteLine("Task 2:");
            Console.WriteLine(Task2(input));
            Console.WriteLine("FINISH");
            Console.ReadLine();
        }
        
        public abstract string Task1(IList<T> input);
        public abstract string Task2(IList<T> input);

        protected abstract IList<T> ParseInput(IList<string> input);

        private IList<string> ReadInput()
        {
            using (var stream = new StreamReader("input.txt"))
            {
                var result = new List<string>();
                while (!stream.EndOfStream)
                {
                    result.Add(stream.ReadLine());
                }


                return result;
            }
        }
    }

}
