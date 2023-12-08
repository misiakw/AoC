using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Base;
using AoC.Base.TestInputs;
using AoC.Common;


namespace AoC2023.Days
{
    public class Day7 : AbstractDay<long, IComparableInput<long>>
    {
        public override void PrepateTests(InputBuilder<long, IComparableInput<long>> builder)
        {
            builder.New("example1", "./Inputs/Day7/example1.txt")
                .Part1(6440)
                .Part2(5905);
            builder.New("output", "./Inputs/Day7/output.txt")
                .Part1(250602641)
                .Part2(251037509);
        }

        public override long Part1(IComparableInput<long> input)
        {
            var tmp = new List<(Hand, long)>();
            foreach (var line in ReadLines(input).Select(s => s.Split(" ")))
                tmp.Add((new Hand(line[0]), long.Parse(line[1])));

            long score = 0;
            long i = tmp.Count;
            foreach (var hand in tmp.OrderByDescending(t => t.Item1))
                score += i-- * hand.Item2;

            return score;
        }

        public override long Part2(IComparableInput<long> input)
        {
            var tmp = new List<(Hand, long)>();
            foreach (var line in ReadLines(input).Select(s => s.Split(" ")))
                tmp.Add((new Hand(line[0], true), long.Parse(line[1])));

            long score = 0;
            long i = tmp.Count;
            foreach (var hand in tmp.OrderByDescending(t => t.Item1))
                score += i-- * hand.Item2;

            return score;
        }

        private class Hand : IComparable<Hand>
        {
            public readonly char[] Cards;
            private Dictionary<char, int> CardCounts = new Dictionary<char, int>();
            private readonly int jockers = 0;
            public readonly int Score = 0;
            private bool isPart2;

            public Hand(string cards, bool isPart2 = false)
            {
                this.isPart2 = isPart2;
                this.Cards = cards.ToArray();
                foreach (var c in cards)
                {
                    if (isPart2 && c == 'J')
                        jockers++;
                    else
                    {
                        if (!CardCounts.ContainsKey(c))
                            CardCounts.Add(c, 0);
                        CardCounts[c] += 1;
                    }

                    Score = Score << 4;
                    Score += CardToScore(c);
                }
                if (Five)
                    Score += 1 << 26;
                else if (Four)
                    Score += 1 << 25;
                else if (House)
                    Score += 1 << 24;
                else if (Three)
                    Score += 1 << 23;
                else if (TwoPair)
                    Score += 1 << 22;
                else if (OnePair)
                    Score += 1 << 21;
            }


            public bool OnePair => CardCounts.Max(c => c.Value) + jockers >= 2;
            public bool TwoPair
            {
                get
                {
                    var counts = CardCounts.Values.OrderByDescending(v => v).Take(2).ToArray();

                    var pair1 = counts[0] + jockers >= 2;
                    var pair2 = counts[1] + (jockers - (2 - counts[0])) >= 2;
                    return pair1 && pair2;
                }
            }
            public bool Three => CardCounts.Max(c => c.Value) + jockers >= 3;
            public bool House{
                get {
                    var counts = CardCounts.Values.OrderByDescending(v => v).Take(2).ToArray();

                    var get3 = counts[0] + jockers >= 3;
                    var get2 = counts[1] + (jockers - (3 - counts[0])) >= 2;
                    return get3 && get2;
                }
            }

            public bool Four => jockers == 5 || CardCounts.Max(c => c.Value) + jockers >= 4;

            public bool Five => jockers == 5 || CardCounts.Count(c => c.Value == 5-jockers) == 1;

                

            private int CardToScore(char c)
                => (c >= '0' && c <= '9') ? c - '0' : c switch
                {
                    'T' => 10,
                    'J' => isPart2 ? 1 : 11,
                    'Q' => 12,
                    'K' => 13,
                    'A' => 14,
                    _ => 0
                };

            public int CompareTo(Hand other)
                => Score.CompareTo(other.Score);

        }
    }
}
