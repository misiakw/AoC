using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC.Common
{
    public class RollArray<T>
    {
        private Queue<T> inner;
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
