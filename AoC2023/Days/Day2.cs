using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using AoC.Base;
using AoC.Base.TestInputs;
using AoC.Common;


namespace AoC2023.Days
{
    public class Day2 : AbstractDay<int, IComparableInput<int>>
    {
        public override int Part1(IComparableInput<int> input)
        {
            var games = ReadInput(input)
                .Where(g => g.balls.All(b => b.r <= 12 && b.g <= 13 && b.b <= 14))
                .ToList();

            return games.Sum(g=> g.num);
        }

        public override int Part2(IComparableInput<int> input)
        {
            var games = ReadInput(input)
                .Select(g => 
                    g.balls.Max(b => b.r)
                    *g.balls.Max(b => b.g)
                    *g.balls.Max(b => b.b))
                .ToList();
            return games.Sum();
        }

        public override void PrepateTests(InputBuilder<int, IComparableInput<int>> builder)
        {
            builder.New("example1", "./Inputs/Day2/example1.txt")
               .Part1(8)
               .Part2(2286);
            builder.New("output", "./Inputs/Day2/output.txt")
                .Part1(1853)
                .Part2(72706);
        }

        private IEnumerable<Game> ReadInput(IComparableInput<int> input){
            var t = input.ReadLines();
            t.Wait();
            var lines = t.Result.ToArray();

            var result = 0;

            foreach (var line in lines){
                //Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green
                var tmp = line.Split(":");
                var game = new Game(tmp[0].Split(" ").Skip(1).Select(int.Parse).First());
                var tests = tmp[1].Split(";");
                foreach (var test in tests){
                    var parts = test.Split(",").Select(s => s.Trim()).ToArray();
                    var r = 0;
                    var g = 0;
                    var b = 0;
                    foreach (var part in parts){
                        var balls = part.Split(" ");
                        if (balls[1] == "red")
                            r = int.Parse(balls[0]);
                        else if (balls[1] == "green")
                            g = int.Parse(balls[0]);
                        else
                            b = int.Parse(balls[0]);
                    }
                    game.balls.Add(new Balls(r, g, b));
                }
                yield return game;
            }
        }

        private class Balls{
            public Balls(int r, int g, int b){
                this.r = r;
                this.b = b;
                this.g = g;
            }
            public readonly int r = 0;
            public readonly int g = 0;
            public readonly int b;
        }
        private class Game{
            public Game(int num){
                this.num = num;
            }
            public readonly int num;
            public IList<Balls> balls = new List<Balls>();
        }
    }
}