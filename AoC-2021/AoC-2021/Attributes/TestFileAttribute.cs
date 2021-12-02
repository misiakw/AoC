using System;

namespace AoC_2021.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class TestFileAttribute: Attribute
    {
        public string Name { get; set; }
        public string File { get; set; }
    }
}
