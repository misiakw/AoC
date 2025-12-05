using System.Collections.Generic;

namespace AoC.Common
{
    public class RollArray<T>
    {
        private readonly Queue<T> inner;
        public RollArray(IList<T> input)
        {
            inner = new Queue<T>(input);
        }

        public T Pick()
        {
            var tmp = inner.Dequeue();
            inner.Enqueue(tmp);
            return tmp;
        }
    }
}
