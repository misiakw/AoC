using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using AoC.Base;
using AoC.Base.TestInputs;
using AoC.Common;


namespace AoC2023.Days
{
    public class Day4 : AbstractDay<int, IComparableInput<int>>
    {
        public override int Part1(IComparableInput<int> input)
        {
            var score = 0;
            foreach (var line in ReadLines(input))
            {
                var card = new Card(line);
                if (card.Score > 0)
                    score += Convert.ToInt32(Math.Pow(2d, card.Score - 1));
            }
            return score;
        }

        public override int Part2(IComparableInput<int> input)
        {
            var cards = new Dictionary<int, Card>();
            var amounts = new Dictionary<int, int>();
            foreach (var line in ReadLines(input))
            {
                var card = new Card(line);
                cards.Add(card.Num, card);
                amounts.Add(card.Num, 1);
            }

            foreach(var kv in cards)
            {
                var amount = amounts[kv.Key];
                var card = kv.Value;
                for (var i = card.Num+1; i <= card.Num + card.Score; i++)
                    amounts[i] += amount;
            }

            return amounts.Sum(kv => kv.Value);
        }

        public override void PrepateTests(InputBuilder<int, IComparableInput<int>> builder)
        {
            builder.New("example1", "./Inputs/Day4/example1.txt")
                .Part1(13)
                .Part2(30);
            builder.New("output", "./Inputs/Day4/output.txt")
                .Part1(15205)
                .Part2(6189740);
        }

        private class Card
        {
            public IList<int> winning { get; }
            public IList<int> obtained { get; }
            public int Num { get; }
            public Card(string line)
            {
                var x = line.Trim().Split(":");
                Num = int.Parse(x[0].Split(" ").Last());
                var parts = x[1].Split("|");
                winning = parts[0].Split(" ")
                    .Select(s => s.Trim())
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .Select(int.Parse)
                    .ToList();

                obtained = parts[1].Split(" ")
                    .Select(s => s.Trim())
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .Select(int.Parse)
                    .ToList();
            }
            public int Score => obtained.Count(i => winning.Contains(i));
        }
    }
}
