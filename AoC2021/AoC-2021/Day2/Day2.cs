using AoC_2021.Attributes;
using AoC_2021.InputClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC_2021.Day2
{
    [BasePath("Day2")]
    [TestFile(File = "example.txt", Name = "Example")]
    [TestFile(File = "Input.txt", Name = "Input")]
    public class Day2 : ParseableDay<Command>
    {
        public Day2(string path) : base(path) { }

        public override Command Parse(string input)
        {
            return new Command(input);
        }

        [ExpectedResult(TestName = "Example", Result = "150")]
        public override string Part1(string testName = null)
        {
            long x = 0;
            long y = 0;

            foreach(var cmd in this.Input)
            {
                switch (cmd.Direction)
                {
                    case Direction.FORWARD:
                        x += cmd.Distance;
                        break;
                    case Direction.UP:
                        y -= cmd.Distance;
                        break;
                    case Direction.DOWN:
                        y += cmd.Distance;
                        break;
                }
            }
            return (x * y).ToString();
        }

        [ExpectedResult(TestName = "Example", Result = "900")]
        public override string Part2(string testName = null)
        {
            long x = 0;
            long y = 0;
            long aim = 0;


            foreach (var cmd in this.Input)
            {
                switch (cmd.Direction)
                {
                    case Direction.FORWARD:
                        x += cmd.Distance;
                        y += -1* aim * cmd.Distance;
                        break;
                    case Direction.UP:
                        aim += cmd.Distance;
                        break;
                    case Direction.DOWN:
                        aim -= cmd.Distance;
                        break;
                }
            }
            return (x * y).ToString();
        }
    }

    public class Command
    {
        public readonly Direction Direction;
        public readonly long Distance;

        public Command(string command)
        {
            var tiles = command.Split(" ");
            switch (tiles[0].ToLower())
            {
                case "forward":
                    this.Direction = Direction.FORWARD;
                    break;
                case "up":
                    this.Direction = Direction.UP;
                    break;
                case "down":
                    this.Direction = Direction.DOWN;
                    break;
                default:
                    this.Direction = Direction.UNKNOWN;
                    break;
            }

            this.Distance = long.Parse(tiles[1]);
        }
    }

    public enum Direction
    {
        UNKNOWN,
        UP,
        DOWN,
        FORWARD
    }
}
