using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoC2016.Common;
using Ip7 = AoC2016.Days.Day7.Ip7;

namespace AoC2016
{
    public class Day7 : Day<Ip7>
    {
        public Day7() : base(7)
        {
            Input("example1")
                .RunPart(1, 2)
            .Input("output")
                .RunPart(1, 115);
        }

        public override Ip7 Parse(string val) => new Ip7(val);

        public override object Part1(IList<Ip7> data, Input input)
        {
            var cappable = data.Where(i => i.AbbaCapable).ToList();
            return cappable.Count();
        }

        public override object Part2(IList<Ip7> data, Input input)
        {
            throw new NotImplementedException();
        }

        public override IList<string> Split(string val) => val.Split("\n").Select(s => s.Trim()).ToList();
    }
}
namespace AoC2016.Days.Day7{
    public class Ip7{
        public readonly IList<string> IpParts = new List<string>();
        public readonly IList<string> Hypernets = new List<string>();

        public bool AbbaCapable => Hypernets.Any(h => HasAbba(h))? false : IpParts.Any(ip => HasAbba(ip));

        public Ip7(string input){
            var split = input.Replace("[", "|[").Replace("]", "|").Split("|");
            IpParts = split.Where(s => s[0] != '[').ToList();
            Hypernets = split.Where(s => s[0] == '[').Select(s => s.Substring(1)).ToList();
        }

        private bool HasAbba(string val){
            for(var i=0; i<val.Length-3; i++){
                if (val[i] == val[i+3] && val[i+1] == val[i+2] && val[i] != val[i+1]){
                    return true;
                }
            }
            return false;
        }
    }
}
