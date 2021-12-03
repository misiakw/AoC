using AoC_2021.Attributes;
using AoC_2021.InputClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC_2021.Day3
{
    [BasePath("Day3")]
    [TestFile(File = "example.txt", Name = "Example")]
    [TestFile(File = "Input.txt", Name = "Input")]
    public class Day3 : DayBase
    {
        public Day3(string filePath) : base(filePath)
        {
        }

        private string mostCommon = "";
        private string leastCommon = "";

        [ExpectedResult(TestName = "Example", Result = "198")]
        [ExpectedResult(TestName = "Input", Result = "4174964")]
        public override string Part1()
        {
            string gamma = "";
            string epsilon = "";
            for(var i=0; i<this.LineInput[0].Length; i++)
            {
                var bits = GetBitsCount(this.LineInput, i);
                
                if (bits[0] > bits[1])
                {
                    gamma += "0";
                    epsilon += "1";
                }
                else
                {
                    gamma += "1";
                    epsilon += "0";
                }
            }

            this.mostCommon = gamma;
            this.leastCommon = epsilon;

            return (Convert.ToInt32(gamma, 2)*Convert.ToInt32(epsilon, 2)).ToString();
        }

        [ExpectedResult(TestName = "Example", Result = "230")]
        [ExpectedResult(TestName = "Input", Result = "4474944")]
        public override string Part2()
        {
            var oxygen = this.LineInput.ToList();
            var co2 = this.LineInput.ToList();

            for(var pos = 0; pos < this.LineInput[0].Length; pos++)
            {
                if (oxygen.Count > 1)
                {
                    var bits = GetBitsCount(oxygen, pos);
                    oxygen = (bits[0] > bits[1])
                         ? oxygen.Where(l => l[pos] == '0').ToList()
                         : oxygen.Where(l => l[pos] == '1').ToList();
                }
                if (co2.Count > 1)
                {
                    var bits = GetBitsCount(co2, pos);
                    co2 = (bits[0] <= bits[1])
                         ? co2.Where(l => l[pos] == '0').ToList()
                         : co2.Where(l => l[pos] == '1').ToList();
                }
            }

            return (Convert.ToInt32(oxygen.First(), 2)* Convert.ToInt32(co2.First(), 2)).ToString();
        }


        private int[] GetBitsCount(IReadOnlyList<string>input, int pos)
        {
            var result = new int[2] { 0, 0 };

            foreach(var line in input)
            {
                result[line[pos] - '0']++;
            }

            return result;
        }
    }
}
