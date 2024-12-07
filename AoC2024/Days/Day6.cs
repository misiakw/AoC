﻿using AoC.Common.Maps.StaticMap;
using AoCBase2;

namespace AoC2024.Days
{
    internal class Day6 : IDay
    {
        [Flags]
        private enum Field
        {
            Empty = 0,
            Box = 1 << 0,
            //PreBump = 1 << 1,
            Up = 1 <<2,
            Right = 1 << 3,
            Down = 1 << 4,
            Left = 1 << 5,
        }
        private static class DIR
        {
            public static readonly (short, short) UP = (0, -1);
            public static readonly (short, short) DOWN = (0, 1);
            public static readonly (short, short) RIGHT = (1, 0);
            public static readonly (short, short) LEFT = (-1, 0);
        }
        private class Cell
        {
            public Cell(Field field)
            {
                Field = field;
            }
            public Field Field;
        }

        public static void RunAoC() => AocRuntime.Day<Day6>(6, t => new Day6(t.GetLines().ToArray()))
        .Callback(1, (d, t) => d.Part1())
        .Callback(2, (d, t) => d.Part2())
        .Test("example")
        .Test("input").Skip()
        //.Part(1).Correct(4752)
        //.Part(2).Correct(88802350)
        .Run();

        private readonly StaticMap<Cell> map;
        private (int, int) guardPos;
        private (short, short) movementDir = DIR.UP;

        public Day6(string[] lines)
        {
            //var lines = File.ReadAllLines(filePath);                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     
            map = new StaticMap<Cell>(lines[0].Length, lines.Length);

            for(var y=0; y<lines.Length; y++)
                for(var x=0; x < lines[y].Length; x++)
                    if(lines[y][x] == '^')
                    {
                        guardPos = (x, y);
                        map[x, y] = new Cell(Field.Up);
                    }
                    else if(lines[y][x] == '#')
                        map[x, y] = new Cell(Field.Box);
                    else
                        map[x,y] = new Cell(Field.Empty);
        }

        private IList<(long, long, Field)> part1Steps;
        public string Part1()
        {
            part1Steps = ProceedToOutput(guardPos, Field.Up);
            return part1Steps.DistinctBy(x => $"{x.Item1}|{x.Item2}")
                .Count().ToString();
        }

        public string Part2()
        {
            foreach (var step in part1Steps)
            {
                var next = (step.Item1, step.Item2, step.Item3 switch
                {
                    Field.Up => Field.Right,
                    Field.Right => Field.Down,
                    Field.Down => Field.Left,
                    Field.Left => Field.Up
                });
                Console.WriteLine(step.ToString()+" "+ next+" "+ProceedToOutput((next.Item1, next.Item2), next.Item3).Count);
                /*Console.WriteLine(ProceedToOutput((step.Item1, step.Item2), step.Item3 switch
                {
                    Field.Up => Field.Right,
                    Field.Right => Field.Down,
                    Field.Down => Field.Left,
                    Field.Left => Field.Up
                }).Count());*/
            }
            return null;
        }

        private IList<(long, long, Field)> ProceedToOutput((long, long) startingPoint, Field direction)
        {
            var steps = new List<(long, long, Field)>();

            var myPos = (startingPoint.Item1, startingPoint.Item2);

            var nx = myPos.Item1;
            var ny = myPos.Item2;
            do
            {
                if (map[nx, ny].Field.HasFlag(Field.Box))
                {
                    nx -= movementDir.Item1;
                    ny -= movementDir.Item2;
                    movementDir = movementDir switch
                    {
                        (0, -1) => DIR.RIGHT,
                        (1, 0) => DIR.DOWN,
                        (0, 1) => DIR.LEFT,
                        (-1, 0) => DIR.UP,
                        _ => DIR.UP
                    };
                }
                var moveFlag = FromMovement(movementDir);
                //map[nx, ny].Field |= moveFlag;
                steps.Add((myPos.Item1, myPos.Item2, moveFlag));

                myPos = (nx, ny);
                nx = myPos.Item1 + movementDir.Item1;
                ny = myPos.Item2 + movementDir.Item2;
                if(steps.Any(s => s.Item1 == nx && s.Item2 == ny && s.Item3 == moveFlag))
                {
                    Console.WriteLine("loop");
                    return null;
                }
            } while (nx >= 0 && nx < map.Width && ny >= 0 && ny < map.Height);

            steps.Add((myPos.Item1, myPos.Item2, FromMovement(movementDir)));
            return steps;
        }


        private Field FromMovement((short, short) movement) => movement switch
        {
            (0, -1) => Field.Up,
            (1, 0) => Field.Right,
            (0, 1) => Field.Down,
            (-1, 0) => Field.Left
        };
    }
}
