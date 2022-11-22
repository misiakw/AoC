using System.Collections.Generic;
using System.IO;
using System.Linq;
namespace AoC2016
{
    public abstract partial class DayBase
    {
        public DayBase Input(string name)
        {
            var a = new Input($"./Inputs/Day{dayNum}/{name}.txt", name);
            tests.Add(a);
            return this;
        }
        public DayBase RunPart(byte part) => RunPart<string>(part, AoC2016.Input.TestType.Verbal, null);
        public  DayBase RunPart<T>(byte part, T result = default(T)) => RunPart<T>(part, AoC2016.Input.TestType.Verbal, result);
        private DayBase RunPart<T>(byte part, AoC2016.Input.TestType testType, T result = default(T)){
            if(part < 1 || part > 2)
                throw new InvalidDataException("Parts to run should be 1 or 2");
            tests.Last().Tests[part-1] = testType;

            if(result != null){
                tests.Last().Result[part-1] = Tuple.Create((object?)result, typeof(T));
            }

            return this;
        }
        public DayBase Silent(byte part) => RunPart<string>(part, AoC2016.Input.TestType.Silent, null);

        public DayBase TooHigh(byte part, object o){
            if(part < 1 || part > 2)
                throw new InvalidDataException("Parts to run should be 1 or 2");
            tests.Last().FailedResults[part-1, 1] = 0;
            return this;
        }
        public DayBase TooLow(byte part, object o){  if(part < 1 || part > 2)
            if(part < 1 || part > 2)
                throw new InvalidDataException("Parts to run should be 1 or 2");
            tests.Last().FailedResults[part-1, 0] = 0;
            return this;
        }
    }
}
