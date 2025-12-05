using AoC.Common.Abstractions;
using AoC.Common.Maps;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AoCBase2
{
    public static class InputExtensions
    {
        public async static IAsyncEnumerable<string> GetLinesAsync(this TestState test)
        {
            using (StreamReader sr = File.OpenText(test.testFile.PathToRelativeToSolution()))
                while (!sr.EndOfStream)
                    yield return await sr.ReadLineAsync();
        }
        public static IEnumerable<string> GetLines(this TestState test)
            => test.GetLinesAsync().ToEnumerable();
        public static IMap<char> GetMap(this TestState test, bool isInfinite = false)
        {
            var mapParams = new MapBuilderParams<char>() { IsInfinite = isInfinite };
            var lines = test.GetLines().ToArray();

            mapParams.Width = lines[0].Length;
            mapParams.Height = lines.Count();

            var map = MapBuilder<char>.GetEmpty(mapParams);
            for (var y = 0; y < mapParams.Height; y++)
                for (var x = 0; x < mapParams.Width; x++)
                    map[x, y] = lines[y][x];
            return map;
        }

        public async static Task<string> ReadLineAsync(this TestState test)
                => await test.GetLinesAsync().FirstAsync();
    }
}
