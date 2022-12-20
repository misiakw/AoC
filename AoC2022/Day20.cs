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

            var circle = new Day20Circle(orig);

            Console.WriteLine(string.Join(", ", circle.ToList()));
            circle.Mix();
            Console.WriteLine(string.Join(", ", circle.ToList()));

            return 0;
        }

        public override object Part2(Input input)
        {
            throw new NotImplementedException();
        }
    }

    public class Circle<T>{
        protected int currentPos = 0;
        public int Len {get; protected set; } = 0;
        protected CircleObj<T>? Elem;
        
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

        public IList<T> ToList(){
            var result = new List<T>();
            for(var x=0; x<Len; x++)
                result.Add(this[x]);
            return result;
        }

        public T this[int x]{
            get {
                GoTo(x);
                return Elem.Value;
            }
        }

        protected CircleObj<T> GoTo(int x){
            var pos = x % Len;
            if (pos < 0) pos = Len + pos;

            if (currentPos < pos)
            {
                while (currentPos < pos)
                {
                    Elem = Elem.Right;
                    currentPos++;
                }
            }
            else if (currentPos > pos)
            {
                while (currentPos > pos)
                {
                    Elem = Elem.Left;
                    currentPos--;
                }
            }
            return Elem;
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

    public class Day20Circle : Circle<int>
    {
        private CircleObj<int>[] baseOrder;
        public Day20Circle(int[] data) : base(data)
        {
            baseOrder = new CircleObj<int>[data.Length];
            for(var i=0; i<data.Length; i++)
                baseOrder[i] = GoTo(i);
        }

        public void Mix(){

        }
    }

}