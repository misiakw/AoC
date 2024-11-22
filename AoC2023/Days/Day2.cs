using System.Collections.Generic;
using System.Linq;
using AoCBase2;
using AoCBase2.InputClasses;

namespace AoC2023.Days
{
    public class Day2 : LinesInput
    {
        public string Part1(IAsyncEnumerable<string> lines)
        {
            var games = ReadInput(lines)
                .Where(g => g.balls.All(b => b.r <= 12 && b.g <= 13 && b.b <= 14))
                .ToList();

            return games.Sum(g=> g.num).ToString();
        }

        public string Part2(IAsyncEnumerable<string> lines)
        {
            var games = ReadInput(lines)
                .Select(g => 
                    g.balls.Max(b => b.r)
                    *g.balls.Max(b => b.g)
                    *g.balls.Max(b => b.b))
                .ToList();
            return games.Sum().ToString();
        }
        public static void ProceedAoC()
        {
            AocRuntime.Day(2, Setup<Day2>)
                .Callback(1, d => d.Part1(d.input))
                .Callback(2, d => d.Part2(d.input))
                .Test("example1", "./Inputs/Day2/example1.txt")
                    .Part(1).Correct(8)
                    .Part(2).Correct(2286)
                .Test("output", "./Inputs/Day2/output.txt")
                    .Part(1).Correct(1853)
                    .Part(2).Correct(72706)
                .Run();
        }

        private IEnumerable<Game> ReadInput(IAsyncEnumerable<string> input){
            var result = 0;

            foreach (var line in input.ToEnumerable().ToArray())
            {
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