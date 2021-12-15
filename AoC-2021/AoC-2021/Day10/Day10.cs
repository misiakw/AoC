using AoC_2021.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC_2021.Day10
{
    [BasePath("Day10")]
    [TestFile(File = "example.txt", Name = "Example")]
    [TestFile(File = "Input.txt", Name = "Input")]
    public class Day10 : DayBase
    {
        public Day10(string filePath) : base(filePath)
        {
        }

        private IList<Stack<char>> Incompletes = new List<Stack<char>>();

        [ExpectedResult(TestName = "Example", Result = "26397")]
        [ExpectedResult(TestName = "Input", Result = "216297")]
        public override string Part1(string testName)
        {
            var result = 0;
            foreach (var line in this.LineInput)
            {
                result += Process(line);
            }

            return result.ToString();
        }

        [ExpectedResult(TestName = "Example", Result = "288957")]
        [ExpectedResult(TestName = "Input", Result = "2165057169")]
        public override string Part2(string testName)
        {
            var scores = new List<long>();
            var points = new Dictionary<char, long>()
            {
                {'(', 1}, {'[', 2}, {'{', 3}, {'<', 4}
            };
            
            foreach(var stack in Incompletes)
            {
                var score = 0L;
                while (stack.Count() > 0)
                    score = score * 5 + points[stack.Pop()];
                scores.Add(score);
            }

            scores = scores.OrderBy(s => s).ToList();
            var middle = (int)(scores.Count() / 2) + 1;
            return (scores.Take(middle).Last()).ToString();
        }

        private int Process(string line)
        {
            int result = 0;
            var stack = new Stack<char>();
            foreach (var ch in line)
                switch (ch)
                {
                    case '(':
                    case '[':
                    case '{':
                    case '<':
                        stack.Push(ch);
                        break;
                    case ')':
                        if (stack.Pop() != '(')
                        {
                            result += 3;
                            return result;
                        }
                        break;
                    case ']':
                        if (stack.Pop() != '[')
                        {
                            result += 57;
                            return result;
                        }
                        break;
                    case '}':
                        if (stack.Pop() != '{')
                        {
                            result += 1197;
                            return result;
                        }
                        break;
                    case '>':
                        if (stack.Pop() != '<')
                        {
                            result += 25137;
                            return result;
                        }
                        break;
                }
            Incompletes.Add(stack);
            return 0;
        }
    }
}
