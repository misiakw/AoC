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
               .Part2(82000210);
            builder.New("output", "./Inputs/Day11/output.txt")
                .Part1(10292708)
                .Part2(790194712336);
        }
        public override long Part1(IComparableInput<long> input)
        {
            var stars = ReadInput(input);

            stars = ExpandStars(stars, 2);
            var sum = 0L;
            foreach (var star in stars)
                foreach (var star2 in stars.Where(s => s.Item3 > star.Item3))
                    sum += Math.Abs(star2.Item1 - star.Item1) + Math.Abs(star2.Item2 - star.Item2);

            return sum;
        }

        public override long Part2(IComparableInput<long> input)
        {
            var stars = ReadInput(input);

            stars = ExpandStars(stars, 1000000);

            var sum = 0L;
            var i = 1;
            foreach (var star in stars.OrderBy(s => s.Item3))
                foreach (var star2 in stars.Where(s => s.Item3 > star.Item3).OrderBy(s => s.Item3))
                {
                    var inc = Math.Abs(star2.Item1 - star.Item1) + Math.Abs(star2.Item2 - star.Item2);
                    sum += inc;
                }

            return sum;
        }

        private IEnumerable<(int, int, int)> ReadInput(IComparableInput<long> input)
        {
            var lines = ReadLines(input);
            var i = 0;
            var y = 1;
            foreach (var line in lines) {
                for (var x = 1; x <= line.Length; x++)
                    if(line[x-1] == '#')
                        yield return (x, y, i++);
                y++;
            }
        }

        private void PrintStars(IEnumerable<(int, int, int)> stars)
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

        private IEnumerable<(int, int, int)> ExpandStars(IEnumerable<(int, int, int)> stars, int newSize)
        {
            newSize = newSize - 1;
            for (var x = 1; x < stars.Max(p => p.Item1); x++)
            {
                if (stars.All(s => s.Item1 != x))
                {
                    var newStars = stars.Where(s => s.Item1 < x).ToList();
                    foreach (var star in stars.Where(s => s.Item1 > x))
                        newStars.Add((star.Item1 + newSize, star.Item2, star.Item3));
                    x+= newSize;
                    stars = newStars;
                }
            }

            for (var y = 1; y < stars.Max(p => p.Item2); y++)
            {
                if (stars.All(s => s.Item2 != y))
                {
                    var newStars = stars.Where(s => s.Item2 < y).ToList();
                    foreach (var star in stars.Where(s => s.Item2 > y))
                        newStars.Add((star.Item1, star.Item2+ newSize, star.Item3));
                    y+= newSize;
                    stars = newStars;
                }
            }

            return stars;
        }
    }
}
