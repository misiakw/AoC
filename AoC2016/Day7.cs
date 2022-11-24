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
                .RunPart(1, 2);
            Input("example2")
                .RunPart(2, 3);
            Input("output")
                .RunPart(1, 115)
                .RunPart(2, 231);
        }

        public override Ip7 Parse(string val) => new Ip7(val);

        public override object Part1(IList<Ip7> data, Input input)
        {
            var cappable = data.Where(i => i.AbbaCapable).ToList();
            return cappable.Count();
        }

        public override object Part2(IList<Ip7> data, Input input)
        {
            var count = 0;
            foreach (var ip in data)
            {
                IList<SSL> hypnetsSSL = new List<SSL>();
                foreach (var sNet in ip.Hypernets)
                {
                    hypnetsSSL = GetSSL(sNet, hypnetsSSL);
                }

                IList<SSL> ipParts = new List<SSL>();
                foreach (var part in ip.IpParts)
                {
                    ipParts = GetSSL(part, ipParts);
                }

                if(hypnetsSSL.Any(bab => ipParts.Any(aba => aba.EqualsReverse(bab))))
                    count++;
            }

            return count;
        }

        public override IList<string> Split(string val) => val.Split("\n").Select(s => s.Trim()).ToList();

        private IList<SSL> GetSSL(string val, IList<SSL> result)
        {
            for (var i = 0; i < val.Length - 2; i++)
            {
                if (val[i] == val[i + 2] && val[i] != val[i + 1])
                {
                    var ssl = new SSL("" + val[i] + val[i + 1] + val[i + 2]);
                    if (!result.Contains(ssl))
                        result.Add(ssl);
                }
            }
            return result;
        }
        private class SSL : IEquatable<SSL>
        {
            public readonly char ch1;
            public readonly char ch2;
            public SSL(string val)
            {
                if (val.Length != 3)
                    throw new ArgumentException("ssl length not valid");
                if (val[0] != val[2] || val[0] == val[1])
                    throw new ArgumentException("ssl val invalid format, not XYX");
                ch1 = val[0];
                ch2 = val[1];
            }

            public bool Equals(SSL? other) => (ch1 == other?.ch1 && ch2 == other?.ch2);
            public bool EqualsReverse(SSL? other) => (ch1 == other?.ch2 && ch2 == other?.ch1);
            public override string ToString() => $"{ch1}{ch2}{ch1}";
        }
    }
}
namespace AoC2016.Days.Day7
{
    public class Ip7
    {
        public readonly IList<string> IpParts = new List<string>();
        public readonly IList<string> Hypernets = new List<string>();

        public bool AbbaCapable => Hypernets.Any(h => HasAbba(h)) ? false : IpParts.Any(ip => HasAbba(ip));

        public Ip7(string input)
        {
            var split = input.Replace("[", "|[").Replace("]", "|").Split("|");
            IpParts = split.Where(s => s[0] != '[').ToList();
            Hypernets = split.Where(s => s[0] == '[').Select(s => s.Substring(1)).ToList();
        }

        private bool HasAbba(string val)
        {
            for (var i = 0; i < val.Length - 3; i++)
            {
                if (val[i] == val[i + 3] && val[i + 1] == val[i + 2] && val[i] != val[i + 1])
                {
                    return true;
                }
            }
            return false;
        }
    }
}
