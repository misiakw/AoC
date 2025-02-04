using System;
using System.Threading.Tasks;

namespace AdventOfCode.Interfaces
{
    public interface IDay
    {
        string Task1(IInput input);
        string Task2(IInput input);
    }
}