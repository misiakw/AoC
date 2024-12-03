using AoCBase2;

namespace AoC2024.Days{
    public class Day2 : IDay
    {
        public static void RunAoC() => AocRuntime.Day<Day2>(2)
                .Callback(1, (d, t) => d.Part1(t.GetLinesAsync()))
                .Callback(2, (d, t) => d.Part2(t.GetLinesAsync()))
                .Test("example", "Inputs/Day2/example1.txt")
//                    .Part(1).Correct(2)
//                    .Part(2).Correct(6)
                .Test("input", "Inputs/Day2/output.txt")
//                    .Part(1).Correct(483)
//                    .Part(2).Correct(528)
                .Run();

        public async Task<string> Part1(IAsyncEnumerable<string> lines)
            => (await lines.Select(l => ValidateReport(l.Trim().Split(" ").Select(int.Parse).ToArray()))
                .Where(v => v).CountAsync()).ToString();
        public async Task<string> Part2(IAsyncEnumerable<string> lines)
            => (await lines.Select(l => GetInvalidLevelCount(l.Trim().Split(" ").Select(int.Parse).ToArray()) <= 1)
                .Where(v => v).CountAsync()).ToString();

        private bool ValidateReport(int[] report){
            var order = report[0].CompareTo(report[1]);
            for(var i=1; i<report.Count(); i++){
                if(report[i-1].CompareTo(report[i]) != order || Math.Abs(report[i-1]-report[i]) > 3)
                    return false;
            }
            return true; 
        }

        private int GetInvalidLevelCount(int[] report){
            var invalid = 0;
            var order = report[0].CompareTo(report[1]);
            for(var i=1; i<report.Count(); i++){
                if(report[i-1].CompareTo(report[i]) != order || Math.Abs(report[i-1]-report[i]) > 3)
                    invalid++;
            }
            return invalid;
        }
    }
}