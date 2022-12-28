using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AoC.Base;
using AoC.Common;

namespace AoC2022
{
    public class Day23 : DayBase
    {
        public Day23() : base(23)
        {
            Input("small")
                //.RunPart(1)
            .Input("example1")
                .RunPart(1, 110)
                .RunPart(2, 20)
            .Input("output")
                .RunPart(1, 4241)
                .RunPart(2, 1079);
        }

        public override object Part1(Input input)
        {
            var elves = GetElves(input);
            var moves = new Queue<char>(new char[4]{ 'N', 'S', 'W', 'E' });

            int i=0;
            while(true){
                //determine if any move
                var toMove = elves.Where(e => e.ShouldMove).ToList();
                if(i++ == 10 || !toMove.Any())
                    break;

                //locate potential position
                var order = moves.ToArray();
                var moveMap = new Dictionary<string, IList<Elf>>();

                foreach(var elf in toMove){
                    var dest = elf.ConsiderMove(order);
                    if(string.IsNullOrEmpty(dest))
                        continue;
                    if(!moveMap.ContainsKey(dest))
                        moveMap.Add(dest, new List<Elf>());
                    moveMap[dest].Add(elf);
                }

                //move
                foreach(var kv in moveMap){
                    if(kv.Value.Count() > 1)
                        continue;
                    kv.Value.First().MoveTo(kv.Key);
                }

                //reorder destination
                moves.Enqueue(moves.Dequeue());
            }

            return elves.First().Print().Where(c => c == '.').Count();
        }

        public override object Part2(Input input)
        {
            var elves = GetElves(input);
            var moves = new Queue<char>(new char[4]{ 'N', 'S', 'W', 'E' });

            int i=0;
            while(true){
                //determine if any move
                var toMove = elves.Where(e => e.ShouldMove).ToList();
                if(!toMove.Any())
                    break;

                //locate potential position
                var order = moves.ToArray();
                var moveMap = new Dictionary<string, IList<Elf>>();

                foreach(var elf in toMove){
                    var dest = elf.ConsiderMove(order);
                    if(string.IsNullOrEmpty(dest))
                        continue;
                    if(!moveMap.ContainsKey(dest))
                        moveMap.Add(dest, new List<Elf>());
                    moveMap[dest].Add(elf);
                }

                //move
                foreach(var kv in moveMap){
                    if(kv.Value.Count() > 1)
                        continue;
                    kv.Value.First().MoveTo(kv.Key);
                }

                //reorder destination
                moves.Enqueue(moves.Dequeue());
                i++;
            }

            return i+1;
        }

        private IList<Elf> GetElves(Input input){
            var map = new Array2D<Elf?>(null);
            var elves = new List<Elf>();

            var y = 0;
            foreach(var line in input.Lines){
                int x = 0;
                foreach(var ch in line){
                    if(ch == '#')
                        elves.Add(new Elf(x, y, map));
                    x++;
                }
                y++;
            }

            return elves;
        }

        private class Elf{
            private Array2D<Elf?> map;
            private long x, y;
            public Elf(long X, long Y, Array2D<Elf?> Map){
                this.x = X;
                this.y = Y;
                this.map = Map;
                Map[X, Y] = this;
            }

            public string ConsiderMove(char[] dest){
                foreach(var dir in dest){
                    switch(dir){
                        case 'N':
                            if(MoveN) return $"{x}|{y-1}";
                        break;
                        case 'S':
                            if(MoveS) return $"{x}|{y+1}";
                        break;
                        case 'E':
                            if(MoveE) return $"{x+1}|{y}";
                        break;
                        case 'W':
                            if(MoveW) return $"{x-1}|{y}";
                        break;
                    }
                }
                return string.Empty;
            }

            public void MoveTo(string position){
                var pos = position.Split("|").Select(p => long.Parse(p)).ToArray();
                map.Remove(x, y);
                x = pos[0];
                y = pos[1];
                map[x, y] = this;
            }

            public string Print() => map.Draw(e => e == null ? ".": "#");

            public override string ToString() => $"Elf({x}|{y})";

            public bool ShouldMove => Around.Any(e => e != null);
            private bool MoveN => Top.All(e => e == null);
            private bool MoveS => Bottom.All(e => e == null);
            private bool MoveE => Right.All(e => e == null);
            private bool MoveW => Left.All(e => e == null);

            private Elf?[] Around => new Elf?[8]{ NE, N, NW, E, W, SE, S, SW };
            private Elf?[] Top => new Elf?[3]{ NE, N, NW };
            private Elf?[] Bottom => new Elf?[3]{ SE, S, SW };
            private Elf?[] Left => new Elf?[3]{ NW, W, SW };
            private Elf?[] Right => new Elf?[3]{ NE, E, SE };

            private Elf? NE => map[x+1, y-1];
            private Elf? N => map[x, y-1];
            private Elf? NW => map[x-1, y-1];
            private Elf? E => map[x+1, y];
            private Elf? W => map[x-1, y];
            private Elf? SE => map[x+1, y+1];
            private Elf? S => map[x, y+1];
            private Elf? SW => map[x-1, y+1];
        }
    }
}
