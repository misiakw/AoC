using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoC2016.Common;
using EncName = AoC2016.Days.Day4.EncName;

namespace AoC2016
{
    public class Day4 : Day<EncName>
    {
        public Day4() : base(4, true)
        {
            Input("example")
                //.RunPart(1, 1514)
            .Input("example2")
                //.RunPart(2)
            .Input("output")
                .RunPart(1, 158835);
                .RunPart(2, 993);
        }

        public override EncName Parse(string val) => new EncName(val);

        public override object Part1(IList<EncName> data, Input input) => data.Where(d => d.IsReal).Sum(d => d.SectorId);

        public override object Part2(IList<EncName> data, Input input)
        {
            var alphabet = "abcdefghijklmnopqrstuvwxyz";
            var searchedId = 0;
            foreach(var room in data.Where(d => d.IsReal)){
                var move = room.SectorId%alphabet.Length;

                var newName = string.Empty;
                foreach(var ch in room.Name)
                    if(ch == '-')
                        newName += " ";
                    else
                        newName += alphabet[(ch-'a'+move)%alphabet.Length];
                        
                if (newName == "northpole object storage")
                    searchedId = room.SectorId;
                Console.WriteLine(newName);
            }
            return searchedId;
        }

        public override IList<string> Split(string val) => val.Split("\n").Select(s => s.Trim()).ToList();
    }
}
namespace AoC2016.Days.Day4{
    public class EncName{
        public readonly string Name;
        public readonly int SectorId;
        private readonly string checksum;
        private bool? isReal;

        public EncName(string val){
            checksum = val.Substring(val.Length-6, 5);
            var chunks = val.Split("-").ToList();
            SectorId = int.Parse(chunks.Last().Substring(0, chunks.Last().Count() - 7));
            chunks.RemoveAt(chunks.Count - 1);
            Name = string.Join("-", chunks);
        }
        public bool IsReal{
            get {
                if (!isReal.HasValue)
                    isReal = string.Equals(checksum, Hash.Substring(0, 5));
                return isReal.Value;
            }
        }

        private string Hash {
            get {
                var dict = new Dictionary<char, int>();
                foreach(var ch in Name.Replace("-", "")){
                    if(dict.ContainsKey(ch))
                        dict[ch]++;
                    else
                        dict.Add(ch, 1);
                }
                var list = dict.ToList();
                list.Sort((a, b) => {
                    if (a.Value != b.Value)
                        return b.Value.CompareTo(a.Value);
                    return a.Key.CompareTo(b.Key);
                });
                return string.Join("", list.Select(l => l.Key));
            }
        }
    }
}
