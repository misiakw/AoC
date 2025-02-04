using System;

namespace AdventOfCode.Attributes
{
    public class DayAttribute: Attribute
    {
        public DayAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }
}