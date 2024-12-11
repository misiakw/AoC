using AoC.Common.Maps.StaticMap;
using AoCBase2;

namespace AoC2024.Days
{
    internal class Day6 : IDay
    {
        private enum Field
        {
            Empty = 0,
            Box = 1 << 0,
            //PreBump = 1 << 1,
            Up = 1 <<2,
            Right = 1 << 3,
            Down = 1 << 4,
            Left = 1 << 5,
        }
        /*private class Cell
        {
            public Cell(Field field)
            {
                Field = field;
            }
            //public Field Field;
        }*/

        public static void RunAoC() => AocRuntime.Day<Day6>(6, t => new Day6(t.GetLines().ToArray()))
        .Callback(1, (d, t) => d.Part1()).Skip()
        .Callback(2, (d, t) => d.Part2())
        .Test("example")
        .Test("input")
        //.Part(1).Correct(4752)
        .Part(2).Correct(1719)
        .Run();

        private readonly StaticMap<Field> map;
        private (int x, int y) guardPos;
        private IList<(long x, long y, Field dir)> part1Steps;

        public Day6(string[] lines)
        {                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              
            map = new StaticMap<Field>(lines[0].Length, lines.Length);

            for(var y=0; y<lines.Length; y++)
                for(var x=0; x < lines[y].Length; x++)
                    if(lines[y][x] == '^')
                    {
                        guardPos = (x, y);
                        map[x, y] =Field.Empty;
                    }
                    else if(lines[y][x] == '#')
                        map[x, y] = Field.Box;
                    else
                        map[x,y] = Field.Empty;
        }


        public string Part1()
        {
            part1Steps = ProceedToOutput(guardPos, (0, -1));
            return part1Steps.DistinctBy(step => $"{step.x}|{step.y}")
                .Count().ToString();
        }

        public string Part2()
        {
            if (part1Steps == null)
                Part1();
            var steps = part1Steps.ToArray();
            var position = 0;
            //var blocks = new IList<
            for (var i=0; i<steps.Length-1; i++)
            {
                if(steps[i+1].dir == steps[i].dir)
                {

                    var moved = ProceedToOutput((steps[i].x, steps[i].y), steps[i].dir switch { 
                        Field.Up => (1, 0), //right
                        Field.Right => (0, 1), //down
                        Field.Down => (-1, 0), //left
                        Field.Left => (0, -1) //up
                        }, (steps[i+1].x, steps[i+1].y));
                    if(moved == null)
                    {
                        position++;
                    }
                } //can place rock
            }
            return position.ToString();
        }

        private IList<(long x, long y, Field dir)> ProceedToOutput((long x, long y) startingPoint, (short moveX, short moveY) movementDir, (long x, long y)? rockPosition = null)
        {
            var steps = new List<(long, long, Field)>();
            var direction = Field.Up;
            movementDir = (0, -1);
            var nx = guardPos.x;
            var ny = guardPos.y;
            do
            {
                if (steps.Any(s => s.Item1 == nx && s.Item2 == ny && s.Item3 == direction)) //loop exist if next step was already done earlier
                    return null;

                if (map[nx, ny] == Field.Box || (rockPosition.HasValue && nx == rockPosition?.x && rockPosition?.y == ny)) //if hit box, revert and rotate
                {
                    nx -= movementDir.moveX;
                    ny -= movementDir.moveY;
                    movementDir = Rotate(movementDir);
                }
                direction = FromMovement(movementDir);
                steps.Add((nx, ny, direction));

                nx += movementDir.Item1;
                ny += movementDir.Item2;
                

            } while (isPointInRange(nx, ny));
            return steps;
        }
        private bool isPointInRange(long x, long y) 
            => (x >= 0 && x < map.Width && y >= 0 && y < map.Height);

        private Field FromMovement((short movX, short movY) movement) => movement switch
        {
            (0, -1) => Field.Up,
            (1, 0) => Field.Right,
            (0, 1) => Field.Down,
            (-1, 0) => Field.Left
        };

        private (short moveX, short moveY) Rotate((short moveX, short moveY) move) => move switch
        {
            (0, -1) => (1, 0), //up => right
            (1, 0) => (0, 1), //right -> down
            (0, 1) => (-1, 0), //down -> left
            (-1, 0) => (0, -1) //left => up
        };
    }
}
