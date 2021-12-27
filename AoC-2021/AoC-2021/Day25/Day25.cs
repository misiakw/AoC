using AoC_2021.Attributes;
using AoC_2021.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC_2021.Day25
{
    [BasePath("Day25")]
    [TestFile(File = "example.txt", Name = "Example", TestToProceed = TestCase.Part1)]
    [TestFile(File = "Input.txt", Name = "Input", TestToProceed = TestCase.Part1)]
    public class Day25 : DayBase
    {
        private char[,] Map;
        private int width, height;

        public Day25(string filePath) : base(filePath)
        {
            width = LineInput[0].Length;
            height = LineInput.Count();
            Map = new char[width, height];

            for (var y = 0; y < height; y++)
                for (var x = 0; x < width; x++)
                    Map[x, y] = LineInput[y][x];
        }

        [ExpectedResult(TestName = "Example", Result = "58")]
        [ExpectedResult(TestName = "Input", Result = "374")]
        public override string Part1(string testName)
        {
            var wasMovement = true;
            var rounds = 0L;

            while (wasMovement)
            {
                wasMovement = false;
                //wasMovement |= MovedRight();
                wasMovement = Moved((x, y, moves) =>
                {
                    if (Map[x, y] == '>')
                    {
                        var next = x < width - 1 ? x + 1 : 0;
                        if (Map[next, y] == '.')
                            moves.Add(new Point(x, y));
                    }
                }, move => {
                    var next = move.X < width - 1 ? move.X + 1 : 0;
                    Map[next, move.Y] = '>';
                    Map[move.X, move.Y] = '.';
                });
                //wasMovement |= MovedDown();
                wasMovement |= Moved((x, y, moves) =>
                {
                    if (Map[x, y] == 'v')
                    {
                        var next = y < height - 1 ? y + 1 : 0;
                        if (Map[x, next] == '.')
                            moves.Add(new Point(x, y));
                    }
                }, move =>
                {
                    var next = move.Y < height - 1 ? move.Y + 1 : 0;
                    Map[move.X, next] = 'v';
                    Map[move.X, move.Y] = '.';
                });
                rounds++;
            }

            return rounds.ToString();
        }

        public override string Part2(string testName)
        {
            throw new NotImplementedException();
        }

        private bool Moved(Action<long, long, IList<Point>> testAction, Action<Point> moveAction)
        {

            var moves = new List<Point>();
            for (var y = 0; y < height; y++)
                for (var x = 0; x < width; x++)
                {
                    testAction(x, y, moves);
                }

            foreach (var move in moves)
            {
                moveAction(move);
            }

            return moves.Any();
        }
    }
}
