using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using AbstractDay;

namespace Day3
{
    public class Program: AbstractProgram<Sheet>
    {
        static void Main(string[] args)
        {
            new Program();
        }

        private Dictionary<string, IList<Sheet>> map = new Dictionary<string, IList<Sheet>>();
        private List<Sheet> sheets;

        public override string Task1(IList<Sheet> input)
        {
            sheets = input.ToList();
            foreach (var item in sheets)
            {
                for (var x = item.X; x < (item.X + item.Width); x++)
                {
                    for (var y = item.Y; y < (item.Y + item.Height); y++)
                    {
                        if (!map.ContainsKey($"{x}x{y}"))
                        {
                            map.Add($"{x}x{y}", new List<Sheet>());
                        }

                        if (!map[$"{x}x{y}"].Contains(item))
                        {
                            map[$"{x}x{y}"].Add(item);
                        }

                        if (map[$"{x}x{y}"].Count >= 2)
                        {
                            foreach (var sheet in map[$"{x}x{y}"])
                            {
                                sheet.DoesOverlap = true;
                            }
                        }
                    }
                }
            }

            var result = map.Count(v => v.Value.Count() >= 2);
            return result.ToString();
        }

        public override string Task2(IList<Sheet> input)
        {
            var sheet = sheets.First(v => !v.DoesOverlap);

            return sheet.Id.ToString();
        }

        protected override IList<Sheet> ParseInput(IList<string> input)
        {
            var regex = new Regex("#(\\d*) @ (\\d*).(\\d*): (\\d*).(\\d*)");
            return input.AsParallel()
                .Select(x =>
                {
                    var sheet =  new Sheet();
                    var matches = regex.Matches(x);
                    
                    sheet.Id = long.Parse(matches.First().Groups[1].Value);
                    sheet.X = long.Parse(matches.First().Groups[2].Value);
                    sheet.Y = long.Parse(matches.First().Groups[3].Value);
                    sheet.Width = long.Parse(matches.First().Groups[4].Value);
                    sheet.Height = long.Parse(matches.First().Groups[5].Value);
                    return sheet;
                })
                .ToList();
        }
    }

    public class Sheet
    {
        public long Id { get; set; }
        public long X { get; set; }
        public long Y { get; set; }
        
        public long Width { get; set; }
        public long Height { get; set; }

        public bool DoesOverlap = false;
    }
}
