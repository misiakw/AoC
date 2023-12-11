using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AoC.Base;
using AoC.Base.TestInputs;
using AoC.Common;


namespace AoC2023.Days
{
    public class Day11 : AbstractDay<long, IComparableInput<long>>
    {
        public override void PrepateTests(InputBuilder<long, IComparableInput<long>> builder)
        {
            builder.New("example1", "./Inputs/Day11/example1.txt")
               .Part1(374)
               .Part2(1030);
            builder.New("output", "./Inputs/Day11/output.txt")
                .Part1(10292708);
            //   .Part2(933);
        }
        public override long Part1(IComparableInput<long> input)
        {
            var stars = ReadInput(input);

            stars = ExpandStars(stars, 1);
            var starDist = new Dictionary<int, (int, int)>();

            var i = 1;
            var sum = 0;
            foreach (var star in stars)
                foreach (var star2 in stars.Where(s => s != star))
                    sum += Math.Abs(star2.Item1 - star.Item1) + Math.Abs(star2.Item2 - star.Item2);

            return sum / 2;
        }

        public override long Part2(IComparableInput<long> input)
        {
            var stars = ReadInput(input);

            stars = ExpandStars(stars, 10);
            var starDist = new Dictionary<int, (int, int)>();

            var i = 1;
            var sum = 0;
            foreach (var star in stars)
                foreach (var star2 in stars.Where(s => s != star))
                    sum += Math.Abs(star2.Item1 - star.Item1) + Math.Abs(star2.Item2 - star.Item2);

            return sum / 2;
        }

        private IEnumerable<(int, int)> ReadInput(IComparableInput<long> input)
        {
            var lines = ReadLines(input);
            var y = 0;
            foreach (var line in lines) {
                for (var x = 0; x < line.Length; x++)
                    if(line[x] == '#')
                        yield return (x, y);
                y++;
            }
        }

        private void PrintStars(IEnumerable<(int, int)> stars)
        {
            var width = stars.Max(p => p.Item1);
            var height = stars.Max(p => p.Item2);

            for (var y=0; y<=height; y++)
            {
                for (var x = 0; x <= width; x++)
                    Console.Write(stars.Any(s => s.Item1 == x && s.Item2 == y) ? '#' : '.');
                Console.WriteLine();
            }
        }

        private IEnumerable<(int, int)> ExpandStars(IEnumerable<(int, int)> stars, int increase)
        {
            for (var x = 0; x < stars.Max(p => p.Item1); x++)
            {
                if (stars.All(s => s.Item1 != x))
                {
                    var newStars = stars.Where(s => s.Item1 < x).ToList();
                    foreach (var star in stars.Where(s => s.Item1 > x))
                        newStars.Add((star.Item1 + increase, star.Item2));
                    x+= increase;
                    stars = newStars;
                }
            }

            for (var y = 0; y < stars.Max(p => p.Item2); y++)
            {
                if (stars.All(s => s.Item2 != y))
                {
                    var newStars = stars.Where(s => s.Item2 < y).ToList();
                    foreach (var star in stars.Where(s => s.Item2 > y))
                        newStars.Add((star.Item1, star.Item2+ increase));
                    y+= increase;
                    stars = newStars;
                }
            }

            return stars;
        }
    }
}
