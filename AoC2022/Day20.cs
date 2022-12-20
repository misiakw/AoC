using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AoC.Base;

namespace AoC2022
{
    public class Day20 : DayBase
    {
        public Day20() : base(20)
        {
            Input("example1")
                .RunPart(1, 3)
            .Input("output");
        }

        public override object Part1(Input input)
        {
            var orig = input.Lines.Select(int.Parse).ToArray();

            var circle = new Circle<int>(orig);

            /*for (i=0; i<len; i++){
                var newPos = (i+orig[i])%len;
                if (newPos < 0)
                    newPos += len;
                Console.WriteLine($"pos {i} val {orig[i]} new Pos {newPos}");
            }*/

            throw new NotImplementedException();
        }

        public override object Part2(Input input)
        {
            throw new NotImplementedException();
        }
    }

    public class Circle<T>{
        protected int currentPos = 0;
        public int Len {get; protected set; } = 0;
        private CircleObj<T>? Elem;
        
        public Circle(T[] data){
            Elem = new CircleObj<T>(data[0]);
            Len = data.Length;
            var tmp = Elem;
            for(var i=1; i<Len; i++){
                tmp.Right = new CircleObj<T>(data[i]);
                tmp.Right.Left = tmp;
                tmp = tmp.Right;
            }
            Elem.Left = tmp;
            tmp.Right = Elem;
        }
        
        protected class CircleObj<V>{
            public readonly V Value;
            public CircleObj(V value){
                Value = value;
            }
            public CircleObj<V>? Left;
            public CircleObj<V>? Right;

            public override string ToString() => Value.ToString();
        }

        public override string ToString() => $"Circle({Elem})";
    }
    
}