using AoC_2021.Attributes;
using AoC_2021.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC_2021.Day11
{
    [BasePath("Day11")]
    [TestFile(File = "example.txt", Name = "Example")]
    [TestFile(File = "Input.txt", Name = "Input")]
    public class Day11 : DayBase
    {
        public Octopuss[,] Octopusses = new Octopuss[10, 10];
        public IList<Octopuss> OctoList = new List<Octopuss>();
        private long flashes = 0;
        private long fullFlash = -1;

        public Day11(string filePath) : base(filePath)
        {
            var i = 0;
            foreach (var line in this.LineInput)
                foreach (var ch in line)
                {
                    var octo = new Octopuss(ch - '0');
                    octo.Flash += (s, a) => { flashes++; };
                    Octopusses[i % 10, i++ / 10] = octo;
                    OctoList.Add(octo);
                }

            for (var y = 0; y < 10; y++)
                for (var x = 0; x < 10; x++)
                {
                    int[] rangeX = { x - 1, x, x + 1 };
                    int[] rangeY = { y - 1, y, y + 1 };
                    foreach (var dx in rangeX.Where(x => x >= 0 && x < 10))
                        foreach (var dy in rangeY.Where(y => y >= 0 && y < 10))
                        {
                            if (dx == x && dy == y) continue;
                            var octo1 = Octopusses[x, y];
                            var octo2 = Octopusses[dx, dy];
                            octo1.AddNeighbour(octo2);
                            octo2.AddNeighbour(octo1);
                        }
                }
        }
        [ExpectedResult(TestName = "Example", Result = "1656")]
        [ExpectedResult(TestName = "Input", Result = "1640")]
        public override string Part1(string testName)
        {
            int days = 100;

            for (int day = 0; day < days; day++)
            {
                foreach (var octo in OctoList)
                    octo.Counter++;
                if (!OctoList.Any(o => o.Counter < 10))
                    fullFlash = day;
                foreach (var octo in OctoList)
                    octo.OnClean();

                //Print($"after day {day}");
            }

            return flashes.ToString();
        }
        [ExpectedResult(TestName = "Example", Result = "195")]
        [ExpectedResult(TestName = "Input", Result = "312")]
        public override string Part2(string testName)
        {
            int day = 100;
            while (fullFlash < 0)
            {
                foreach (var octo in OctoList)
                    octo.Counter++;
                if (!OctoList.Any(o => o.Counter < 10))
                    fullFlash = day+1;
                foreach (var octo in OctoList)
                    octo.OnClean();
                day++;

                //Print($"after day {day}");
            }
            return fullFlash.ToString();
        }

        public void Print(string label)
        {
            Console.WriteLine(label);
            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 10; x++)
                    Console.Write(Octopusses[x, y].Counter);
                Console.WriteLine();
            }
        }

        public class Octopuss
        {
            public EventHandler Flash;
            private int _counter;
            public int Counter
            {
                get { return _counter; }
                set { 
                    _counter = value; 
                    if(_counter == 10)//flash
                    {
                        Flash.Invoke(this, null);
                        foreach (var octo in _neighbours)
                            octo.Counter++;
                    }
                }
            }

            private IList<Octopuss> _neighbours = new List<Octopuss>();

            public Octopuss(int counter)
            {
                _counter = counter;
            }

            public void AddNeighbour(Octopuss neighbour)
            {
                if (!_neighbours.Contains(neighbour))
                {
                    _neighbours.Add(neighbour);
                }
            }

            public void OnClean()
            {
                if (Counter > 9)
                    _counter = 0;
            }

            /*public override string ToString()
            {
                return base.ToString();
            }*/
        }
    }
}
