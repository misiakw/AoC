using AoC_2021.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AoC_2021.Day24
{
    //[BasePath("Day24")]
    [TestFile(File = "Input.txt", Name = "Input")]
    public class Day24 : DayBase
    {
        private IReadOnlyList<int[]> Params;
        public Day24(string filePath) : base(filePath)
        {
            var regex = new Regex(@"inp wmul x 0add x zmod x 26div z (\d+)add x (-?\d+)eql x weql x 0mul y 0add y 25mul y xadd y 1mul z ymul y 0add y wadd y (\d+)mul y xadd z y");
            var nums = new List<int[]>();
            var sb = new StringBuilder();
            foreach (var line in LineInput)
            {
                if (line.StartsWith("inp ") && sb.Length > 0)
                {
                    var match = regex.Match(sb.ToString());
                    var num = new int[3];
                    num[0] = int.Parse(match.Groups[1].Value);
                    num[1] = int.Parse(match.Groups[2].Value);
                    num[2] = int.Parse(match.Groups[3].Value);

                    nums.Add(num);

                    sb = new StringBuilder();
                }
                sb.Append(line.Trim()); ;
            }
            var match1 = regex.Match(sb.ToString());
            var num1 = new int[3];
            num1[0] = int.Parse(match1.Groups[1].Value);
            num1[1] = int.Parse(match1.Groups[2].Value);
            num1[2] = int.Parse(match1.Groups[3].Value);

            nums.Add(num1);

            Params = nums;

            foreach (var par in Params) Console.WriteLine($"[{par[0]}][{par[1]}][{par[2]}]");
        }

        public override string Part1(string testName)
        {
            var enumeator = Params.GetEnumerator();

           // do
           // {
                var a = ProcessSet(Params.ToList(), 0, new int[] { 0, 0, 0, 0 }, "");
            //} while (enumeator.Current != null);

            var parsed = a.Select(x => long.Parse(x)).ToList();
            Console.WriteLine($"min => {parsed.Min()}");
            Console.WriteLine($"max => {parsed.Max()}");
            
            return "done";
        }

        public override string Part2(string testName)
        {
            throw new NotImplementedException();
        }

        public IList<string> ProcessSet(IList<int[]> inputs, int inputPos, int[] state, string previous)
        {
            var inps = new int[] { 9,8,7,6,5,4,3,2,1 };
            if (inputPos == inputs.Count())
                return new List<string>() { previous };
            var param = inputs[inputPos];
            var results = new List<string>();
            if (inputPos == inputs.Count() - 1) //do last call;
            {
                foreach (var input in inps)
                {
                    var result = Process(input, param, state.CloneArray());
                    if (result[3] == 0)
                        results.Add($"{previous}{input}");
                }
            }
            else //process to next
            {
                foreach (var input in inps)
                {
                    var result = Process(input, param, state.CloneArray());
                    results.AddRange(ProcessSet(inputs, inputPos+1, result, $"{previous}{input}"));
                }
            }
            return results;
        }

        public int[] Process(int input, int[] nums, int[] state)
        {
            /*inp w
             * mul x 0
             * add x z
             * mod x 26
             * div z(\d+)
             * add x(-?\d +)
             * eql x w
             * eql x 0
             * mul y 0
             * add y 25
             * mul y x
             * add y 1
             * mul z y
             * mul y 0
             * add y w
             * add y(\d+)
             * mul y x
             * add z y*/
            int w = state[0];
            int x = state[1];
            int y = state[2];
            int z = state[3];

            w = input;
            x = 0;
            x = z;
            x = x % 26;
            z = z / nums[0];
            x = x + nums[1]; //here potential below zero w=<1,9>, x = <0,25>
            x = x == w ? 1 : 0;
            x = x == 0 ? 1 : 0; //x == 0 if x == w
            y = y + 25;
            y = y * x;
            y = y + 1;
            z = z * y;
            y = y * 0;
            y = y + w;
            y = y + nums[2];
            y = y * x;  // y==0 if x==0
            z = z + y;  //to get z=0 need y = 0;
           
            return new int[] { w, x, y, z };
        }
    }
    public static class Extensions
    {
        public static int[] CloneArray(this int[] arr)
        {
            var result = new int[arr.Length];
            for (var i = 0; i < arr.Length; i++) result[i] = arr[i];
            return result;
        }
    }
}
