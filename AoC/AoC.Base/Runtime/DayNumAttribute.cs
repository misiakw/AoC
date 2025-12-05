using System;

namespace AoC.Base.Runtime;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class DayNumAttribute(int dayNum) : Attribute
{
    public int DayNum { get; init; } = dayNum;
}