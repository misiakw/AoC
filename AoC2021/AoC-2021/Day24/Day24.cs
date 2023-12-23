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
            var req = new Req()
            {
                zDivMin = 0,
                zDivMax = 0,
                zModMin = 0,
                zModMax = 0
            };

            for(var s=13; s>=0; s--)
            {
                var par = Params[s];
                var newReq = new Req()
                {
                    zDivMin = req.zDivMin,
                    zDivMax = req.zDivMax,
                    zModMin = req.zModMin,
                    zModMax = req.zModMax,
                };
                Console.WriteLine($"--- step {s} ---");

                if (1+par[2] > req.zModMax || 9+par[2] < req.zModMin)
                {
                    var tmp = new List<int>();
                    for (var i= 1; i < 10; i++){
                        tmp.Add(i - par[1]);
                    }
                    tmp = tmp.Where(y => y >= 0 && y < 26).ToList();
                    newReq.zModMin = tmp.Min();
                    newReq.zModMax = tmp.Max();
                }
                else
                {
                    Console.WriteLine("Any input?");
                }

                if (par[0] == 1)
                {
                    newReq.zDivMin = req.zDivMin;
                    newReq.zDivMax = req.zDivMax;
                }
                else
                {
                    newReq.zDivMin = req.zModMin;
                    newReq.zDivMax = req.zModMax;
                }
                req = newReq;
                Console.WriteLine($"step {s} input: Z/26<{req.zDivMin},{req.zDivMax}> z%26<{req.zModMin},{req.zModMax}>");
            }

            throw new NotImplementedException();
        }

        public override string Part2(string testName)
        {
            throw new NotImplementedException();
        }

        private int ProcessStep(int[] param, int input, int z)
        {
            int x = z % 26;
            x += param[1];
            z = z / param[0];
            if (x == input)
                return z;
            else
                return z * 26 + input + param[2];
        }
        protected struct Req
        {
            public long zDivMin, zDivMax;
            public long zModMin, zModMax;
        }
    }
}
