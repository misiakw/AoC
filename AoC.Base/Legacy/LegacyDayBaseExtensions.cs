using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TestType = AoC.Base.LegacyInput.TestType;

namespace AoC.Base
{
    public abstract partial class LegacyDayBase
    {
        public LegacyDayBase Input(string name)
        {
            var a = new LegacyInput($"./Inputs/Day{dayNum}/{name}.txt", name);
            tests.Add(a);
            return this;
        }
        public LegacyDayBase RunPart(byte part) => RunPart<string>(part, TestType.Verbal, null);
        public  LegacyDayBase RunPart<T>(byte part, T? result = default(T)) => RunPart<T>(part, TestType.Verbal, result);
        private LegacyDayBase RunPart<T>(byte part, TestType testType, T? result = default(T)){
            if(part < 1 || part > 2)
                throw new InvalidDataException("Parts to run should be 1 or 2");
            tests.Last().Tests[part-1] = testType;

            if(result != null){
                tests.Last().SetResult(part, result);
            }

            return this;
        }
        public LegacyDayBase Silent(byte part) => RunPart<string>(part, TestType.Silent, null);
    }
}
