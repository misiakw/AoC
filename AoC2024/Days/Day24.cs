using AoCBase2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace AoC2024.Days
{
    public class Day24 : IDay
    {
        public static void RunAoC() => AocRuntime.Day<Day24>(24)
                .Callback(1, (d, t) => d.Part1(t.GetLines()))
                //.Callback(2, (d, t) => d.Part2(t.GetLinesAsync()))
                .Test("example1")
                .Test("example2")
                .Test("input")
                //.Part(1).Correct(42410633905894)
                //.Part(2).Correct()
                .Run();

        public string Part1(IEnumerable<string> input)
        {
            var cable = GetCables(input);
            var operations = GetOperations(input.Skip(cable.Count() + 1)).ToList();

            while (operations.Any())
            {
                var set = operations.First(o => cable.ContainsKey(o.a) && cable.ContainsKey(o.b));
                operations.Remove(set);
                if (!cable.ContainsKey(set.output))
                    cable.Add(set.output, set.action(cable[set.a], cable[set.b]));
                else
                    cable[set.output] = set.action(cable[set.a], cable[set.b]);
            }

            return GetRegister(cable, "z").ToString();
        }

        private IDictionary<string, bool> GetCables(IEnumerable<string> input)
        {
            var cable = new Dictionary<string, bool>();

            foreach (var line in input)
            {
                if (string.IsNullOrEmpty(line)) break;
                var parts = line.Split(':').Select(l => l.Trim()).ToArray();
                cable.Add(parts[0], parts[1] == "1");
            }
            return cable;
        }

        private IEnumerable<(string a, string b, string output, Func<bool, bool, bool> action)> GetOperations(IEnumerable<string> input)
        {
            foreach (var line in input)
            {
                var parts = line.Split(" ").ToArray();
                var action = parts[1] switch
                {
                    "AND" => new Func<bool, bool, bool>((a, b) => a & b),
                    "OR" => new Func<bool, bool, bool>((a, b) => a | b),
                    "XOR" => new Func<bool, bool, bool>((a, b) => a ^ b)
                };
                yield return (parts[0], parts[2], parts[4], action);
            }
        }

        private long GetRegister(IDictionary<string, bool> cable, string register)
        {
            var result = string.Empty;
            foreach (var k in cable.Keys.Where(k => k.StartsWith(register)).OrderDescending())
                result += cable[k] ? "1" : "0";

            return Convert.ToInt64(result, 2);
        }
    }
}
