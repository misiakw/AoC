using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day2
{
    class Program
    {
        static void Main(string[] args)
        {
            var prog = new Program();
            //Console.WriteLine(prog.Task1());
            Console.WriteLine(prog.Task2());
            Console.ReadLine();
        }

        public string Task1()
        {
            using (var stream = new StreamReader("../../input.txt"))
            {
                var counts = new Dictionary<int, int>();
                while (!stream.EndOfStream)
                {
                    var line = stream.ReadLine();
                    var set = new Dictionary<char, int>();
                    foreach (var ch in line.Trim())
                    {
                        if (!set.ContainsKey(ch))
                        {
                            set.Add(ch, 0);
                        }
                        set[ch]++;
                    }

                    foreach (var g in set.Values.GroupBy(v => v).Select(v => v.Key).Distinct().OrderBy(v => v))
                    {
                        if (!counts.ContainsKey(g))
                        {
                            counts.Add(g, 0);
                        }
                        counts[g]++;
                    }
                }

                var checksum = counts[2] * counts[3];
                return $"task1: {counts[2]} * {counts[3]} = {checksum}";
            }
        }

        public string Task2()
        {
            var lines = new List<string>();
            using (var stream = new StreamReader("../../input.txt"))
            {
                while (!stream.EndOfStream)
                {
                    var line = stream.ReadLine();
                    lines.Add(line.Trim());
                }
            }

            lines = lines.OrderBy(x => x).ToList();

            Tuple<string, string> result = null;
            Parallel.ForEach(lines, line =>
            {
                var match = GetSymilar(line, lines);
                if (match != null)
                {
                    result = match;
                    return;
                }
            });

            if (result != null)
            {
                var sb = new StringBuilder();
                for(var i=0; i<result.Item1.Length; i++)
                {
                    if (result.Item1[i] == result.Item2[i])
                    {
                        sb.Append(result.Item1[i]);
                    }
                }
                return sb.ToString();
            }

            return "nie ma";
        }

        private Tuple<string, string> GetSymilar(string key, IList<string> array)
        {
            string match = null;
            var result = Parallel.ForEach(array, item =>
            {
                if (IsSimilar(item, key, 1))
                {
                    match = item;
                    return;
                }
            });
            return (match != null)
                ? new Tuple<string, string>(key, match)
                : null;
        }

        public static bool IsSimilar(string source, string val, int difSize)
        {
            var dif = 0;
            for (var i = 0; i < source.Length; i++)
            {
                dif += (source[i] == val[i]) ? 0 : 1;
                if (dif > difSize) return false;
            }
            return dif != 0 ? true : false ;
        }
    }
}
