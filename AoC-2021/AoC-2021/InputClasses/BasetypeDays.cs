namespace AoC_2021.InputClasses
{
    public abstract class LongDay : ParseableDay<long>
    {
        public LongDay(string path) : base(path) { }
        public override long Parse(string input)
        {
            return long.Parse(input);
        }
    }
}
