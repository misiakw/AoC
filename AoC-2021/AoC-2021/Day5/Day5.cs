using AoC_2021.Attributes;
using AoC_2021.Common;
using AoC_2021.InputClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC_2021.Day5
{
    [BasePath("Day5")]
    [TestFile(File = "example.txt", Name = "Example")]
    [TestFile(File = "Input.txt", Name = "Input")]
    public class Day5 : ParseableDay<Tuple<Point, Point>>
    {
        public Day5(string path) : base(path)
        {
        }

        private Array2D<int> map = new Array2D<int>();

        public override Tuple<Point, Point> Parse(string input)
        {
            var parts = input.Trim().Split(" -> ");
            return Tuple.Create(new Point(parts[0].Split(",")), new Point(parts[1].Split(",")));
        }

        [ExpectedResult(TestName = "Example", Result = "5")]
        [ExpectedResult(TestName = "Input", Result = "5608")]
        public override string Part1()
        {
            
            foreach (var horizontal in this.Input.Where(t => t.Item1.Y == t.Item2.Y))
            {
                var min = horizontal.Item1.X <= horizontal.Item2.X ? horizontal.Item1.X : horizontal.Item2.X;
                var max = horizontal.Item1.X >= horizontal.Item2.X ? horizontal.Item1.X : horizontal.Item2.X;
                for (var i = min; i <= max; i++)
                    map[i, horizontal.Item1.Y]++;
            }
            foreach (var vertical in this.Input.Where(t => t.Item1.X == t.Item2.X))
            {
                var min = vertical.Item1.Y <= vertical.Item2.Y ? vertical.Item1.Y : vertical.Item2.Y;
                var max = vertical.Item1.Y >= vertical.Item2.Y ? vertical.Item1.Y : vertical.Item2.Y;
                for (var i = min; i <= max; i++)
                    map[vertical.Item1.X, i]++;
            }

            //Console.WriteLine(map.Draw(x => x == 0 ? "." : x.ToString()));
            var count = 0;
            var bounds = map.Bounds;

            for (var y = bounds[1].Item1; y <= bounds[1].Item2; y++)
                for (var x = bounds[0].Item1; x <= bounds[0].Item2; x++)
                    if (map[x, y] > 1)
                        count++;

           return count.ToString();
        }

        [ExpectedResult(TestName = "Example", Result = "12")]
        [ExpectedResult(TestName = "Input", Result = "20299")]
        public override string Part2()
        {
            foreach(var diag in this.Input.Where(t => IsDiagoal(t)))
            {
                var span = Math.Abs(diag.Item1.X - diag.Item2.X);
                var dx = (diag.Item1.X - diag.Item2.X)/span*-1;
                var dy = (diag.Item1.Y - diag.Item2.Y)/span*-1;

                for(var mul=0; mul <= span; mul++)
                {
                    var x = diag.Item1.X + (mul * dx);
                    var y = diag.Item1.Y + (mul * dy);
                    map[x, y]++;
                }
            }
            //Console.WriteLine(map.Draw(x => x == 0 ? "." : x.ToString()));
            var count = 0;
            var bounds = map.Bounds;

            for (var y = bounds[1].Item1; y <= bounds[1].Item2; y++)
                for (var x = bounds[0].Item1; x <= bounds[0].Item2; x++)
                    if (map[x, y] > 1)
                        count++;

            return count.ToString();
        }

        private bool IsDiagoal(Tuple<Point, Point> line)
        {
            var w = line.Item1.X - line.Item2.X;
            var h = line.Item1.Y - line.Item2.Y;
            return Math.Abs(w) == Math.Abs(h);
        }
    }
}
