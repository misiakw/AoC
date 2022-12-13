using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AoC.Base;
using SignalPacket = AoC2022.Days.Day13.SignalPacket;

namespace AoC2022
{
    public class Day13 : Day<SignalPacket[]>
    {
        public Day13() : base(13)
        {
            Input("example1")
                .RunPart(1, 13)
                .RunPart(2, 140)
            .Input("output")
                .RunPart(1, 5185)
                .RunPart(2);
        }

        public override SignalPacket[] Parse(string val) =>
            val.Split("\n").Select(s => new SignalPacket(s.Substring(1, s.Length - 2))).ToArray();

        public override object Part1(IList<SignalPacket[]> data, Input input)
        {
            var pairs = new List<int>();
            var pairNum = 1;
            foreach (var set in data){
                var left = set[0];
                var right = set[1];
                var compare = left.CompareTo(right);
                if (compare < 0)
                   pairs.Add(pairNum);
                pairNum++;
            }
            
            return pairs.Sum();;
        }

        public override object Part2(IList<SignalPacket[]> data, Input input)
        {
            var div1 = new SignalPacket("[[2]]");
            var div2 = new SignalPacket("[[6]]");

            var packets = new List<SignalPacket>(){div1, div2};
            foreach (var pair in data){
                packets.Add(pair[0]);
                packets.Add(pair[1]);
            }
            packets = packets.Order().ToList();

            return (packets.IndexOf(div1)+1)*(packets.IndexOf(div2)+1);
        }

        public override IList<string> Split(string val) =>
            val.Split("\n\n").ToList();
    }
}
namespace AoC2022.Days.Day13{
    public class SignalPacket : IComparable<SignalPacket>
    {
        private IList<SignalPacket> packets;
        private int value;
        public SignalPacket(int input){
            value = input;
        }
        public SignalPacket(string input){
            packets= new List<SignalPacket>();
            var ctr = 0;
            while(ctr < input.Length){
                if(input[ctr] == '['){
                    var list = ReadPacketList(input, ctr);
                    ctr += list.Length+3;
                    packets.Add(new SignalPacket(list));
                }else{
                    var number = ReadNumber(input, ctr);
                    ctr += number.Length+1;
                    packets.Add(new SignalPacket(int.Parse(number)));
                }
            }
        }
        public int CompareTo(SignalPacket? other)
        {
            if (packets == null && other.packets == null){ // string comparation
                var result = value.CompareTo(other.value);
                return result;
            }

            var leftList = packets ?? new List<SignalPacket>(){new SignalPacket(value)};
            var rightList = other.packets ?? new List<SignalPacket>(){new SignalPacket(other.value)};

            for(var i=0; i<leftList.Count(); i++){
                if(rightList.Count() <= i) //right list ended earlier
                    return 1;
                var comp = leftList[i].CompareTo(rightList[i]);
                if (comp != 0)
                    return comp;
            }
            return leftList.Count() < rightList.Count() ? -1 : 0;
        }
        private string ReadPacketList(string input, int start){
            var lvl = 0;
            var result = string.Empty;
            while(start+1 < input.Length){
                start++;
                if(input[start] == '[') lvl++;
                if(input[start] == ']'){
                    if(lvl == 0) break;
                    lvl--;
                }
                result += input[start];
            }
            return result;
        }
        private string ReadNumber(string input, int start){
            var result = string.Empty;
            while(start < input.Length && input[start] != ',')
                result += input[start++];
            return result;
        }
        public override string ToString()
        {
            return packets != null 
                ? "["+string.Join(",", packets.Select(p => p.ToString()))+"]"
                : value.ToString();
        }
    }
}