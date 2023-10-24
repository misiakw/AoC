using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AoC.Base;
using SignalPacket = AoC2022.Days.Day13.SignalPacket;

namespace AoC2022
{
    public class Day13 : LegacyDay<SignalPacket[]>
    {
        public Day13() : base(13)
        {
            Input("example1")
                .RunPart(1, 13)
                .RunPart(2, 140)
            .Input("output")
                .RunPart(1, 5185)
                .RunPart(2, 23751);
        }

        public override SignalPacket[] Parse(string val) =>
            val.Split("\n").Select(s => new SignalPacket(s.Substring(1, s.Length - 2))).ToArray();

        public override object Part1(IList<SignalPacket[]> data, LegacyInput input)
        {
            var result = 0;
            for(var i=0; i<data.Count(); i++)
                if (data[i][0].CompareTo(data[i][1]) < 0)
                    result += i+1;
            
            return result;
        }

        public override object Part2(IList<SignalPacket[]> data, LegacyInput input)
        {
            var div1 = new SignalPacket("[[2]]");
            var div2 = new SignalPacket("[[6]]");

            var packets = data.SelectMany(d => d).ToList();
            packets.Add(div1);
            packets.Add(div2);
            packets = packets.OrderBy(p => p).ToList();

            return (packets.IndexOf(div1)+1)*(packets.IndexOf(div2)+1);
        }

        public override IList<string> Split(string val) =>
            val.Split("\n\n").ToList();
    }
}
namespace AoC2022.Days.Day13{
    public class SignalPacket : IComparable<SignalPacket>
    {
        private IList<SignalPacket> packets = new List<SignalPacket>();
        private int value;
        public SignalPacket(int input){
            value = input;
        }
        public SignalPacket(string input){
            packets = new List<SignalPacket>();
            
            for(var ctr=0; ctr < input.Length;)
                if(input[ctr] == '['){
                    var list = ReadPacketList(input, ctr).ToArray();
                    ctr += list.Length+3;
                    packets.Add(new SignalPacket(new string(list)));
                }else{
                    var number = ReadNumber(input, ctr).ToArray();
                    ctr += number.Length+1;
                    packets.Add(new SignalPacket(int.Parse(number)));
                }
        }
        public int CompareTo(SignalPacket? other)
        {
            if (packets == null && other?.packets == null) // string comparation
                return value.CompareTo(other?.value);

            var leftList = packets ?? new List<SignalPacket>(){new SignalPacket(value)};
            var rightList = other?.packets ?? new List<SignalPacket>(){new SignalPacket(other?.value ?? 0)};

            for(var i=0; i<leftList.Count(); i++){
                if(rightList.Count() <= i) //right list ended earlier
                    return 1;
                var comp = leftList[i].CompareTo(rightList[i]);
                if (comp != 0)
                    return comp;
            }
            return leftList.Count() < rightList.Count() ? -1 : 0;
        }
        private IEnumerable<char> ReadPacketList(string input, int start){
            var lvl = 0;
            for (var i = start+1; i < input.Length; i++){
                if(input[i] == '[') lvl++;
                if(input[i] == ']'){
                    if(lvl == 0) break;
                    lvl--;
                }
                yield return input[i];
            }
        }
        private IEnumerable<char> ReadNumber(string input, int start){
            while(start < input.Length && input[start] != ',')
                yield return input[start++];
        }
        public override string ToString() => packets != null 
            ? "["+string.Join(",", packets.Select(p => p.ToString()))+"]"
            : value.ToString();
    }
}