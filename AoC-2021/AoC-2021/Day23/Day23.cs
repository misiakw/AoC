using AoC_2021.Attributes;
using AoC_2021.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC_2021.Day23
{
    [BasePath("Day23")]
    [TestFile(File = "example.txt", Name = "Example", TestToProceed = TestCase.Part1)]
    [TestFile(File = "Input.txt", Name = "Input", TestToProceed = TestCase.Part1)]
    [TestFile(File = "exampleExtended.txt", Name = "ExampleExtended", TestToProceed = TestCase.Part2)]
    [TestFile(File = "InputExtended.txt", Name = "InputExtended", TestToProceed = TestCase.Part2)]
    public class Day23 : DayBase
    {
        public Day23(string filePath) : base(filePath)
        {
        }

        [ExpectedResult(TestName = "Example", Result = "12521")]
        [ExpectedResult(TestName = "Input", Result = "15111")] //result by no coding, just work on paper :/
        public override string Part1(string testName)
        {
            var capacity = 2;
            CaveStateExtensions.MinVal = long.MaxValue;

            var initialState = new CaveState()
            {
                Hallway = "...........",
                Value = 0,
                Rooms = new char[][] { new char[capacity], new char[capacity], new char[capacity], new char[capacity] },
                History = new List<string>()
            };

            for (var y = 2; y < 2 + capacity; y++)
            {
                var line = LineInput[y].Replace("#", "");
                initialState.Rooms[0][y - 2] = line[0];
                initialState.Rooms[1][y - 2] = line[1];
                initialState.Rooms[2][y - 2] = line[2];
                initialState.Rooms[3][y - 2] = line[3];
            }

            initialState.Solve();

            return CaveStateExtensions.MinVal.ToString();
        }

        [ExpectedResult(TestName = "ExampleExtended", Result = "44169")]
        [ExpectedResult(TestName = "InputExtended", Result = "47625")]
        public override string Part2(string testName)
        {
            var capacity = 4;
            CaveStateExtensions.MinVal = long.MaxValue;

            var initialState = new CaveState()
            {
                Hallway = "...........",
                Value = 0,
                Rooms = new char[][] { new char[capacity], new char[capacity], new char[capacity], new char[capacity] },
                History = new List<string>()
            };

            for (var y = 2; y < 2 + capacity; y++)
            {
                var line = LineInput[y].Replace("#", "");
                initialState.Rooms[0][y - 2] = line[0];
                initialState.Rooms[1][y - 2] = line[1];
                initialState.Rooms[2][y - 2] = line[2];
                initialState.Rooms[3][y - 2] = line[3];
            }

            initialState.Solve();

            return CaveStateExtensions.MinVal.ToString();
        }
    }
    internal struct CaveState
    {
        public string Hallway;
        public char[][] Rooms;
        public long Value;
        public IList<string> History;
    }
    internal static class CaveStateExtensions
    {
        public static long MinVal = long.MaxValue;
        private static readonly int[] multipliers = new int[] {1,10,100, 1000};
        public  static IList<string> MinSteps;
        public static CaveState Clone(this CaveState source)
        {
            var capacity = source.Rooms[0].Length;
            var result = new CaveState()
            {
                Hallway = source.Hallway,
                Value = source.Value,
                Rooms = new char[][] { new char[capacity], new char[capacity], new char[capacity], new char[capacity] },
                History = source.History.ToList()
            };
            for (var y = 0; y < capacity; y++)
            {
                result.Rooms[0][y] = source.Rooms[0][y];
                result.Rooms[1][y] = source.Rooms[1][y];
                result.Rooms[2][y] = source.Rooms[2][y];
                result.Rooms[3][y] = source.Rooms[3][y];
            }
            return result;
        }
        public static char[] GetRoom(this CaveState state, char color) => state.Rooms[color - 'A'];
        public static int RoomX(this char room) => 2 + (room - 'A') * 2;
        public static bool CanReceive(this CaveState state, char color) => !state.GetRoom(color).Any(c => c != '.' && c != color);
        public static bool RoomIsClosed(this CaveState state, char color) => state.GetRoom(color).All(a => a == color);
        public static bool IsFinished(this CaveState state) => "ABCD".All(room => state.RoomIsClosed(room));
        public static bool CanGoThru(this CaveState state, int from, int to) => (from > to)
                ? state.Hallway.Substring(to, from - to).All(c => c == '.')
                : state.Hallway.Substring(from + 1, to - from).All(c => c == '.');
        public static void AddSteps(this ref CaveState state, char color, int steps) => state.Value += multipliers[color - 'A'] * steps;

        public static bool MoveAnyBetweenRooms(this ref CaveState state)
        {
            foreach (var room in "ABCD")
                if (state.MovedBetwenRooms(room))
                    return true;
            return false;
        }
        public static bool MoveAnyFromHallaway(this ref CaveState state)
        {
            for (var x = 0; x < state.Hallway.Length; x++)
                if (state.Hallway[x] != '.')
                {
                    var ami = state.Hallway[x];
                    if (!state.CanReceive(ami)) continue; //room cannot receive
                    var roomX = ami.RoomX();
                    if (!state.CanGoThru(x, roomX)) continue; //road is blocked

                    state.MoveThruHallaway(x, roomX);
                    state.EnterRoom(ami);
                    state.History.Add($"{x}[{ami}] to dest");
                    return true;
                }
            return false;
        }
        public static IList<long> MoveAnyFromRoom(this ref CaveState state)
        {
            var result = new List<long>();
            var lockedPositions = new int[4] { 2, 4, 6, 8 };

            foreach (var room in "ABCD")
            {
                if (state.RoomIsClosed(room)) continue; //room is closed
                if (state.GetRoom(room)[0] == '.') continue; //no element to leave room

                var roomX = room.RoomX();
                var ami = state.GetRoom(room)[0];

                var proceedLeft = state.Hallway[roomX - 1] == '.';
                var proceedRight = state.Hallway[roomX + 1] == '.'; ;
                for (var d = 1; proceedLeft || proceedRight; d++)
                {
                    proceedLeft = (roomX - d >= 0 && state.Hallway[roomX - d] == '.');
                    proceedRight = (roomX + d < state.Hallway.Length && state.Hallway[roomX + d] == '.');
                    if (proceedLeft && !lockedPositions.Contains(roomX - d) && state.CanGoThru(roomX, roomX - d))
                    {
                        var newState = state.Clone();
                        newState.LeaveRoom(room);
                        newState.MoveThruHallaway(roomX, roomX - d);
                        newState.History.Add($"{room}[{ami}] to {roomX - d} <" );
                        foreach (var res in newState.Solve())
                            result.Add(res);
                    }
                    if (proceedRight && !lockedPositions.Contains(roomX + d)&& state.CanGoThru(roomX, roomX + d))
                    {
                        var newState = state.Clone();
                        newState.LeaveRoom(room);
                        newState.MoveThruHallaway(roomX, roomX + d);
                        newState.History.Add($"{room}[{ami}] to {roomX + d} >");
                        foreach (var res in newState.Solve())
                            result.Add(res);
                    }
                }
            }

            return result;
        }
        public static void SetHallaway(this ref CaveState state, int x, char c)
        {
            state.Hallway = $"{state.Hallway.Substring(0, x)}{c}{state.Hallway.Substring(x + 1)}";
        }
        public static void LeaveRoom(this ref CaveState state, char color)
        {
            var room = state.GetRoom(color);
            if (state.RoomIsClosed(color)) throw new Exception("Should not leave empty room");
            var roomX = color.RoomX();
            if (room[0] == '.') throw new Exception("Leaving empty room");
            state.SetHallaway(roomX, room[0]);
            state.AddSteps(room[0], 1);
            room[0] = '.';

            var bottomIndex = room.Length - 1;
            while (room[bottomIndex] == color) bottomIndex--; //defne bottom to leave valid color at bottom
            bottomIndex++;

            for (var i = 1; i < bottomIndex && room[i] != '.'; i++)
            {
                state.AddSteps(room[i], 1);
                room[i - 1] = room[i];
                room[i] = '.';
            }
        }
        public static void EnterRoom(this ref CaveState state, char color)
        {
            if (!state.CanReceive(color)) throw new Exception("room cannot receive elements");
            var room = state.GetRoom(color);
            var roomX = color.RoomX();
            if (state.Hallway[roomX] != color) throw new Exception("invalid color stands in front of room");

            var targetY = 0;
            for (; targetY < room.Length && room[targetY] == '.'; targetY++) ;
            targetY--;

            state.SetHallaway(roomX, '.');
            room[targetY] = color;
            state.AddSteps(color, targetY + 1);
        }
        public static void MoveThruHallaway(this ref CaveState state, int start, int end)
        {
            var ami = state.Hallway[start];
            var newHallaway = string.Empty;

            for (var x = 0; x < state.Hallway.Length; x++)
            {
                if (x == start) newHallaway = $"{newHallaway}.";
                else if (x == end) newHallaway = $"{newHallaway}{ami}";
                else newHallaway = $"{newHallaway}{state.Hallway[x]}";
            }
            state.Hallway = newHallaway;
            var steps = end > start ? end - start : start - end;
            state.AddSteps(ami, steps);
        }
        public static bool MovedBetwenRooms(this ref CaveState state, char color)
        {
            var source = state.GetRoom(color);
            var ami = source[0];
            if (ami == color) return false; //already in correct room
            if (!"ABCD".Contains(ami)) return false; //moving out of empty room
            if (!state.CanReceive(ami)) return false; //Moving to ocupied room;

            int sx = color.RoomX();
            int fx = ami.RoomX();
            if (!state.CanGoThru(sx, fx)) return false; //hallaway blocked

            state.LeaveRoom(color);
            state.MoveThruHallaway(sx, fx);
            state.EnterRoom(ami);
            state.History.Add($"{color}[{ami}] to dest");
            return true;
        }

        public static IList<long> Solve(this ref CaveState state)
        {
            if (state.Value >= MinVal)
                return new List<long>();

            if (!state.IsValidState())
            {
                throw new Exception("invalid state");
            }
            if (state.IsFinished())
            {
                if (state.Value < MinVal)
                {
                    MinVal = state.Value;
                    MinSteps = state.History.ToList();
                }
                return new List<long>(){
                   state.Value
                };
            }

            if (state.MoveAnyBetweenRooms())
                return state.Solve();

            if (state.MoveAnyFromHallaway())
                return state.Solve();

            return state.MoveAnyFromRoom();
        }

        public static bool IsValidState(this CaveState state)
        {
            foreach (var c in "ABCD")
            {
                var count = state.Hallway.Where(ch => ch == c).Count();
                count += state.Rooms[0].Where(ch => ch == c).Count();
                count += state.Rooms[1].Where(ch => ch == c).Count();
                count += state.Rooms[2].Where(ch => ch == c).Count();
                count += state.Rooms[3].Where(ch => ch == c).Count();
                if (count != state.Rooms[0].Length)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
