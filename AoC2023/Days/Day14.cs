using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AoC.Base;
using AoC.Base.TestInputs;
using AoC.Common;
using AoC.Common.Maps;

namespace AoC2023.Days
{
    public class Day14 : AbstractDay<long, MapInput<long>>
    {
        public override void PrepateTests(InputBuilder<long, MapInput<long>> builder)
        {
            builder.New("./Inputs/Day14/example1.txt", "example1")
                .Part1(136)
                .Part2(64);
            builder.New("./Inputs/Day14/output.txt", "output")
                .Part1(107951)
                .Part2(95736);
        }

        public override long Part1(MapInput<long> input)
        {
            var platform = new Platform(input.GetMap());
            platform.ShiftNorth();
            return platform.BeamWeights;
        }

        public override long Part2(MapInput<long> input)
        {
            var platform = new Platform(input.GetMap());

            var states = new Dictionary<string, long>();
            var currentLoop = 0L;
            var loopSize = 0L;
            var loopsToDo = 1000000000;
            var gotLoop = false;

            for (var i = 0L; i < loopsToDo; i++)
            {
                var state = string.Empty;
                platform.ShiftNorth();
                state += $"{platform.BeamWeights}|";
                platform.ShiftWest();
                state += $"{platform.BeamWeights}|";
                platform.ShiftSouth();
                state += $"{platform.BeamWeights}|";
                platform.ShiftEast();
                state += $"{platform.BeamWeights}|";
                if (!gotLoop)
                {
                    if (states.ContainsKey(state))
                    {
                        gotLoop = true;
                        loopSize = i - states[state];

                        while(loopsToDo - i > loopSize)
                             i += loopSize;
                    }
                    else
                        states.Add(state, i);
                }
            }

            return platform.BeamWeights;
        }

        private class Platform
        {
            private IMap<char> map;
            private IList<(long, long)> movingPositions = new List<(long, long)>();
            private IDictionary<long, IList<long>> collumnBlocks = new Dictionary<long, IList<long>>();
            private IDictionary<long, IList<long>> rowsBlocks = new Dictionary<long, IList<long>>();

            public Platform(IMap<char> inputMap)
            {
                map = inputMap;
                for (var y = 0; y < map.Height; y++)
                    rowsBlocks.Add(y, new List<long>());
                for (var x = 0; x < map.Width; x++)
                    collumnBlocks.Add(x, new List<long>());

                for (var y = 0; y < map.Height; y++)
                    for (var x = 0; x < map.Width; x++)
                    {
                        if (map[x, y] != '.')
                        {
                            if (map[x, y] == 'O')
                                movingPositions.Add((x, y));

                            collumnBlocks[x].Add(y);
                            rowsBlocks[y].Add(x);
                        }
                    }
            }

            public long Rocks
            {
                get
                {
                    var result = 0L;
                    for (var y = 0; y < map.Height; y++)
                        for (var x = 0; x < map.Width; x++)
                            if (map[x, y] == 'O')
                                result++;
                    return result;
                }
            }

            public long BeamWeights
            {
                get
                {
                    var result = 0L;
                    for (var y = 0; y < map.Height; y++)
                        result += BeamWeight(y);
                    return result;
                }
            }

            private long BeamWeight(int y)
            {
                var result = 0;
                for (var x = 0; x < map.Width; x++)
                    if (map[x, y] == 'O')
                        result++;
                return result * (map.Height - y);
            }

            public void Print() => Console.WriteLine(map.Draw(c => $"{c}"));

            private (long, long) ShiftInCollumm(long currY, long newY, long collumn, Func<long, long, bool> shuldShift)
            {
                if (shuldShift(newY, currY))
                {//yay, move
                    map[collumn, currY] = '.';
                    map[collumn, newY] = 'O';

                    rowsBlocks[currY].Remove(collumn);
                    rowsBlocks[newY].Add(collumn);
                    collumnBlocks[collumn].Remove(currY);
                    collumnBlocks[collumn].Add(newY);

                    return (collumn, newY);
                }
                else //nope, stay
                    return (collumn, currY);
            }

            private (long, long) ShiftInRow(long currX, long newX, long row, Func<long, long, bool> shuldShift)
            {
                if (shuldShift(newX, currX))
                {//yay, move
                    map[currX, row] = '.';
                    map[newX, row] = 'O';

                    rowsBlocks[row].Remove(currX);
                    rowsBlocks[row].Add(newX);
                    collumnBlocks[currX].Remove(row);
                    collumnBlocks[newX].Add(row);

                    return (newX, row);
                }
                else //nope, stay
                    return (currX, row);
            }

            public void ShiftNorth()
            {
                var newPositions = new List<(long, long)>();
                foreach (var rock in movingPositions.OrderBy(p => p.Item2).ThenBy(p => p.Item1))
                {
                    var collumn = rock.Item1;
                    var currY = rock.Item2;
                    var blockers = collumnBlocks[collumn].Where(y => y < currY).ToList();
                    var newY = !blockers.Any() ? 0 : blockers.Max() + 1;
                    newPositions.Add(ShiftInCollumm(currY, newY, collumn, (newY, currY) => newY < currY));
                }
                movingPositions = newPositions;
            }

            public void ShiftSouth()
            {
                var newPositions = new List<(long, long)>();
                foreach (var rock in movingPositions.OrderByDescending(p => p.Item2).ThenBy(p => p.Item1))
                {
                    var collumn = rock.Item1;
                    var currY = rock.Item2;
                    var blockers = collumnBlocks[collumn].Where(y => y > currY).ToList();
                    var newY = !blockers.Any() ? map.Height - 1 : blockers.Min() - 1;
                    newPositions.Add(ShiftInCollumm(currY, newY, collumn, (newY, currY) => newY > currY));
                }
                movingPositions = newPositions;
            }

            public void ShiftWest()
            {
                var newPositions = new List<(long, long)>();
                foreach (var rock in movingPositions.OrderBy(p => p.Item2).ThenBy(p => p.Item1))
                {
                    var row = rock.Item2;
                    var currX = rock.Item1;
                    var blockers = rowsBlocks[row].Where(x => x < currX).ToList();
                    var newX = !blockers.Any() ? 0 : blockers.Max() + 1;
                    newPositions.Add(ShiftInRow(currX, newX, row, (newX, currX) => newX < currX));
                }
                movingPositions = newPositions;
            }
            public void ShiftEast()
            {
                var newPositions = new List<(long, long)>();
                foreach (var rock in movingPositions.OrderBy(p => p.Item2).ThenByDescending(p => p.Item1))
                {
                    var row = rock.Item2;
                    var currX = rock.Item1;
                    var blockers = rowsBlocks[row].Where(x => x > currX).ToList();
                    var newX = !blockers.Any() ? map.Width-1 : blockers.Min() +- 1;
                    newPositions.Add(ShiftInRow(currX, newX, row, (newX, currX) => newX > currX));
                }
                movingPositions = newPositions;
            }
        }

    }
}
