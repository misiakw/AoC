using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using AoC.Base;

namespace AoC2022
{
    public class Day5 : DayBase
    {
        public Day5() : base(5)
        {
            Input("example1")
                .RunPart(1, "CMZ")
                .RunPart(2, "MCD")
            .Input("output")
                .RunPart(1, "TGWSMRBPN")
                .RunPart(2, "TZLTLWRNF");
        }

        public override object Part1(Input input)
        {
            var parts = input.Raw.Split("\n\n");
            var stacks = ReadIput(parts[0].Split("\n"));

            var regex = new Regex(@"move (\d+) from (\d+) to (\d+)");
            int line = 10;
            foreach(var cmd in parts[1].Split("\n")){
                line ++;
                var matches = regex.Match(cmd);
                var amount = int.Parse(matches.Groups[1].Value);
                var source = int.Parse(matches.Groups[2].Value);
                var dest = int.Parse(matches.Groups[3].Value);

                try{
                for(var i=0; i<amount; i++)
                        stacks[dest].Push(stacks[source].Pop());
                }catch(Exception){
                    Console.Write($"line {line}: ");
                    Console.WriteLine(cmd);
                }
            }

            var result = string.Empty;
            for(var i=1; i<=stacks.Count; i++)
                result += stacks[i].Peek();
            return result;
        }

        public override object Part2(Input input)
        {
            var parts = input.Raw.Split("\n\n");
            var stacks = ReadIput(parts[0].Split("\n"));

            var regex = new Regex(@"move (\d+) from (\d+) to (\d+)");
            int line = 10;
            foreach(var cmd in parts[1].Split("\n")){
                line ++;
                var matches = regex.Match(cmd);
                var amount = int.Parse(matches.Groups[1].Value);
                var source = int.Parse(matches.Groups[2].Value);
                var dest = int.Parse(matches.Groups[3].Value);

                try{
                    var tempStack = new Stack<char>();
                    for(var i=0; i<amount; i++)
                        tempStack.Push(stacks[source].Pop());
                    while(tempStack.Any())
                        stacks[dest].Push(tempStack.Pop());
                }catch(Exception){
                    Console.Write($"line {line}: ");
                    Console.WriteLine(cmd);
                }
            }

            var result = string.Empty;
            for(var i=1; i<=stacks.Count; i++)
                result += stacks[i].Peek();
            return result;
        }

        private IDictionary<int, Stack<char>>ReadIput(string[] rows){
            var labels = rows.Last();
            var stacks = new Dictionary<int, Stack<char>>();

            for(var i=1; i<labels.Length; i+=4)
                stacks.Add(int.Parse(""+labels[i]), new Stack<char>());

            rows = rows.Take(rows.Count() - 1)
                .Reverse()
                .ToArray();

            foreach(var row in rows){
                var i = 0;
                for(var x=1; x<row.Length; x+=4){
                    i++;
                    if(row[x] != ' ')
                        stacks[i].Push(row[x]);
                }
            }
            return stacks;
        }
    }
}
