using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using AoC.Base;
using AoC.Common;

namespace AoC2022
{
    public class Day19 : LegacyDayBase
    {
        public Day19() : base(19)
        {
            Input("example1")
                .RunPart(1, 33)
                .RunPart(2, 56 * 62)
            .Input("output")
                .RunPart(1, 1389)
                .RunPart(2, 3003);
        }

        public override object Part1(LegacyInput input)
        {
            var blueprints = ReadInput(input).ToArray();
            input.Cache = blueprints;
            var result = new List<int>();
            Parallel.ForEach(blueprints, b =>
            {
                result.Add(b.Optimize() * b.Number);
            });

            return result.Sum();
        }

        public override object Part2(LegacyInput input)
        {
            var blueprints = (Blueprint[])(input.Cache ?? new Blueprint[0]);
            var result = new List<int>();
            Parallel.ForEach(blueprints.Take(3).ToArray(), b =>
            {
                var a = b.Optimize2();
                Console.WriteLine($"{b.Number} : {a}");
                result.Add(a);
            });

            var mul = 1;
            foreach (var num in result)
                mul *= num;
            return mul;
        }

        private IEnumerable<Blueprint> ReadInput(LegacyInput input)
        {
            foreach (var line in input.Lines)
                yield return new Blueprint(line);
        }

        public enum Material
        {
            ore, clay, obsidian, geode
        }

        private class Blueprint
        {
            public readonly int Number;
            public IList<IRobot> PriceList = new List<IRobot>();
            private IRobot geodeBot;
            private int[] MaxResource;
            private int currentMax = 0;
            public Blueprint(string recepie)
            {
                var parts = recepie.Split(":", StringSplitOptions.RemoveEmptyEntries)
                    .Select(l => l.Trim()).ToArray();
                Number = int.Parse(parts[0].Replace("Blueprint ", ""));

                var regex = new Regex(@"Each ([a-z]*)? robot costs ((([0-9]*) ([a-z]*))+)");
                foreach (var single in parts[1].Trim().Split(".", StringSplitOptions.RemoveEmptyEntries))
                {
                    var match = regex.Match(single);
                    var costs = new Dictionary<Material, int>();
                    var materialCost = new int[3];
                    foreach (var cost in match.Groups[2].Value.Split(" and ").Select(c => c.Split()))
                        materialCost[(int)getMaterial(cost[1])] = int.Parse(cost[0]);
                    if (match.Groups[1].Value == "geode")
                    {
                        geodeBot = new GeodeRobot(match.Groups[1].Value, materialCost);
                        PriceList.Add(geodeBot);
                    }
                    else if (match.Groups[1].Value == "obsidian")
                    {
                        PriceList.Add(new ObsidianRobot(match.Groups[1].Value, materialCost));
                    }
                    else
                    {
                        PriceList.Add(new Robot(match.Groups[1].Value, materialCost));
                    }

                }

                foreach (var robot in PriceList.Where(r => r.type != (int)Material.geode))
                {
                    var max = int.MinValue;
                    foreach (var other in PriceList.Where(r => r != robot))
                    {
                        var price = other.PriceForType((Material)robot.type);
                        if (price > max) max = price;
                    }
                    robot.SetMax(max);
                }

                MaxResource = new int[4];
                for (var i = 0; i < 4; i++)
                {
                    MaxResource[i] = int.MinValue;
                    foreach (var r in PriceList)
                    {
                        var price = r.PriceForType((Material)i);
                        if (price > MaxResource[i]) MaxResource[i] = price;
                    }
                }
                if (geodeBot != null)
                    PriceList.Remove(geodeBot);
                else
                    geodeBot = PriceList.Last();
            }

            private static Array2D<int> maxOx = new Array2D<int>(-1);
            private int MaxObsidian(int current, int robots, int timeLeft)
            {
                var oxVal = maxOx[robots, timeLeft];
                if (oxVal >= 0)
                    return oxVal + current;
                var start = 0;
                while (timeLeft-- > 0)
                {
                    start += robots;
                    robots++;
                }
                maxOx[robots, timeLeft] = start;
                return current + start;
            }

            public static Material getMaterial(string inp)
            {
                if (inp == Material.ore.ToString())
                    return Material.ore;
                if (inp == Material.obsidian.ToString())
                    return Material.obsidian;
                if (inp == Material.geode.ToString())
                    return Material.geode;
                return Material.clay;
            }
            public int Optimize() => Optimize(new int[4] { 1, 0, 0, 0 }, new int[4] { 0, 0, 0, 0 }, 24);
            public int Optimize2() => Optimize(new int[4] { 1, 0, 0, 0 }, new int[4] { 0, 0, 0, 0 }, 32);
            protected int Optimize(int[] robots, int[] resources, int time)
            {
                if (time <= 0)
                {
                    if (resources[3] > currentMax) currentMax = resources[3];
                    return resources[3];
                }

                if (MaxObsidian(resources[3], robots[3], time) < currentMax)
                    return 0;

                if (geodeBot.CanBuild(resources, 0, 0, 0))
                {
                    var newRobots = new int[4]{
                        robots[0], robots[1], robots[2], robots[3]+1
                    };
                    var geodeRes = new int[4]{
                        resources[0]-geodeBot.Ore+robots[0], resources[1]-geodeBot.Clay+robots[1],
                        resources[2]-geodeBot.Obsidian+robots[2], resources[3]+robots[3]
                    };
                    return Optimize(newRobots, geodeRes, time - 1);
                }

                var results = new List<int>();
                int[] newRes;

                var newAvailables = PriceList.Where(r => r.CanBuild(resources, robots[r.type], time, MaxResource[r.type])).ToArray();

                foreach (var robot in newAvailables)
                {
                    var newRobots = new int[4]{
                            robots[0], robots[1], robots[2], robots[3]
                        };
                    newRobots[robot.type]++;

                    newRes = new int[4]{
                            resources[0]-robot.Ore+robots[0], resources[1]-robot.Clay+robots[1],
                            resources[2]-robot.Obsidian+robots[2], resources[3]+robots[3]
                        };

                    results.Add(Optimize(newRobots, newRes, time - 1));
                }

                newRes = new int[4]{
                    resources[0]+robots[0], resources[1]+robots[1],
                    resources[2]+robots[2], resources[3]+robots[3]
                };
                results.Add(Optimize(robots, newRes, time - 1));

                return results.Max();
            }
        }

        public interface IRobot
        {
            public void SetMax(int max);
            public bool CanBuild(int[] res, int howMuch, int time, int maxRes);
            public int PriceForType(Material material);
            public int MaxToBuild { get; }
            public int type { get; }
            public int Ore { get; }
            public int Clay { get; }
            public int Obsidian { get; }
        }

        public abstract class AbstractRobot : IRobot
        {
            public int type { get; protected set; }
            public int Ore { get; protected set; }
            public int Clay { get; protected set; }
            public int Obsidian { get; protected set; }
            public int MaxToBuild { get; protected set; } = int.MaxValue;
            public AbstractRobot(string material, int[] res)
            {
                type = (int)Blueprint.getMaterial(material);
                Ore = res[0];
                Clay = res[1];
                Obsidian = res[2];
            }

            public void SetMax(int max) => MaxToBuild = max;

            public abstract bool CanBuild(int[] res, int howMuch, int time, int maxRes);

            public int PriceForType(Material material)
            {
                return material == Material.ore
                    ? Ore
                    : material == Material.clay
                    ? Clay
                    : Obsidian;
            }
        }

        public class Robot : AbstractRobot
        {
            public Robot(string material, int[] res) : base(material, res) { }

            public override bool CanBuild(int[] res, int howMuch, int time, int maxRes)
            {
                if (howMuch == MaxToBuild) return false;
                if (howMuch * time + res[type] >= maxRes * time) return false;
                if (Ore > res[0]) return false;
                return true;
            }
        }
        public class ObsidianRobot : AbstractRobot
        {
            public ObsidianRobot(string material, int[] res) : base(material, res) { }

            public override bool CanBuild(int[] res, int howMuch, int time, int maxRes)
            {
                if (howMuch == MaxToBuild) return false;
                if (howMuch * time + res[type] >= maxRes * time) return false;
                if (Ore > res[0]) return false;
                if (Clay > res[1]) return false;
                return true;
            }
        }
        public class GeodeRobot : AbstractRobot
        {
            public GeodeRobot(string material, int[] res) : base(material, res) { }

            public override bool CanBuild(int[] res, int howMuch, int time, int maxRes)
            {
                if (Ore > res[0]) return false;
                if (Obsidian > res[2]) return false;
                return true;
            }
        }
    }
}
