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
                .RunPart(1, 3L)
                .RunPart(2, 1623178306L)
            .Input("myExamle")
                //.RunPart(1)
            .Input("output")
                .RunPart(1, 2203L)
                .RunPart(2, 6641234038999L);
        }

        public override object Part1(Input input)
        {
            var circle = new Day20Circle(input.Lines.Select(long.Parse).ToArray());
            circle.Mix();
            circle.Zero();

            var a = circle[1000];
            var b=  circle[2000];
            var c = circle[3000];

            return a+b+c;
        }

        public override object Part2(Input input)
        {
            var orig = input.Lines.Select(long.Parse).ToArray();
            for(var i=0; i< orig.Length; i++)
                orig[i] *= 811589153;

            var circle = new Day20Circle(orig);
            for(int i=0; i<10; i++)
                circle.Mix();
            circle.Zero();

            var a = circle[1000];
            var b=  circle[2000];
            var c = circle[3000];

            return a+b+c;
        }
    }

    public class Circle<T>{
        public int Len {get; protected set; } = 0;
        protected CircleObj<T> Start;
        
        public Circle(T[] data){
            Start = new CircleObj<T>(data[0]);
            Len = data.Length;
            var tmp = Start;
            for(var i=1; i<Len; i++){
                tmp.Right = new CircleObj<T>(data[i]);
                tmp.Right.Left = tmp;
                tmp = tmp.Right;
            }
            Start.Left = tmp;
            tmp.Right = Start;
        }

        public IList<T> ToList(){
            var result = new List<T>();
            var el = Start;
            for(var x=0; x<Len; x++){
                result.Add(el.Value);
                el = el.Right;
            }
            return result;
        }

        public T this[long x]{
            get => Get(x).Value;
        }

        protected CircleObj<T> Get(long x){
            var pos = x % Len;
                if (pos < 0) pos = Len + pos;
                var el = Start;
                var hops = 0;
                if(pos > Len/2){
                    el = Start.Left;
                    hops++;
                    for(var i=Len-1; i>pos; i--){
                        el = el.Left;
                        hops ++;
                    }
                }else{
                    for(var i=0; i<pos; i++){
                        el = el.Right;
                        hops ++;
                    }
                }
                return el;
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

        public override string ToString() => $"Circle({Start})";
    }

    public class Day20Circle : Circle<long>
    {
        private CircleObj<long>[] baseOrder;
        public Day20Circle(long[] data) : base(data)
        {
            baseOrder = new CircleObj<long>[data.Length];
            for(var i=0; i<data.Length; i++)
                baseOrder[i] = Get(i);
        }

        public void Mix(){
            foreach(var el in baseOrder){
                if (el.Value == 0){
                    continue;
                }
                Pop(el);
                var next = el.Value > 0 
                    ? Right(el, el.Value%(Len-1))
                    : Left(el, (el.Value%(Len-1)*-1));
                var right = el.Value > 0 ? next.Right : next;
                var left = el.Value > 0 ? next : next.Left;
                PutBetween(el, left, right);
            }
        }

        private CircleObj<long> Right(CircleObj<long> el, long num){
            while(num-->0)
                el = el.Right;
            return el;
        }
        private CircleObj<long> Left(CircleObj<long> el, long num){
            while(num-->0)
                el = el.Left;
            return el;
        }

        private void Pop(CircleObj<long> obj){
            obj.Left.Right = obj.Right;
            obj.Right.Left = obj.Left;
            if (obj == Start)
                Start = obj.Right;
        }

        private void PutBetween(CircleObj<long> obj, CircleObj<long> l, CircleObj<long> r){
            obj.Left = l;
            obj.Right = r;
            l.Right = obj;
            r.Left = obj;
        }

        public void Zero(){
            var ns = Start;
            while(ns.Value != 0)
                ns = ns.Right;
            Start = ns;
        }
    }

}