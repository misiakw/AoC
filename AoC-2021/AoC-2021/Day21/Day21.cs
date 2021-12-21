using AoC_2021.Attributes;
using AoC_2021.InputClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC_2021.Day21
{
    [BasePath("Day21")]
    [TestFile(File = "example.txt", Name = "Example")]
    [TestFile(File = "Input.txt", Name = "Input")]
    public class Day21 : ParseableDay<long>
    {
        public Day21(string path) : base(path)
        {
            var tmp = new List<int>();
            for (var x = 1; x < 4; x++)
                for (var y = 1; y < 4; y++)
                    for (var z = 1; z < 4; z++)
                        tmp.Add(x + y + z);
            var throws = new Dictionary<int, int>();
            foreach (var group in tmp.GroupBy(t => t))
                throws.Add(group.Key, group.Count());
            diracThrows = throws;
        }

        //key is result, value is how often it is in real world
        protected readonly IReadOnlyDictionary<int, int> diracThrows;

        public override long Parse(string input)
        {
            return long.Parse(input.Substring(28).Trim());
        }

        [ExpectedResult(TestName = "Example", Result = "739785")]
        [ExpectedResult(TestName = "Input", Result = "571032")]
        public override string Part1(string testName)
        {
            var dice = new Dice();
            var score = new long[2] { 0, 0 };
            var pos = Input.ToArray();
            var current = 1;

            while(score[current] < 1000)
            {
                current = (current + 1) % 2;
                var move = dice.Throw() + dice.Throw() + dice.Throw();
                pos[current] += move;
                if (pos[current] > 10) pos[current] = pos[current] % 10 == 0 ? 10 : pos[current] % 10;
                score[current] += pos[current];
            }

            var result = score[(current + 1) % 2] * dice.RollCount;

            return result.ToString();
        }

        [ExpectedResult(TestName = "Example", Result = "444356092776315")]
        [ExpectedResult(TestName = "Input", Result = "49975322685009")]
        public override string Part2(string testName)
        {
            var initState = new GameState()
            {
                pos = Input.Select(i => (int)i).ToArray(),
                score = new int[2] { 0, 0 },
                current = 0
            };

            var result = ForkThrows(initState);

            return result.Max().ToString();
        }

        private class Dice
        {
            private int value = 1;
            public long RollCount { get; private set; } = 0;
            public long Throw()
            {
                var tmp = value;
                value++;
                RollCount++;
                if (value == 101) value = 1;
                return tmp;
            }
        }

        private long[] ForkThrows(GameState state)
        {
            if (state.score[0] >= 21)
                return new long[2] { 1, 0 }; 
            if (state.score[1] >= 21)
                return new long[2] { 0, 1 };

            var result = new long[2] { 0, 0 };

            foreach(var kv in diracThrows)
            {
                var newState = new GameState()
                {
                    score = state.score.ToArray(),
                    current = (state.current+1)%2,
                    pos = state.pos.ToArray()
                };

                newState.pos[state.current] += kv.Key;
                if (newState.pos[state.current] > 10) newState.pos[state.current] = (newState.pos[state.current] % 10 == 0) ? 10 : newState.pos[state.current] % 10;
                newState.score[state.current] += newState.pos[state.current];

                var results = ForkThrows(newState);
                result[0] += results[0] * kv.Value;
                result[1] += results[1] * kv.Value;
            }

            return result;
        }

        private struct GameState
        {
            public int[] pos;
            public int[] score;
            public int current;
        }
    }
}
