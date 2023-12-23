using AoC_2021.Attributes;
using AoC_2021.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC_2021.Day13
{
    [BasePath("Day13")]
    [TestFile(File = "example.txt", Name = "Example")]
    [TestFile(File = "Input.txt", Name = "Input")]
    public class Day13 : DayBase
    {
        private FoldableArray sheet1 = new FoldableArray();
        private IList<Tuple<char, int>> folds = new List<Tuple<char, int>>();
        public Day13(string filePath) : base(filePath)
        {
            bool doFolds = false;
            foreach(var line in LineInput)
            {
                if (line.Trim().Count() == 0)
                {
                    doFolds = true;
                    continue;
                }
                if (!doFolds) {
                    var pos = line.Trim().Split(",").Select(l => int.Parse(l)).ToArray();
                    sheet1[pos[0], pos[1]] = '#';
                }
                else
                {
                    var cmd = line.Replace("fold along", "").Trim().Split("=");
                    folds.Add(Tuple.Create(cmd[0][0], int.Parse(cmd[1])));
                }
            }
        }

        [ExpectedResult(TestName = "Example", Result = "17")]
        [ExpectedResult(TestName = "Input", Result = "729")]
        public override string Part1(string testName)
        {
            foreach (var fold in folds.Take(1))
            {
                if (fold.Item1 == 'x')
                    sheet1.FoldX(fold.Item2);
                else
                    sheet1.FoldY(fold.Item2);
            }

            //Console.WriteLine(sheet1);
            return sheet1.Size.ToString();
        }

        public override string Part2(string testName)
        {

            foreach (var fold in folds.Skip(1))
            {
                if (fold.Item1 == 'x')
                    sheet1.FoldX(fold.Item2);
                else
                    sheet1.FoldY(fold.Item2);
            }

            Console.WriteLine(sheet1);
            return sheet1.Size.ToString();
        }

        private class FoldableArray
        {
            private IDictionary<string, char> data = new Dictionary<string, char>();
            public int Width { get; private set; }
            public int Height { get; private set; }

            public char this[int x, int y]
            {
                get { return data.ContainsKey($"{x},{y}") ? data[$"{x},{y}"] : '.'; }
                set {
                    if (data.ContainsKey($"{x},{y}"))
                        data[$"{x},{y}"] = value;
                    else
                    {
                        data.Add($"{x},{y}", value);
                        if (Width < x) Width = x;
                        if (Height < y) Height = y;
                    }
                }
            }

            public override string ToString()
            {
                var sb = new StringBuilder();
                for (int y= -1; y <= Height+1; y++){
                    for (int x = -1; x <= Width+1; x++)
                        sb.Append(this[x, y]);
                    sb.AppendLine();
                }
                return sb.ToString();
            }

            public int Size { get { return data.Count(); } }
            public void FoldY(int pos)
            {
                for (var y = pos + 1; y <= Height; y++)
                    for (var x = 0; x <= Width; x++)
                        if (data.ContainsKey($"{x},{y}"))
                        {
                            var newY = pos + (pos - y);
                            this[x, newY] = data[$"{x},{y}"];
                            data.Remove($"{x},{y}");
                        }
                Height = pos - 1;

            }
            public void FoldX(int pos)
            {
                for (var y = 0; y <= Height; y++)
                    for (var x = pos+1; x <= Width; x++)
                        if (data.ContainsKey($"{x},{y}"))
                        {
                            var newX = pos + (pos - x);
                            this[newX, y] = data[$"{x},{y}"];
                            data.Remove($"{x},{y}");
                        }
                Width = pos - 1;
            }
        }
    }
}
