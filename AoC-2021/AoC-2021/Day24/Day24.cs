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

            Params = nums;
        }

        public override string Part1(string testName)
        {
            var enumeator = Params.GetEnumerator();

            do
            {
                var a = ProcessSet(enumeator, new int[] { 0, 0, 0, 0 }, "");
            } while (enumeator.Current != null);
            var b = 0;

            throw new NotImplementedException();
        }

        public override string Part2(string testName)
        {
            throw new NotImplementedException();
        }

        public IList<string> ProcessSet(IEnumerator<int[]> paramEnumerator, int[] state, string previous)
        {
            var inps = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            if (!paramEnumerator.MoveNext())
                return new List<string>() { previous };
            var param = paramEnumerator.Current;
            var results = new List<string>();
            if (param[0] != 1) //do last call;
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
                    results.AddRange(ProcessSet(paramEnumerator, result, $"{previous}{input}"));
                }
            }
            return results;
        }

        public int[] Process(int input, int[] nums, int[] state)
        {
            int z = state[3];
            int w = input;
            int x = z % 26;

            z /= nums[0];
            x += nums[1];
            x = x == w ? 1 : 0;
            x = x == 0 ? 1 : 0;
            int y = 25 * x + 1;
            z = z * y;
            y = (w + nums[2]) * x;
            z += y;

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
