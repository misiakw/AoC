using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AoC.Base;
using AoC2022.Days.Day25;

namespace AoC2022
{
    public class Day25 : LegacyDay<SafuNum>
    {
        public Day25() : base(25)
        {
            Input("sequence")
            //.RunPart(1, "2=-1=0")
            .Input("example1")
                .RunPart(1, "2=-1=0")
            .Input("output")
                .RunPart(1, "2-00=12=21-0=01--000");
        }

        public override SafuNum Parse(string val) => SafuNum.Parse(val);

        public override object Part1(IList<SafuNum> data, LegacyInput input)
        {
            var sum = SafuNum.Parse("0");
            foreach (var safu in data)
                sum = sum + safu;

            return sum.ToString();
        }

        public override object Part2(IList<SafuNum> data, LegacyInput input)
        {
            throw new NotImplementedException();
        }

        public override IList<string> Split(string val)
            => val.Split("\n", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                .ToList();
    }
}

namespace AoC2022.Days.Day25
{
    public class SafuNum
    {
        protected short[] _value;
        protected SafuNum(short[] val)
        {
            _value = val;
        }

        public static SafuNum Parse(string val)
        {
            var sum = new List<short>();
            for (var i = val.Length - 1; i >= 0; i--)
            {
                short tmp = 0;
                switch (val[i])
                {
                    case '=':
                        tmp = -2;
                        break;
                    case '-':
                        tmp = -1;
                        break;
                    default:
                        tmp = (short)(val[i] - '0');
                        break;
                }
                sum.Add(tmp);
            }
            sum.Reverse();
            return new SafuNum(sum.ToArray());
        }

        public override string ToString()
        {
            var result = string.Empty;
            foreach (var l in _value)
                result += l >= 0 ? l.ToString() : l == -1 ? '-' : '=';

            return result;
        }

        public static SafuNum operator +(SafuNum A, SafuNum B)
        {
            var output = new List<short>();
            short pass = 0;
            var lenA = A._value.Length-1;
            var lenB = B._value.Length-1;
            for(var i=0; ; i++){
                if(lenA == i && lenB == i)
                    break;
                if (lenA < i && lenB >= i){
                    //pass remains of B
                    for(; lenB >= i; i++){
                        var tmp = (short)(B._value[lenB-i] + pass);
                        pass = ProcessVal(ref tmp);
                        output.Add(tmp);
                    }
                    break;
                }else if(lenA >= i && lenB < i){
                    //pass remains of A                    
                    for(; lenA >= i; i++){
                        var tmp = (short)(A._value[lenA-i] + pass);
                        pass = ProcessVal(ref tmp);
                        output.Add(tmp);
                    }
                    break;
                } else{
                    var tmp = (short)(A._value[lenA-i] + B._value[lenB-i] + pass);
                    pass = ProcessVal(ref tmp);
                    output.Add(tmp);
                }
            }
            output.Reverse();
            return new SafuNum(output.ToArray());
        }
        private static short ProcessVal(ref short value){
            short pass = 0;
            if(value < -2){
                pass = -1;
                value += 5;
            }
            if(value > 2){
                pass = 1;
                value -= 5;
            }
            return pass;
        }
        public int ToInt(){
            int result = 0;
            int pow = 1;
            for(var i = _value.Length-1; i>=0; i--){
                result += _value[i]*pow;
                pow *= 5;
            }
            return result;
        }
    }
}