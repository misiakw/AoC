using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoC.Base;
using AoC.Base.TestInputs;


namespace AoC2023.Days
{
    public class Day1 : AbstractDay<int, IComparableInput<int>>
    {
        public override int Part1(IComparableInput<int> input)
        {
            var t = input.ReadLines();
            t.Wait();
            var lines = t.Result.ToArray();

            var sum = 0;
            foreach (var line in lines)
            {
                var tmp = (line.First(c => c >= '0' && c <= '9') - '0') * 10;
                tmp += (line.Last(c => c >= '0' && c <= '9') - '0');
                sum += tmp;
            }
            return sum;
        }

        public override int Part2(IComparableInput<int> input)
        {
            var replacer = new RollReplacer(new List<(string, string)>
            {
                ("one", "1"), ("two", "2"), ("three", "3"), ("four", "4"),
                ("five", "5"), ("six", "6"), ("seven", "7"), ("eight", "8"), ("nine", "9")
            });

            var t = input.ReadLines();
            t.Wait();
            var lines = t.Result.ToArray();

            var sum = 0;
            foreach (var line in lines)
            {
                var line2 = replacer.Replace(line);

                var tmp = (line2.First(c => c >= '0' && c <= '9') - '0') * 10;
                tmp += (line2.Last(c => c >= '0' && c <= '9') - '0');
                sum += tmp;
            }
            return sum;
        }

        public override void PrepateTests(InputBuilder<int, IComparableInput<int>> builder)
        {
            builder.New("example1", "./Inputs/Day1/example1.txt")
               .Part1(142);
            builder.New("example2", "./Inputs/Day1/example2.txt")
               .Part2(281);
            builder.New("output", "./Inputs/Day1/output.txt")
                .Part1(55834)
                .Part2(0); //52606 too low, 53254 too high
        }

        private class RollReplacer
        {
            private RepNode _root = new RepNode();
            public RollReplacer(IList<(string, string)> replacements)
            {
                foreach (var item in replacements)
                    this._root.Add(item.Item1, item.Item2);
            }

            public string Replace(string input)
            {
                var result = string.Empty;

                var buff = new Queue<char>();

                foreach(var ch in input)
                {
                    buff.Enqueue(ch);
                    var currKey = new string(buff.ToArray());
                    if (this._root.CanReachValue(currKey))
                    {
                        var val = this._root.GetValue(currKey);
                        if(val != null)
                        {
                            result = result + val;
                            buff.Clear();
                        }
                    }
                    else
                    {
                        while(buff.Count > 0 && !this._root.CanReachValue(currKey))
                        {
                            result += buff.Dequeue();
                            currKey = new string(buff.ToArray());
                            if (this._root.CanReachValue(currKey))
                            {
                                var val = this._root.GetValue(currKey);
                                if (val != null)
                                {
                                    result = result + val;
                                    buff.Clear();
                                }
                            }
                        }
                    }
                }

                return result;
            }

            private class RepNode
            {
                public IDictionary<char, RepNode> Nodes = new Dictionary<char, RepNode>();
                private  string? _value = null;

                public void Add(string key, string value)
                {
                    if (string.IsNullOrEmpty(key))
                        this._value = value;
                    else
                    {
                        var newK = key[0];
                        if (!Nodes.ContainsKey(newK))
                            Nodes.Add(newK, new RepNode());
                        Nodes[newK].Add(key.Substring(1), value);
                    }
                }

                public bool CanReachValue(string key)
                {
                    if (string.IsNullOrEmpty(key))
                        return true;
                    if (!this.Nodes.ContainsKey(key[0]))
                        return false;
                    return Nodes[key[0]].CanReachValue(key.Substring(1));
                }
                public string? GetValue(string key)
                    => string.IsNullOrEmpty(key)
                    ? this._value
                    : this.Nodes[key[0]].GetValue(key.Substring(1));
            }
        }
    }
}
