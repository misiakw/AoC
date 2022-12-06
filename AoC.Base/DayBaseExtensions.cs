using System.Collections.Generic;
using System.IO;
using System.Linq;
using TestType = AoC.Base.Input.TestType;

namespace AoC.Base
{
    public abstract partial class DayBase
    {
        public DayBase Input(string name)
        {
            var a = new Input($"./Inputs/Day{dayNum}/{name}.txt", name);
            tests.Add(a);
            return this;
        }
        public DayBase RunPart(byte part) => RunPart<string>(part, TestType.Verbal, null);
        public  DayBase RunPart<T>(byte part, T result = default(T)) => RunPart<T>(part, TestType.Verbal, result);
        private DayBase RunPart<T>(byte part, TestType testType, T result = default(T)){
            if(part < 1 || part > 2)
                throw new InvalidDataException("Parts to run should be 1 or 2");
            tests.Last().Tests[part-1] = testType;

            if(result != null){
                tests.Last().Result[part-1] = Tuple.Create((object?)result, typeof(T));
            }

            return this;
        }
        public DayBase Silent(byte part) => RunPart<string>(part, TestType.Silent, null);
    }
}
