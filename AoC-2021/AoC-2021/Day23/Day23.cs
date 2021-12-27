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
                Hallway = "..X.X......",
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

            initialState.CanGoThru(3, 5);
            throw new NotImplementedException();
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
        public static char[] GetRoom(this CaveState state, char color)
        {
            return state.Rooms[color - 'A'].ToCharArray();
        }
        public static bool CanReceive(this CaveState state, char color) => !state.GetRoom(color).Any(c => c != '.' && c != color);
        public static bool CanGoThru(this CaveState state, int from, int to)
        {
            return (from > to)
                ? state.Hallway.Substring(to, from - to-1).All(c => c == '.')
                : state.Hallway.Substring(from, to - from-1).All(c => c == '.');
        }
        public static void AddSteps(this CaveState state, char color, int steps) => state.Steps[color - 'A'] += steps;
        public static bool MovedBetwenRooms(this CaveState state, char color)
        {
            var source = state.GetRoom(color);
            var ami = source[0];
            if (!"ABCD".Contains(ami)) return false; //moving out of empty room
            if (!state.CanReceive(ami)) return false; //Moving to ocupied room;

            int sx = 3 + (color - 'A') * 2;
            int fx = 3 + (color - 'A') * 2;
            if (!state.CanGoThru(sx, fx)) return false; //hallaway blocked

            int steps = 1; //mooving out of room

            return false;
        }
    }
}
