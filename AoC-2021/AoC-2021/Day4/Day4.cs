using AoC_2021.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC_2021.Day4
{
    [BasePath("Day4")]
    [TestFile(File = "example.txt", Name = "Example")]
    [TestFile(File = "Input.txt", Name = "Input")]
    public class Day4 : DayBase
    {
        public Day4(string filePath) : base(filePath)
        {
            this.numbers = this.LineInput[0].Trim().Split(",").Select(s => long.Parse(s)).ToList();
            var toBoard = new List<BingoBoard>();
            for (var i=2; i<this.LineInput.Count; i += 6)
            {
                toBoard.Add(new BingoBoard(this.LineInput.Skip(i).Take(5).ToArray()));
            }
            this.boards = toBoard;
        }
        private IReadOnlyList<long> numbers;
        private IReadOnlyList<BingoBoard> boards;

        [ExpectedResult(TestName = "Example", Result = "4512")]
        [ExpectedResult(TestName = "Input", Result = "38913")]
        public override string Part1()
        {
            foreach(var select in this.numbers)
                foreach(var board in this.boards)
                {
                    board.Select(select);
                    if (board.WonSet.Length > 0)
                    {
                        return $"{board.Unmarked.ToList().Sum()*select}";
                    }
                }

            return "end of method :(";
        }

        [ExpectedResult(TestName = "Example", Result = "1924")]
        [ExpectedResult(TestName = "Input", Result = "16836")]
        public override string Part2()
        {
            BingoBoard lastWon = this.boards.First();
            long lastNum = 0;
            var boards = this.boards.ToList();
            foreach (var select in this.numbers)
            {
                lastNum = select;
                foreach (var board in boards.Where(b => !b.Removed))
                {
                    board.Select(select);
                    if (board.WonSet.Length > 0)
                    {
                        lastWon = board;
                        board.Removed = true;
                    }
                }
                if (boards.All(b => b.Removed))
                    break;
            }
            var unmarkedSum = lastWon.Unmarked.ToList().Sum();
            return $"{ unmarkedSum * lastNum}";
        }

    }

    internal class BingoBoard
    {
        private IList<IList<BingoNum>> board = new List<IList<BingoNum>>();
        public bool Removed = false;
        public BingoBoard(string[] seed)
        {
            foreach(var line in seed)
            {
                var row = new List<BingoNum>();
                foreach(var cell in line.Trim().Split(" ").Where(s => s.Length > 0).Select(s => long.Parse(s)))
                {
                    row.Add(new BingoNum{ number = cell, marked = false });
                }
                this.board.Add(row);
            }
        }

        public long[] WonSet
        {
            get
            {
                if (this.board.Any(r => r.All(t => t.marked)))
                    return this.board.First(r => r.All(t => t.marked)).Select(t => t.number).ToArray();

                for (var i=0; i < 5; i++){
                    if (this.board.All(r => r[i].marked))
                        return this.board.Select(r => r[i].number).ToArray();
                }
                return new long[0];
            }
        }
        public IEnumerable<long> Unmarked
        {
            get
            {
                foreach(var row in this.board)
                    foreach(var cell in row.Where(r => !r.marked))
                        yield return cell.number;
            }
        }

        public void Select(long number)
        {
            foreach (var row in this.board)
                for (var i=0; i<5; i++)
                    if (row[i].number == number)
                        row[i] = new BingoNum{ number = number, marked = true} ;
        }

        private struct BingoNum
        {
            public long number;
            public bool marked;
        }
    }
}
