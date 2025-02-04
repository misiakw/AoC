using AdventOfCode.Interfaces;

namespace AdventOfCode.Day12
{
    public class Day12ShortTestInput: IInput
    {
        public string Input => @"<x=-1, y=0, z=2>
<x=2, y=-10, z=-7>
<x=4, y=-8, z=8>
<x=3, y=5, z=-1>";
    }
    public class Day12LongTestInput: IInput
    {
        public string Input => @"<x=-8, y=-10, z=0>
<x=5, y=5, z=10>
<x=2, y=-7, z=3>
<x=9, y=-8, z=-3>";
    }

    public class Day12AocInput: IInput
    {
        public string Input => @"<x=10, y=15, z=7>
<x=15, y=10, z=0>
<x=20, y=12, z=3>
<x=0, y=-3, z=13>";
    }
}