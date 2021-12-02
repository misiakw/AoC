using System.Collections.Generic;

namespace Shared
{
    public interface IDay
    {
        Conf Config { get; }
        string Task1(IList<string> input);
        string Task2(IList<string> input);
    }
}
