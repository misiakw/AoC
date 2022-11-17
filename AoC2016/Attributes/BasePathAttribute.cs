using System;

namespace AoC2016.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class BasePathAttribute: Attribute
    {
        public string Path { get; private set; }

        public BasePathAttribute(string path)
        {
            this.Path = path;
        }
    }
}
