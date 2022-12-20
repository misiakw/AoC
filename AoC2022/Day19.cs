using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using AoC.Base;

namespace AoC2022
{
    public class Day19 : DayBase
    {
        public Day19() : base(19)
        {
            Input("example1")
                .RunPart(1, 33)
            .Input("output");
        }

        public override object Part1(Input input)
        {
            var blueprints = ReadInput(input).ToArray();
            var output = blueprints[0].Optimize();
            //Console.WriteLine(output.Item2);
            return output;
        }

        public override object Part2(Input input)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<Blueprint> ReadInput(Input input){
            foreach(var line in input.Lines)
                yield return new Blueprint(line);
        }

        public enum Material{
            ore, clay, obsidian, geode
        }

        private class Blueprint{
            public readonly int Number;
            public IList<Robot> PriceList = new List<Robot>();
            private Robot geodeBot;
            private int[] MaxResource;
            public Blueprint(string recepie){
                var parts = recepie.Split(":", StringSplitOptions.RemoveEmptyEntries)
                    .Select(l => l.Trim()).ToArray();
                Number = int.Parse(parts[0].Replace("Blueprint ", ""));

                var regex = new Regex(@"Each ([a-z]*)? robot costs ((([0-9]*) ([a-z]*))+)");
                foreach(var single in parts[1].Trim().Split(".", StringSplitOptions.RemoveEmptyEntries)){
                    var match = regex.Match(single);
                    var costs = new Dictionary<Material, int>();
                    var materialCost = new int[3];
                    foreach(var cost in match.Groups[2].Value.Split(" and ").Select(c => c.Split( )))
                        materialCost[(int)getMaterial(cost[1])] = int.Parse(cost[0]);
                    PriceList.Add(new Robot(match.Groups[1].Value, materialCost));
                }

                foreach(var robot in PriceList.Where(r => r.type!= (int)Material.geode)){
                    var max = int.MinValue;
                    foreach(var other in PriceList.Where(r => r != robot)){
                        var price = other.PriceForType((Material)robot.type);
                        if (price > max) max = price;
                    }
                    robot.SetMax(max);
                }

                geodeBot = PriceList.First(r => r.type == (int)Material.geode);
                MaxResource = new int[4];
                for(var i=0; i<4; i++){
                    MaxResource[i] = int.MinValue;
                    foreach(var r in PriceList){
                        var price = r.PriceForType((Material)i);
                        if (price > MaxResource[i]) MaxResource[i] = price;
                    }
                }
            }

            public static Material getMaterial(string inp){
                if (inp == Material.ore.ToString())
                    return Material.ore;
                if (inp == Material.obsidian.ToString())
                    return Material.obsidian;
                if (inp == Material.geode.ToString())
                    return Material.geode;
                return Material.clay;
            }
            public int Optimize() => Optimize(new int[4]{1,0,0,0}, new int[4]{0,0,0,0}, 24);
            protected int Optimize(int[] robots, int[] resources, int time){
                if(time <= 0)
                    return resources[3];

                if(geodeBot.CanBuild(resources, 0, 0, 0)){
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
                results.Add(Optimize(robots, newRes, time-1));

                return results.Max();
            }
        }

        public class Robot{
            public readonly int type;
            public readonly int Ore;
            public readonly int Clay;
            public readonly int Obsidian;
            public int MaxToBuild { get; protected set;}
            public Robot(string material, int[] res){
                type = (int) Blueprint.getMaterial(material);
                Ore = res[0];
                Clay = res[1];
                Obsidian = res[2];
                if (type == (int)Material.geode)
                    MaxToBuild = int.MaxValue;
            } 

            public void SetMax(int max) => MaxToBuild = max;

            public bool CanBuild(int[] res, int howMuch, int time, int maxRes){
                if(howMuch == MaxToBuild) return false;
                if(howMuch*time + res[type] >= maxRes*time) return false;
                if(Ore > res[0]) return false;
                if(Clay > res[1]) return false;
                if(Obsidian> res[2]) return false;
                return true;
            }

            public int PriceForType(Material material){
                return material == Material.ore
                    ? Ore
                    : material == Material.clay
                    ? Clay
                    : Obsidian;
            }
        }
    }
}
