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
    [TestFile(File = "example.txt", Name = "Example")]
    [TestFile(File = "Input.txt", Name = "Input")]
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

            var initialState = new CaveState()
            {
                Hallway = "...........",
                Steps = new int[4] { 0, 0, 0, 0 },
                Rooms = new string[4] { string.Empty , string.Empty , string.Empty , string.Empty }
            };
            
            for (var y = 2; y < 2 + capacity; y++)
            {
                var line = LineInput[y].Replace("#", "");
                initialState.Rooms[0] += $"{line[0]}";
                initialState.Rooms[1] += $"{line[1]}";
                initialState.Rooms[2] += $"{line[2]}";
                initialState.Rooms[3] += $"{line[3]}";
            }

            var results = initialState.Solve();

            return results.Min().ToString();
        }

        [ExpectedResult(TestName = "Example", Result = "44169")]
        [ExpectedResult(TestName = "Input", Result = "")]
        public override string Part2(string testName)
        {
            throw new NotImplementedException();
        }
    }
    internal struct CaveState
    {
        public string Hallway;
        public string[] Rooms;
        public int[] Steps;
    }
    internal static class CaveStateExtensions
    {
        public static CaveState Clone(this CaveState source)
        {
            return new CaveState()
            {
                Hallway = source.Hallway,
                Steps = new int[4] { source.Steps[0] , source.Steps[1] , source.Steps[2] , source.Steps[3] },
                Rooms = new string[4] { source.Rooms[0] , source.Rooms[1] , source.Rooms[2] , source.Rooms[3] }
            };
        }
        public static char[] GetRoom(this CaveState state, char color) => state.Rooms[color - 'A'].ToCharArray();
        public static int RoomX(this char room) => 2 + (room - 'A') * 2;
        public static bool CanReceive(this CaveState state, char color) => !state.GetRoom(color).Any(c => c != '.' && c != color);
        public static bool RoomIsClosed(this CaveState state, char color) => state.GetRoom(color).All(a => a == color);
        public static bool IsFinished(this CaveState state) => "ABCD".All(room => state.RoomIsClosed(room));
        public static bool CanGoThru(this CaveState state, int from, int to) => (from > to)
                ? state.Hallway.Substring(to, from - to-1).All(c => c == '.')
                : state.Hallway.Substring(from, to - from-1).All(c => c == '.');
        public static void AddSteps(this CaveState state, char color, int steps) => state.Steps[color - 'A'] += steps;

        public static bool MoveAnyBetweenRooms(this CaveState state)
        {
            foreach (var room in "ABCD")
                if (state.MovedBetwenRooms(room))
                    return true;
            return false;
        }
        public static bool MoveAnyFromHallaway(this CaveState state)
        {
            for(var x=0; x<state.Hallway.Length; x++)
                if (state.Hallway[x] != '.')
                {
                    var ami = state.Hallway[x];
                    if (!state.CanReceive(ami)) continue; //room cannot receive
                    var roomX = ami.RoomX();
                    if (!state.CanGoThru(x, roomX)) continue; //road is blocked

                    state.MoveThruHallaway(x, roomX);
                    state.EnterRoom(ami);
                    return true;
                }
            return false;
        }
        public static IList<long> MoveAnyFromRoom(this CaveState state)
        {
            var result = new List<long>();
            var lockedPositions = new int[4] { 2, 4, 6, 8 };

            foreach (var room in "ABCD")
            {
                if (state.RoomIsClosed(room)) continue; //room is closed
                var ami = state.GetRoom(room)[0];
                var roomX = room.RoomX();

                var proceedLeft = state.Hallway[roomX - 1] == '.';
                var proceedRight = state.Hallway[roomX + 1] == '.'; ;
                for(var d=1; proceedLeft || proceedRight; d++)
                {
                    if (proceedLeft && !lockedPositions.Contains(roomX - d))
                    {

                        var newState = state.Clone();
                        newState.LeaveRoom(room);
                        newState.MoveThruHallaway(roomX, roomX - d);
                        foreach (var res in newState.Solve())
                            result.Add(res);

                        proceedLeft = (roomX - d - 1 >= 0 && state.Hallway[roomX - d - 1] == '.');
                    }
                    if (proceedRight && !lockedPositions.Contains(roomX + d))
                    {

                        var newState = state.Clone();
                        newState.LeaveRoom(room);
                        newState.MoveThruHallaway(roomX, roomX + d);
                        foreach (var res in newState.Solve())
                            result.Add(res);

                        proceedRight = (roomX + d + 1 < state.Hallway.Length && state.Hallway[roomX + d + 1] == '.');
                    }
                }
            }

            return result;
        }

        public static void LeaveRoom(this CaveState state, char color)
        {
            var roomX = color.RoomX();
            var room = state.GetRoom(color);
            if (room[0] == '.') throw new Exception("Leaving empty room");
            state.Hallway = $"{state.Hallway.Substring(0, roomX - 1)}{room[0]}{state.Hallway.Substring(roomX + 1)}";

            state.AddSteps(room[0], 1);
            for(var i=1; i<room.Length && room[i] != '.'; i++)
            {
                state.AddSteps(room[i], 1);
                room[i - 1] = room[i];
                room[i] = '.';
            }
        }
        public static void EnterRoom(this CaveState state, char color)
        {
            if (!state.CanReceive(color)) throw new Exception("room cannot receive elements");
            var room = state.GetRoom(color);
            var roomX = color.RoomX();
            if (state.Hallway[roomX] != color) throw new Exception("invalid color stands in front of room");

            var targetY = 0;
            for (; targetY < room.Length && room[targetY] != '.'; targetY++) ;
            targetY--;
            //ToDo: implement proper movement
        }
        public static void MoveThruHallaway(this CaveState state, int start, int end)
        {
            var ami = state.Hallway[start];
            var newHallaway = string.Empty;

            for(var x = 0; x < state.Hallway.Length; x++)
            {
                if (x == start) newHallaway = $"{newHallaway}.";
                else if (x == end) newHallaway = $"{newHallaway}{ami}";
                else newHallaway = $"{newHallaway}{state.Hallway[x]}";
            }
            state.Hallway = newHallaway;
            var steps = end > start ? end - start : start - end;
            state.AddSteps(ami, steps);
        }
        public static bool MovedBetwenRooms(this CaveState state, char color)
        {
            var source = state.GetRoom(color);
            var ami = source[0];
            if (!"ABCD".Contains(ami)) return false; //moving out of empty room
            if (!state.CanReceive(ami)) return false; //Moving to ocupied room;

            int sx = color.RoomX();
            int fx = ami.RoomX();
            if (!state.CanGoThru(sx, fx)) return false; //hallaway blocked

            state.LeaveRoom(color);
            state.MoveThruHallaway(sx, fx);
            state.EnterRoom(ami);
            return true;
        }

        public static IList<long> Solve(this CaveState state)
        {
            if (state.IsFinished())
                return new List<long>(){
                    state.Steps[0] + state.Steps[1]*10 + state.Steps[2]*100 + state.Steps[3]*1000
                };
            
            if (state.MoveAnyBetweenRooms())
                return state.Solve();

            if (state.MoveAnyFromHallaway())
                return state.Solve();

            return state.MoveAnyFromRoom();
        }
    }
}
