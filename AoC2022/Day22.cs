using AoC.Base;
using AoC.Common;

namespace AoC2022
{
    public class Day22 : DayBase
    {
        public Day22() : base(22)
        {
            Input("example1")
                .RunPart(1, 5)
            .Input("output");
        }

        public override object Part1(Input input)
        {
            var trip = new Journey(input);
            trip.Proceed();


            throw new NotImplementedException();
        }

        public override object Part2(Input input)
        {
            throw new NotImplementedException();
        }

        private class Journey{
            private Array2D<char> map = new Array2D<char>(' ');
            private IList<Step> steps = new List<Step>();
            private char Dir = 'R';
            private Point Location;

            public Journey(Input input){
                var y = 0;
                var lines  = input.Raw.Split("\n").ToArray();
                foreach (var line in lines.Take(input.Lines.Count()-2))
                {
                    var x = 0;
                    foreach (var ch in line.TrimEnd())
                    {
                        if (ch != ' ')
                            map[x, y] = ch;
                        x++;
                    }
                    y++;
                }

                var tmp = string.Empty;
                foreach (var ch in input.Lines.Last().Trim())
                {
                    if (ch == 'L' || ch == 'R')
                    {
                        steps.Add(new Step{
                            Distance = int.Parse(tmp),
                            Rotate = ch
                        });
                        tmp = string.Empty;
                    }else
                        tmp += ch;
                }
                var X = 0L;
                for(; X<lines.First().Length; X++)
                    if (map[X, 0] == '.')
                        break;
                map[X, 0] = 'S';
                Location = new Point(X, 0);
            }

            public void Proceed(){
                Console.WriteLine(map.Draw(c => c.ToString()));
                Console.WriteLine("===============================================");
                foreach (var step in steps){
                    var dif = GetDif(Dir);
                    Move(step.Distance, dif);
                    Dir = Rotate(step.Rotate);
                    Console.WriteLine(map.Draw(c => c.ToString()));
                    Console.WriteLine("===============================================");
                }

                Console.WriteLine(map.Draw(c => c.ToString()));
            }

            private Tuple<long, long> GetDif(char dir)
            {
                switch (dir)
                {
                    case 'R':
                        return Tuple.Create(1L, 0L);
                    case 'L':
                        return Tuple.Create(-1L, 0L);
                    case 'U':
                        return Tuple.Create(0L, -1L);
                    default:
                        return Tuple.Create(0L, 1L);
                }
            }
            private void Move(long dist, Tuple<long, long> dir){
                var ToMove = dist;
                var x = Location.X;
                var y = Location.Y;
                while(ToMove > 0){
                    var nX = x+dir.Item1;
                    var nY = x+dir.Item2;
                    if(map[nX, nY] == ' '){
                        var newPos = Wrap(x, y);
                        if (newPos == null){
                    map[nX, nY] = Dir;
                            Location = new Point(x, y);
                            return;
                        }
                        nX = newPos.Item1;
                        nY =newPos.Item2;
                    }else if(map[nX, nY] == '#'){
                    map[nX, nY] = Dir;
                        Location = new Point(x, y);
                        return;
                    }
                    x = nX;
                    y = nY;
                    map[nX, nY] = Dir;
                    ToMove--;
                }
                map[x, y] = 'O';
                Location = new Point(x, y);
            }
            private Tuple<long, long>? Wrap(long x, long y){
                var nx = x;
                var ny = y;
                if(Dir == 'U' || Dir == 'D'){
                    //wrap Y
                    var dif = Dir == 'U'? -1: 1;
                    while(map[x, y+dif] != ' ')
                        y+=dif;
                }else{
                    //wrap X
                    var dif = Dir == 'L'? -1: 1;
                    while(map[x+dif, y] != ' ')
                        x+=dif;
                }
                return map[nx, ny] == '#' ? null: Tuple.Create(nx, ny);
            }

            private char Rotate(char dir){
                switch(Dir){
                    case 'U':
                        return dir == 'C'? 'R' : 'L';
                    case 'R':
                        return dir == 'C'? 'D' : 'U';
                    case 'D':
                        return dir == 'C'? 'L' : 'R';
                    default:
                        return dir == 'C'? 'U' : 'D';

                }
            }
        }

        private struct Step{
            public int Distance;
            public char Rotate;
        }

        private Tuple<Array2D<char>, List<string>> ReadInput(Input input)
        {
            var map = new Array2D<char>(' ');

            

            var steps = new List<string>();
            
           

            return Tuple.Create(map, steps);
        }
    }
}
