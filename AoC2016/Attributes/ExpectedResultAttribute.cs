using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2016.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class ExpectedResultAttribute: Attribute
    {
        public string TestName;
        public string Result;
        public long TooHigh = long.MaxValue;
        public long TooLow = long.MinValue;
    }
}
