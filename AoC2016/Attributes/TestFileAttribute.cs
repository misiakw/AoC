using System;

namespace AoC2016.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class TestFileAttribute: Attribute
    {
        public string Name { get; set; }
        public string File { get; set; }
        public TestCase TestToProceed { get; set; } = TestCase.All;
    }

    public enum TestCase
    {
        All = 0,
        Part1 = 1,
        Part2 = 2
    }
}
