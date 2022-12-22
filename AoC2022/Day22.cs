using AoC.Base;
using AoC.Common;
using ImageMagick;

namespace AoC2022
{
    public class Day22 : DayBase
    {
        public Day22() : base(22)
        {
            Input("example1")
            //.RunPart(1, 6032)
            .Input("output")
                .RunPart(1); //75290 Too High
        }

        public override object Part1(Input input)
        {
            var trip = new Journey(input);
            trip.Proceed();

            var rowPart = (trip.Location.Y + 1) * 1000;
            var colPart = (trip.Location.X + 1) * 4;
            var dirPart = trip.Dir == 'R' ? 0
                : trip.Dir == 'D' ? 1
                : trip.Dir == 'L' ? 2 : 3;
            return rowPart + colPart + dirPart;
        }

        public override object Part2(Input input)
        {
            throw new NotImplementedException();
        }

        private class Journey
        {
            private Array2D<char> map = new Array2D<char>(' ');
            private IList<Step> steps = new List<Step>();
            public char Dir { get; private set; } = 'R';
            public Point Location;
            private ImagePrinter printer;
            private string name;

            public Journey(Input input)
            {
                printer = new ImagePrinter(input.InputDir);
                name = input.Name;
                var y = 0;
                var lines = input.Raw.Split("\n").ToArray();
                foreach (var line in lines.Take(input.Lines.Count() - 2))
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
                        steps.Add(new Step
                        {
                            Distance = int.Parse(tmp),
                            Rotate = ch
                        });
                        tmp = string.Empty;
                    }
                    else
                        tmp += ch;
                }
                var X = 0L;
                for (; X < lines.First().Length; X++)
                    if (map[X, 0] == '.')
                        break;
                map[X, 0] = 'S';
                Location = new Point(X, 0);
            }

            private MagickImage initImg;

            public void Proceed()
            {
                var i = 1;
                foreach (var step in steps)
                {
                    var dif = GetDif(Dir);
                    Move(step.Distance, dif);
                    Dir = Rotate(step.Rotate);
                    Draw($"{i++}-{step.Distance}{step.Rotate}-{Dir}");
                }

                Draw($"end");
            }

            private void Draw(string name)
            {
                printer.DrawImage((int)map.Width * 6, (int)map.Height * 6, $"{this.name}-{name}",
                img =>
                {
                    img.Settings.StrokeColor = MagickColors.Red;
                    img.Settings.StrokeWidth = 1;
                    img.Settings.FillColor = MagickColors.White;

                    var border = new DrawableBorderColor(MagickColors.Black);
                    var drawables = new List<IDrawable>();
                    for (var y = 0; y <= map.Height; y++)
                    {
                        for (var x = 0; x <= map.Width; x++)
                        {
                            var ch = map[x, y];
                            if (map[x, y] == ' ') continue;
                            var rect = new DrawableRectangle(x * 6, y * 6, x * 6 + 5, y * 6 + 5);
                            var fill = ch == '#'
                                ? MagickColors.Gray
                                : MagickColors.LightGreen;
                            if (x == Location.X && y == Location.Y)
                                fill = MagickColors.Blue;
                            drawables.AddRange(new IDrawable[3]
                            { new DrawableStrokeColor(fill), new DrawableFillColor(fill), rect});

                            switch (ch)
                            {
                                case 'R':
                                    drawables.AddRange(new IDrawable[4]
                                        { new DrawableStrokeColor(MagickColors.Red),
                                         new DrawableFillColor(MagickColors.Red),
                                        new DrawableLine(x*6, y*6, x*6+5, y*6+3),
                                        new DrawableLine(x*6+5, y*6+3, x*6, y*6+5) }
                                    );
                                    break;
                                case 'L':
                                    drawables.AddRange(new IDrawable[4]
                                        { new DrawableStrokeColor(MagickColors.Red),
                                         new DrawableFillColor(MagickColors.Red),
                                        new DrawableLine(x*6+5, y*6, x*6, y*6+3),
                                        new DrawableLine(x*6, y*6+3, x*6+5, y*6+5) }
                                    );
                                    break;
                                case 'D':
                                    drawables.AddRange(new IDrawable[4]
                                        { new DrawableStrokeColor(MagickColors.Red),
                                         new DrawableFillColor(MagickColors.Red),
                                        new DrawableLine(x*6, y*6, x*6+3, y*6+5),
                                        new DrawableLine(x*6+3, y*6+5, x*6+5, y*6) }
                                    );
                                    break;
                                case 'U':
                                    drawables.AddRange(new IDrawable[4]
                                        { new DrawableStrokeColor(MagickColors.Red),
                                         new DrawableFillColor(MagickColors.Red),
                                        new DrawableLine(x*6, y*6+5, x*6+3, y*6),
                                        new DrawableLine(x*6+3, y*6, x*6+5, y*6+5) }
                                    );
                                    break;
                            }
                        }
                    }
                    img.Draw(drawables);
                });
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
            private void Move(long dist, Tuple<long, long> dir)
            {
                var ToMove = dist;
                var x = Location.X;
                var y = Location.Y;
                while (ToMove > 0)
                {
                    var nX = x + dir.Item1;
                    var nY = y + dir.Item2;
                    if (map[nX, nY] == ' ')
                    {
                        var newPos = Wrap(x, y);
                        if (newPos == null)
                        {
                            Location = new Point(x, y);
                            return;
                        }
                        nX = newPos.Item1;
                        nY = newPos.Item2;
                    }
                    else if (map[nX, nY] == '#')
                    {
                        map[x, y] = Dir;
                        Location = new Point(x, y);
                        return;
                    }
                    x = nX;
                    y = nY;
                    map[nX, nY] = Dir;
                    ToMove--;
                }
                Location = new Point(x, y);
            }
            private Tuple<long, long>? Wrap(long x, long y)
            {
                var nx = x;
                var ny = y;
                if (Dir == 'U' || Dir == 'D')
                {
                    //wrap Y
                    var dif = Dir == 'U' ? 1 : -1;
                    while (map[x, y + dif] != ' ')
                        y += dif;
                    ny = y;
                }
                else
                {
                    //wrap X
                    var dif = Dir == 'L' ? 1 : -1;
                    while (map[x + dif, y] != ' ')
                        x += dif;
                    nx = x;
                }
                return map[nx, ny] == '#' ? null : Tuple.Create(nx, ny);
            }

            private char Rotate(char dir)
            {
                switch (this.Dir)
                {
                    case 'U':
                        return dir == 'R' ? 'R' : 'L';
                    case 'R':
                        return dir == 'R' ? 'D' : 'U';
                    case 'D':
                        return dir == 'R' ? 'L' : 'R';
                    default:
                        return dir == 'R' ? 'U' : 'D';

                }
            }
        }

        private struct Step
        {
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
