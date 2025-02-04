using System;
using AdventOfCode.Interfaces;

namespace AdventOfCode.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class InputAttribute: Attribute
    {
        public InputAttribute(string name, Type inputType)
        {
            Name = name;
            Input = (IInput) Activator.CreateInstance(inputType);
        }

        public readonly string Name;
        public readonly IInput Input;
    }
}