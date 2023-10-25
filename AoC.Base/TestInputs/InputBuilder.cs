using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Base.TestInputs
{
    public class InputBuilder<TResult, TInput> where TInput: TestInputBase<TResult>
    {
        private IList<TInput> inputs = new List<TInput>();

        public TInput New(string path, string name)
        {
            TInput input = (TInput)Activator.CreateInstance(typeof(TInput), new object[] { path, name });
            inputs.Add(input);
            return input;
        }

        public TInput[] Build() => inputs.ToArray();
    }
}
